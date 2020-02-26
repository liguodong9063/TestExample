using System.Windows.Controls;
using System.Windows.Data;

namespace TestExample.Infrastructure.Bindings
{
    public class ValidationBinding : Binding
    {
        public ValidationBinding()
            : this(null)
        {
        }

        public ValidationBinding(string path) : base(path)
        {
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            NotifyOnValidationError = true;
            ValidationRules.Add(new DataErrorValidationRule { ValidatesOnTargetUpdated = false });
        }
    }
}
