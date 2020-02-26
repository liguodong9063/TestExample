using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using TestExample.Utilities;

namespace TestExample.View.TableView
{
    /// <summary>
    /// BigDataView.xaml 的交互逻辑
    /// </summary>
    public partial class BigDataView : Window
    {
        public BigDataView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 第一行Code,第二行记录数，第三行错误消息，第四行到最后数据（每行数据开头和结尾均使用“~”，每列间隔使用“^”）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog { Title = "选择文件", Filter = "文本|*.txt" };
            if (openFileDialog.ShowDialog() != true)
            {
                e.Handled = true;
                return;
            }
            var fileName = openFileDialog.FileName.Trim();

            var startTime = DateTime.Now;

            //using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            //{
            //    using (var sr = new StreamReader(fs, Encoding.UTF8))
            //    {

            //        //utf-8格式，下面的是gb2312格式
            //        //StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
            //        var rows = new List<RelatedCompanyDto>();
            //        var strLine = sr.ReadLine();
            //        string[] columnFieldNames = { };
            //        if (!string.IsNullOrEmpty(strLine))
            //        {
            //            columnFieldNames = strLine.Split('^');
            //        }

            //        if (columnFieldNames.Length > 0)
            //        {
            //            strLine = sr.ReadLine();

            //            while (!string.IsNullOrEmpty(strLine))
            //            {
            //                var rowDto = new RelatedCompanyDto();
            //                var columnDatas = strLine.Split('^');

            //                if (columnDatas.Length != columnFieldNames.Length)
            //                {
            //                    throw new Exception("数据列数量错误！");
            //                }

            //                for (var i = 0; i < columnFieldNames.Length; i++)
            //                {
            //                    ReflectHelper.SetPropertyValue(rowDto, columnFieldNames[i], columnDatas[i]);
            //                }
            //                rows.Add(rowDto);

            //                strLine = sr.ReadLine();
            //            }
            //        }
            //    }
            //}

            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    
                    var responseCode = string.Empty;
                    var totalSize = 0;
                    var errorMessage = string.Empty;

                    string[] columnFieldNames = { };
                    var strLine = sr.ReadLine();
                    responseCode = strLine;
                    strLine = sr.ReadLine();
                    int.TryParse(strLine, out totalSize);
                    strLine = sr.ReadLine();
                    errorMessage = strLine;
                    strLine = sr.ReadLine();
                    if (!string.IsNullOrEmpty(strLine))
                    {
                        columnFieldNames = strLine.Split('^');
                    }

                    //todo:method1
                    var rowDataArray = new string[totalSize, columnFieldNames.Length];
                    strLine = sr.ReadLine();

                    var index = 0;
                    while (!string.IsNullOrEmpty(strLine))
                    {
                        var message = strLine;
                        while (!message.EndsWith("~"))
                        {
                            strLine = sr.ReadLine();
                            message += strLine;
                        }

                        message = message.Substring(1, message.Length - 2);

                        var columnDataArray = message.Split('^');
                        if (columnDataArray.Length != columnFieldNames.Length)
                        {
                            throw new Exception("数据列数量错误！");
                        }

                        for (var i = 0; i < columnFieldNames.Length; i++)
                        {
                            rowDataArray[index, i] = columnDataArray[i];
                        }

                        strLine = sr.ReadLine();
                        index++;
                    }

                    //todo:method2
                    //var str=sr.ReadToEnd();
                    //var startTime2 = DateTime.Now;
                    //var rowDataArray = new string[totalSize, columnFieldNames.Length];
                    //var matches=Regex.Matches(str, "(?<=~).+?(?=~)");

                    //var size = matches.Count;

                    //var endTime2 = DateTime.Now;
                    //Console.WriteLine(endTime2-startTime2);

                    //if (matches.Count > 0)
                    //{
                    //    for (var t = 0; t < matches.Count; t++)
                    //    {
                    //        var columnDataArray = matches[t].Value.Split('^');
                    //        if (columnDataArray.Length != columnFieldNames.Length)
                    //        {
                    //            throw new Exception("数据列数量错误！");
                    //        }

                    //        for (var i = 0; i < columnFieldNames.Length; i++)
                    //        {
                    //            rowDataArray[t,i] = columnDataArray[i];
                    //        }
                    //    }
                    //}

                    //todo:method3
                    //var rows = new string[columnFieldNames.Length,100000];
                    //if (columnFieldNames.Length > 0)
                    //{
                    //    strLine = sr.ReadLine();

                    //    while (!string.IsNullOrEmpty(strLine))
                    //    {
                    //        var rowDto = new RelatedCompanyDto();
                    //        var columnDatas = strLine.Split('^');

                    //        if (columnDatas.Length != columnFieldNames.Length)
                    //        {
                    //            throw new Exception("数据列数量错误！");
                    //        }

                    //        for (var i = 0; i < columnFieldNames.Length; i++)
                    //        {
                    //            ReflectHelper.SetPropertyValue(rowDto, columnFieldNames[i], columnDatas[i]);
                    //        }
                    //        rows.Add(rowDto);

                    //        strLine = sr.ReadLine();
                    //    }
                    //}
                }
            }

            var endTime = DateTime.Now;

            Console.WriteLine($"解析耗时：{endTime-startTime}");

        }

        public static string StringToUnicode(string s)//字符串转UNICODE代码
        {

            char[] charbuffers = s.ToCharArray();

            byte[] buffer;

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < charbuffers.Length; i++)

            {

                buffer = System.Text.Encoding.Unicode.GetBytes(charbuffers[i].ToString());

                sb.Append(String.Format("\\u{0:X2}{1:X2}", buffer[1], buffer[0]));

            }

            return sb.ToString();

        }
    }

    public class RelatedCompanyDto
    {
        public int Id { get; set; }
        public string EntNo { get; set; }
        public string EntName { get; set; }
        public string Pinyin { get; set; }
        public string PinyinSimple { get; set; }
        public int OrgId { get; set; }
        public int? Pid { get; set; }
        public string EntType { get; set; }
        public int CurrencyType { get; set; }
        public string DevlopMan { get; set; }
        public DateTime DevlopTime { get; set; }
        public int? BusiMan { get; set; }
        public string CreditLevel { get; set; }
        public int? CreditDays { get; set; }
        public decimal? CreditMoney { get; set; }
        public int IsUse { get; set; }
        public decimal? AdjMoney { get; set; }
        public string CollationCode { get; set; }
        public string Ecname { get; set; }
        public string EcLocation { get; set; }
        public string EcPhone { get; set; }
        public int? EcStatus { get; set; }
        public int? EcShowable { get; set; }
        public decimal? Qty { get; set; }
        public decimal? MQty { get; set; }
        public decimal? PreferentialPrice { get; set; }
        public string Remark { get; set; }
        public int CrtOpr { get; set; }
        public DateTime? CrtDate { get; set; }
        public int LstEdtOpr { get; set; }
        public DateTime? LstEdtDate { get; set; }
        public int Version { get; set; }
    }
}
