using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TestExample.Model.AutoGenerateColumn;

namespace TestExample.Utilities
{
    public class ObservableCollectionHelperForAutoGenerate : IList, ITypedList
    {
        private Type _rowType;
        private int _rowCount;

        private PropertyDescriptorCollection _columnCollection;

        public void SetPropertyValue(int rowIndex, int columnIndex, object value, string name)
        {
            var t = Obs[rowIndex];
            _rowType = t.GetType();

            var property = _rowType.GetProperty(name);
            if (property != null)
            {
                property.SetValue(t, Convert.ChangeType(value, property.PropertyType), null);
            }
        }

        public object GetPropertyValue(int rowIndex, int columnIndex, string name)
        {
            var t = Obs[rowIndex];
            foreach (var columnData in t.Columns)
            {
                if (columnData.FieldName.ToLower() == name.ToLower())
                {
                    return columnData.FieldValue;
                }
            }
            return "";
        }

        public ObservableCollection<CustomRow> Obs { get; set; }

        public void Ini(ObservableCollection<CustomRow> obc, IList<CustomHeader> headers)
        {
            Obs = obc;
            _rowCount = Obs.Count;

            var pds = new PropertyDescriptor[headers.Count];
            _rowType = typeof(CustomRow);

            for (var i = 0; i <= headers.Count - 1; i++)
            {
                pds[i] = new PropertyDescriptorHelperForAutoGenerate(this, i, headers[i].FieldName, headers[i].GetType(), false);
            }
            _columnCollection = new PropertyDescriptorCollection(pds);
        }

        PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] desc)
        {
            return _columnCollection;
        }

        string ITypedList.GetListName(PropertyDescriptor[] desc)
        {
            return "";
        }

        public virtual int Count => _rowCount;

        public virtual bool IsSynchronized => true;

        public virtual object SyncRoot => true;

        public virtual bool IsReadOnly => false;

        public virtual bool IsFixedSize => true;

        public virtual IEnumerator GetEnumerator()
        {
            return null;
        }

        public virtual void CopyTo(Array array, int fIndex)
        {
        }

        public virtual int Add(object val)
        {
            var newRow = val as CustomRow;
            Obs.Add(newRow);
            _rowCount = Obs.Count;
            return _rowCount - 1;
        }

        public virtual bool Contains(object val)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public virtual int IndexOf(object val)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public virtual void Remove(object val)
        {
            var newRow = val as CustomRow;
            Obs.Remove(newRow);
            _rowCount = Obs.Count;
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        object IList.this[int fIndex]
        {
            get => fIndex;
            set { }
        }
    }
}