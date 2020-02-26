using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ModelGenerate.Model;
using Newtonsoft.Json;

namespace ModelGenerate
{
    public class MainWindowViewModel: ViewModelBase
    {
        private const string NEWLINE_CODE = "\r\n";
        private const string INDENT_CODE = "\t";
        private const string SOURCECODE_DIRECTORY = "SourceFiles";
        private const string MODELCODE_DIRECTORY = "CodeFiles";

        private readonly Dictionary<string, string> _propertyTemplates = new Dictionary<string, string>()
        {
            {"Mode1",@"
public FieldType PropertyName { get; set; }"},
            {"Mode2",@"
public FieldType PropertyName
{
    get => _fieldName;
    set => _fieldName = value;
}
" },
            {"Mode3",@"
public FieldType PropertyName
{
    get => _fieldName;
    set
    {
        if (_fieldName == value) return;
        _fieldName = value;
        RaisePropertyChanged();
    }
}"}
        };

        private readonly ICommand _addRowCommand;
        private readonly ICommand _deleteRowCommand;
        private readonly ICommand _generateCodeCommand;
        private readonly ICommand _saveCommand;

        private ObservableCollection<PropertyDto> _properties=new ObservableCollection<PropertyDto>();
        private PropertyDto _selectedProperty;
        private string _className;
        private string _nameSpace;

        private bool _modelBaseChecked=true;
        private bool _dataErrorInfoChecked=true;
        private bool _mode1Checked;
        private bool _mode2Checked;
        private bool _mode3Checked;
        private string _modelText;

        public MainWindowViewModel()
        {
            _addRowCommand=new RelayCommand(() =>
            {
                Properties.Add(new PropertyDto());
            });
            _deleteRowCommand = new RelayCommand(() =>
            {
                Properties.Remove(SelectedProperty);
            },()=>SelectedProperty!=null);
            _generateCodeCommand=new RelayCommand(() =>
            {
                if (!Validate())
                {
                    return;
                }

                var nameSpacesToImport = new List<string> { "System", "Newtonsoft.Json" };
                if (DataErrorInfoChecked)
                {
                    nameSpacesToImport.Add("System.ComponentModel");
                }
                if (Properties.Any(a => a.Type.Contains("List")))
                {
                    nameSpacesToImport.Add("System.Collections.Generic");
                }
                if (ModelBaseChecked)
                {
                    nameSpacesToImport.Add("Linkon.ErpClient.Data.Infrastructure");
                }
                GenerateCode(ClassName, nameSpacesToImport, NameSpace,Properties.ToList());
            });
            _saveCommand=new RelayCommand(SaveSourceFile);
            Mode1Checked = true;
        }

        public ICommand AddRowCommand => _addRowCommand;
        public ICommand DeleteRowCommand => _deleteRowCommand;
        public ICommand GenerateCodeCommand => _generateCodeCommand;
        public ICommand SaveCommand => _saveCommand;

        public ObservableCollection<PropertyDto> Properties
        {
            get => _properties;
            set
            {
                if (_properties == value) return;
                _properties = value;
                RaisePropertyChanged(nameof(Properties));
            }
        }

        public PropertyDto SelectedProperty
        {
            get => _selectedProperty;
            set
            {
                if (_selectedProperty == value) return;
                _selectedProperty = value;
                RaisePropertyChanged(nameof(SelectedProperty));
            }
        }

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName
        {
            get => _className;
            set
            {
                if (_className == value) return;
                _className = value;
                RaisePropertyChanged(nameof(ClassName));
            }
        }

        /// <summary>
        /// 命名空间
        /// </summary>
        public string NameSpace
        {
            get => _nameSpace;
            set
            {
                if (_nameSpace == value) return;
                _nameSpace = value;
                RaisePropertyChanged(nameof(NameSpace));
            }
        }

        /// <summary>
        /// 模式一是否选中
        /// </summary>
        public bool Mode1Checked
        {
            get => _mode1Checked;
            set
            {
                if (_mode1Checked == value) return;
                _mode1Checked = value;
                if (_mode1Checked)
                {
                    ModelText = _propertyTemplates["Mode1"];
                }
                RaisePropertyChanged(nameof(Mode1Checked));
            }
        }

        /// <summary>
        /// 模式二是否选中
        /// </summary>
        public bool Mode2Checked
        {
            get => _mode2Checked;
            set
            {
                if (_mode2Checked == value) return;
                _mode2Checked = value;
                if (_mode2Checked)
                {
                    ModelText = _propertyTemplates["Mode2"];
                }
                RaisePropertyChanged(nameof(Mode2Checked));
            }
        }

        /// <summary>
        /// 模式三是否选中
        /// </summary>
        public bool Mode3Checked
        {
            get => _mode3Checked;
            set
            {
                if (_mode3Checked == value) return;
                _mode3Checked = value;
                if (_mode3Checked)
                {
                    ModelText = _propertyTemplates["Mode3"];
                }
                RaisePropertyChanged(nameof(Mode3Checked));
            }
        }

        /// <summary>
        /// 是否需要ModelBase基类
        /// </summary>
        public bool ModelBaseChecked
        {
            get => _modelBaseChecked;
            set
            {
                if (_modelBaseChecked == value) return;
                _modelBaseChecked = value;
                if (!_modelBaseChecked && _mode3Checked)
                {
                    Mode3Checked = false;
                    Mode1Checked = true;
                }
                RaisePropertyChanged(nameof(ModelBaseChecked));
            }
        }

        /// <summary>
        /// 是否需要实现IDataErrorInfo接口
        /// </summary>
        public bool DataErrorInfoChecked
        {
            get => _dataErrorInfoChecked;
            set
            {
                if (_dataErrorInfoChecked == value) return;
                _dataErrorInfoChecked = value;
                RaisePropertyChanged(nameof(DataErrorInfoChecked));
            }
        }

        /// <summary>
        /// 属性示例文本
        /// </summary>
        public string ModelText
        {
            get => _modelText;
            set
            {
                if (_modelText == value) return;
                _modelText = value;
                RaisePropertyChanged(nameof(ModelText));
            }
        }

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="className"></param>
        /// <param name="nameSpacesToImport"></param>
        /// <param name="nameSpace"></param>
        /// <param name="propertyDtos"></param>
        private void GenerateCode(string className,List<string> nameSpacesToImport,string nameSpace,List<PropertyDto> propertyDtos)
        {
            var outputFile = $"{className}.cs";
            var generatedCode = string.Empty;
            //导入命名控件
            foreach (var nameSpaceToImport in nameSpacesToImport)
            {
                if(string.IsNullOrEmpty(nameSpaceToImport))continue;
                generatedCode += $"using {nameSpaceToImport};";
                generatedCode = IndentCode(generatedCode, 0);
            }
            generatedCode += NEWLINE_CODE;
            //当前类命名控件
            generatedCode += $"namespace {nameSpace}";
            generatedCode = IndentCode(generatedCode, 0);
            generatedCode += "{";
            generatedCode=IndentCode(generatedCode,1);
            //类及基类、接口定义
            generatedCode += $"public class {ClassName}";
            if (ModelBaseChecked)
            {
                generatedCode += " : ModelBase";
            }
            if (DataErrorInfoChecked)
            {
                if (ModelBaseChecked)
                {
                    generatedCode += ", IDataErrorInfo";
                }
                else
                {
                    generatedCode += " : IDataErrorInfo";
                }
            }
            generatedCode = IndentCode(generatedCode, 1);
            generatedCode += "{";
            //生成成员变量
            if (!Mode1Checked)
            {
                //换行
                generatedCode = IndentCode(generatedCode, 0);
                generatedCode = GenerateFieldsCode(generatedCode, propertyDtos, 2);
            }
            //生成属性
            generatedCode = GeneratePropertiesCode(generatedCode, propertyDtos, 2);
            //实现接口
            if (DataErrorInfoChecked)
            {
                generatedCode = ImplementInterfaces(generatedCode, 2);
            }
            generatedCode = IndentCode(generatedCode, 1);
            generatedCode += "}";
            generatedCode = IndentCode(generatedCode, 0);
            generatedCode += "}";
            //写入到cs类文件
            try
            {
                var directory = new DirectoryInfo(MODELCODE_DIRECTORY);
                if (!directory.Exists)
                {
                    directory.Create();
                }
                using (var sw = new StreamWriter($"{MODELCODE_DIRECTORY}/{outputFile}"))
                {
                    sw.Write(generatedCode);
                }
                MessageBox.Show("生成成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 生成字段
        /// </summary>
        /// <param name="code">代码段</param>
        /// <param name="propertyDtos">属性集合</param>
        /// <param name="indentTimes">缩进次数</param>
        /// <returns></returns>
        private string GenerateFieldsCode(string code, List<PropertyDto> propertyDtos, int indentTimes)
        {
            foreach (var propertyDto in propertyDtos)
            {
                //缩进
                code = IndentCode(code, indentTimes,false);
                code += $"private {propertyDto.Type} _{propertyDto.Name.Substring(0,1).ToLower()}{propertyDto.Name.Substring(1, propertyDto.Name.Length-1)};";
                //换行
                code = IndentCode(code, 0);
            }
            return code;
        }

        /// <summary>
        /// 生成属性
        /// </summary>
        /// <param name="code">代码段</param>
        /// <param name="propertyDtos">属性集合</param>
        /// <param name="indentTimes">缩进次数</param>
        /// <returns></returns>
        private string GeneratePropertiesCode(string code, List<PropertyDto> propertyDtos, int indentTimes)
        {
            foreach (var propertyDto in propertyDtos)
            {
                if (Mode1Checked)
                {
                    code = GeneratePropertyCodeByMode1(code, propertyDto, indentTimes);
                }else if (Mode2Checked)
                {
                    code = GeneratePropertyCodeByMode2(code, propertyDto, indentTimes);
                }else if (Mode3Checked)
                {
                    code = GeneratePropertyCodeByMode3(code, propertyDto, indentTimes);
                }
            }
            return code;
        }

        /// <summary>
        /// 模式一属性生成
        /// </summary>
        /// <param name="code">代码段</param>
        /// <param name="propertyDto">属性</param>
        /// <param name="indentTimes">缩进次数</param>
        /// <returns></returns>
        private string GeneratePropertyCodeByMode1(string code,PropertyDto propertyDto, int indentTimes)
        {
            code = IndentCode(code, 0);
            code = GenerateSummary(code, propertyDto.Remark, indentTimes);
            code = GeneratePropertyAttribute(code,propertyDto.NameInServer, indentTimes);
            code = IndentCode(code, indentTimes, false);
            code += $"public {propertyDto.Type} {propertyDto.Name} "+"{ get; set; }";
            code = IndentCode(code, 0);
            return code;
        }

        /// <summary>
        /// 模式二属性生成
        /// </summary>
        /// <param name="code"></param>
        /// <param name="propertyDto"></param>
        /// <param name="indentTimes"></param>
        /// <returns></returns>
        private string GeneratePropertyCodeByMode2(string code, PropertyDto propertyDto, int indentTimes)
        {
            var fieldName = $"_{propertyDto.Name.Substring(0, 1).ToLower()}{propertyDto.Name.Substring(1, propertyDto.Name.Length - 1)}";

            code = IndentCode(code, 0);
            code = GenerateSummary(code, propertyDto.Remark, indentTimes);
            code = GeneratePropertyAttribute(code, propertyDto.NameInServer, indentTimes);
            code = IndentCode(code, indentTimes,false);
            code += $"public {propertyDto.Type} {propertyDto.Name}";
            code = IndentCode(code, indentTimes);
            code += "{";
            code = IndentCode(code, indentTimes + 1);
            code += $"get => {fieldName};";
            code = IndentCode(code, indentTimes + 1);
            code += $"set => {fieldName} = value;";
            code = IndentCode(code, indentTimes);
            code += "}";
            code = IndentCode(code, 0);
            return code;
        }

        /// <summary>
        /// 模式三属性生成
        /// </summary>
        /// <param name="code"></param>
        /// <param name="propertyDto"></param>
        /// <param name="indentTimes"></param>
        /// <returns></returns>
        private string GeneratePropertyCodeByMode3(string code, PropertyDto propertyDto, int indentTimes)
        {
            var fieldName = $"_{propertyDto.Name.Substring(0, 1).ToLower()}{propertyDto.Name.Substring(1, propertyDto.Name.Length - 1)}";
            //换行
            code = IndentCode(code, 0);
            //生成描述
            code = GenerateSummary(code, propertyDto.Remark, indentTimes);
            //生成特性
            code = GeneratePropertyAttribute(code, propertyDto.NameInServer, indentTimes);
            code = IndentCode(code, indentTimes,false);
            code += $"public {propertyDto.Type} {propertyDto.Name}";
            code = IndentCode(code, indentTimes);
            code += "{";
            code = IndentCode(code, indentTimes + 1);
            code += $"get => {fieldName};";
            code = IndentCode(code, indentTimes + 1);
            code += "set";
            code = IndentCode(code, indentTimes + 1);
            code += "{";
            code = IndentCode(code, indentTimes + 2);
            code += $"if ({fieldName} == value" + ") return;";
            code = IndentCode(code, indentTimes + 2);
            code += $"{fieldName} = value;";
            code = IndentCode(code, indentTimes + 2);
            code += "RaisePropertyChanged();";
            code = IndentCode(code, indentTimes + 1);
            code += "}";
            code = IndentCode(code, indentTimes);
            code += "}";
            code = IndentCode(code, 0);
            return code;
        }

        /// <summary>
        /// 实现接口
        /// </summary>
        /// <param name="code"></param>
        /// <param name="indentTimes"></param>
        /// <returns></returns>
        private string ImplementInterfaces(string code,int indentTimes)
        {
            //留一行空白
            code = IndentCode(code, 0);
            //实现IDataErrorInfo接口
            code = IndentCode(code, indentTimes, false);
            code += "[JsonIgnore]";
            code = IndentCode(code, indentTimes);
            code += "public string Error => null;";

            code += NEWLINE_CODE;

            code = IndentCode(code, indentTimes);
            code += "[JsonIgnore]";
            code = IndentCode(code, indentTimes);
            code += "public string this[string columnName] => IsValid(columnName);";

            code += NEWLINE_CODE;

            code = IndentCode(code, 2);
            code += "private string IsValid(string propertyName)";
            code = IndentCode(code, 2);
            code += "{";
            code = IndentCode(code, 3);
            code += "return null;";
            code = IndentCode(code, 2);
            code += "}";
            return code;
        }

        /// <summary>
        /// 缩进换行（允许组合搭配）
        /// </summary>
        /// <param name="code"></param>
        /// <param name="indentTimes">缩进次数</param>
        /// <param name="needNewLine">是否换行</param>
        /// <returns></returns>
        private string IndentCode(string code,int indentTimes,bool needNewLine=true)
        {
            if (needNewLine)
            {
                code += NEWLINE_CODE;
            }
            for (var i = 0; i < indentTimes; i++)
            {
                code += INDENT_CODE;
            }
            return code;
        }

        /// <summary>
        /// 生成描述
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="remark">备注</param>
        /// <param name="indentTimes">缩进次数</param>
        /// <returns></returns>
        private string GenerateSummary(string code, string remark, int indentTimes)
        {
            if (string.IsNullOrEmpty(remark)) return code;
            //缩进
            code = IndentCode(code, indentTimes,false);
            code += "/// <summary>";
            code = IndentCode(code, indentTimes);
            code += $"/// {remark}";
            code = IndentCode(code, indentTimes);
            code += "/// <summary>";
            //换行
            code = IndentCode(code,0);
            return code;
        }

        /// <summary>
        /// 生成JsonProperty特性
        /// </summary>
        /// <param name="code"></param>
        /// <param name="serverName"></param>
        /// <param name="indentTimes"></param>
        /// <returns></returns>
        private string GeneratePropertyAttribute(string code, string serverName, int indentTimes)
        {
            if (serverName == null) return code;
            //缩进
            code = IndentCode(code, indentTimes, false);
            code += $"[JsonProperty(\"{serverName}\")]";
            //换行
            code = IndentCode(code,0);
            return code;
        }

        /// <summary>
        /// 保存源文件
        /// </summary>
        private void SaveSourceFile()
        {
            var classDto = new ClassDto
            {
                ClassName = _className,
                NameSpace = _nameSpace,
                BaseClassEnabled = ModelBaseChecked,
                InterfaceEnabled = DataErrorInfoChecked,
                Mode1Checked = Mode1Checked,
                Mode2Checked = Mode2Checked,
                Mode3Checked = Mode3Checked,
                PropertyDtos = _properties?.ToList()
            };
            var json = JsonConvert.SerializeObject(classDto);

            try
            {
                var directory = new DirectoryInfo(SOURCECODE_DIRECTORY);
                if (!directory.Exists)
                {
                    directory.Create();
                }
                using (var sw = new StreamWriter($"{SOURCECODE_DIRECTORY}/{_className}.txt"))
                {
                    sw.Write(json);
                }
                MessageBox.Show("保存源文件成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <returns></returns>
        private bool Validate()
        {
            if (string.IsNullOrEmpty(ClassName))
            {
                MessageBox.Show("类名不能为空！", "提示", MessageBoxButton.OK);
                return false;
            }
            if (string.IsNullOrEmpty(NameSpace))
            {
                MessageBox.Show("命名空间不能为空！", "提示", MessageBoxButton.OK);
                return false;
            }
            var hasEmptyName = Properties.Any(a => string.IsNullOrEmpty(a.Name));
            if (hasEmptyName)
            {
                MessageBox.Show("属性名不能为空！", "提示", MessageBoxButton.OK);
                return false;
            }
            var hasEmptyType = Properties.Any(a => string.IsNullOrEmpty(a.Type));
            if (hasEmptyType)
            {
                MessageBox.Show("属性类型不能为空！", "提示", MessageBoxButton.OK);
                return false;
            }
            return true;
        }
    }
}