using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ModelGenerate.Model
{
    public class ModelBase
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
