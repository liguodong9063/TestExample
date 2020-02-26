namespace ModelGenerate.Model
{
    public class PropertyDto:ModelBase
    {
        private string _name;
        private string _type;
        private string _nameInServer;
        private string _remark;

        /// <summary>
        /// 属性名
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 属性类型
        /// </summary>
        public string Type
        {
            get => _type;
            set
            {
                if (_type == value) return;
                _type = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 服务端名称
        /// </summary>
        public string NameInServer
        {
            get => _nameInServer;
            set
            {
                if (_nameInServer == value) return;
                _nameInServer = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 注释
        /// </summary>
        public string Remark
        {
            get => _remark;
            set
            {
                if (_remark == value) return;
                _remark = value;
                RaisePropertyChanged();
            }
        }
    }
}