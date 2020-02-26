using System;
using System.ComponentModel;

namespace TestExample.Utilities
{
    public class PropertyDescriptorHelperForAutoGenerate: PropertyDescriptor
    {
        private readonly int _index;
        private readonly string _propertyName;
        private readonly Type _propertyType;
        private readonly bool _isReadOnly;

        private readonly ObservableCollectionHelperForAutoGenerate _list;

        public PropertyDescriptorHelperForAutoGenerate(ObservableCollectionHelperForAutoGenerate list, int index, string propertyName, Type propertyType, bool isReadOnly)
            : base(propertyName, null)
        {
            _propertyName = propertyName;
            _propertyType = propertyType;
            _isReadOnly = isReadOnly;
            _list = list;
            _index = index;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            return _list.GetPropertyValue((int)component, _index, _propertyName);
        }

        public override void ResetValue(object component)
        {
        }

        public override void SetValue(object component, object value)
        {
            _list.SetPropertyValue((int)component, _index, value, _propertyName);
        }

        public override bool IsReadOnly => _isReadOnly;

        public override string Name => _propertyName;

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        public override Type ComponentType => typeof(ObservableCollectionHelperForAutoGenerate);

        public override Type PropertyType => _propertyType;
    }
}