using System.Collections.Generic;

namespace ModelGenerate.Model
{
    public class ClassDto:ModelBase
    {
        private string _className;
        private string _nameSpace;
        private bool _baseClassEnabled;
        private bool _interfaceEnabled;
        private bool _mode1Checked;
        private bool _mode2Checked;
        private bool _mode3Checked;
        private List<PropertyDto> _propertyDtos;

        public string ClassName
        {
            get => _className;
            set
            {
                if (_className == value) return;
                _className = value;
                RaisePropertyChanged();
            }
        }

        public string NameSpace
        {
            get => _nameSpace;
            set
            {
                if (_nameSpace == value) return;
                _nameSpace = value;
                RaisePropertyChanged();
            }
        }

        public bool BaseClassEnabled
        {
            get => _baseClassEnabled;
            set
            {
                if (_baseClassEnabled == value) return;
                _baseClassEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool InterfaceEnabled
        {
            get => _interfaceEnabled;
            set
            {
                if (_interfaceEnabled == value) return;
                _interfaceEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool Mode1Checked
        {
            get => _mode1Checked;
            set
            {
                if (_mode1Checked == value) return;
                _mode1Checked = value;
                RaisePropertyChanged();
            }
        }

        public bool Mode2Checked
        {
            get => _mode2Checked;
            set
            {
                if (_mode2Checked == value) return;
                _mode2Checked = value;
                RaisePropertyChanged();
            }
        }

        public bool Mode3Checked
        {
            get => _mode3Checked;
            set
            {
                if (_mode3Checked == value) return;
                _mode3Checked = value;
                RaisePropertyChanged();
            }
        }

        public List<PropertyDto> PropertyDtos
        {
            get => _propertyDtos;
            set
            {
                if (_propertyDtos == value) return;
                _propertyDtos = value;
                RaisePropertyChanged();
            }
        }
    }
}
