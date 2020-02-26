using Newtonsoft.Json;

namespace TestExample.Model
{
    public class CustomColumnDispaly : ModelBase
    {
        private int? _id;
        private string _coluType;
        private string _noShowColu;
        private int _userId;
        private int? _version;

        /// <summary>
        ///     ID
        /// </summary>
        [JsonProperty("Id")]
        public int? Id
        {
            get { return _id; }
            set
            {
                if (_id == value) return;
                _id = value;
                RaisePropertyChanged("Id");
            }
        }

        /// <summary>
        ///     列所在表格的标识
        /// </summary>
        [JsonProperty("ColuType")]
        public string ColuType
        {
            get { return _coluType; }
            set
            {
                if (_coluType == value) return;
                _coluType = value;
                RaisePropertyChanged("ColuType");
            }
        }

        /// <summary>
        ///     隐藏的列的集合文本
        /// </summary>
        [JsonProperty("NoShowColu")]
        public string NoShowColu
        {
            get { return _noShowColu; }
            set
            {
                if (_noShowColu == value) return;
                _noShowColu = value;
                RaisePropertyChanged("NoShowColu");
            }
        }

        /// <summary>
        ///     用户ID
        /// </summary>
        [JsonProperty("UserId")]
        public int UserId
        {
            get { return _userId; }
            set
            {
                if (_userId == value) return;
                _userId = value;
                RaisePropertyChanged("UserId");
            }
        }

        /// <summary>
        ///     版本
        /// </summary>
        [JsonProperty("Version")]
        public int? Version
        {
            get { return _version; }
            set
            {
                if (_version == value) return;
                _version = value;
                RaisePropertyChanged("Version");
            }
        }
    }
}
