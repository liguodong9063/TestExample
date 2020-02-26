using System;
using System.ComponentModel;

namespace TestExample.Utilities
{
    public class PropertyDescriptorHelper<T> : PropertyDescriptor
    {
        private string propertyName;
        private Type propertyType;
        private bool isReadOnly;

        private ObservableCollectionHelper<T> list;

        private int index;

        public PropertyDescriptorHelper(ObservableCollectionHelper<T> list, int index, string propertyName,
            Type propertyType,
            bool isReadOnly)
            : base(propertyName, null)
        {
            this.propertyName = propertyName;
            this.propertyType = propertyType;
            this.isReadOnly = isReadOnly;
            this.list = list;
            this.index = index;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override object GetValue(object component)
        {
            return list.GetPropertyValue((int)component, index, propertyName);
        }

        public override void ResetValue(object component)
        {
        }

        public override void SetValue(object component, object value)
        {
            list.SetPropertyValue((int)component, index, value, propertyName);
        }

        public override bool IsReadOnly => isReadOnly;

        public override string Name => propertyName;

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        public override Type ComponentType => typeof(ObservableCollectionHelper<T>);

        public override Type PropertyType => propertyType;
    }
}
