using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using NLog;

namespace TestExample.Utilities
{
    public class ObservableCollectionHelper<T> : IList, ITypedList
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private PropertyDescriptorCollection _columnCollection;

        public void SetPropertyValue(int rowIndex, int columnIndex, object value, string name)
        {
            var t = Obs[rowIndex];
            _type = typeof(T);

            if (value == null)
            {
                _type.GetProperty(name).SetValue(t, null, null);
            }
            else
            {
                var pType = _type.GetProperty(name).PropertyType;

                if (pType.IsGenericType && pType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    var nullType = new NullableConverter(pType);
                    pType = nullType.UnderlyingType;
                }

                _type.GetProperty(name).SetValue(t, Convert.ChangeType(value, pType), null);
            }

        }

        public object GetPropertyValue(int rowIndex, int columnIndex, string name)
        {
            var t = Obs[rowIndex];
            _type = typeof(T);
            var propertyInfo = _type.GetProperty(name);
            try
            {
                return propertyInfo.GetValue(t, null);
            }
            catch (Exception ex)
            {
                Logger.Info(ex.Message);
                return null;
            }
        }
        private Type _type;
        private int _count;
        public ObservableCollection<T> Obs { get; set; }
        public ObservableCollectionHelper<T> Ini(ObservableCollection<T> obc)
        {
            Obs = obc;
            _count = obc.Count;
            _type = typeof(T);

            PropertyInfo[] propertyInfos = _type.GetProperties();

            int properCounts = propertyInfos.Length;

            var pds = new PropertyDescriptorHelper<T>[properCounts];
            for (int i = 0; i <= properCounts - 1; i++)
            {
                pds[i] = new PropertyDescriptorHelper<T>(this, i, propertyInfos[i].Name, propertyInfos[i].PropertyType, false);
            }
            _columnCollection = new PropertyDescriptorCollection(pds);
            return this;
        }


        PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] decs)
        {
            return _columnCollection;
        }

        string ITypedList.GetListName(PropertyDescriptor[] descs)
        {
            return "";
        }


        public virtual int Count
        {
            get { return _count; }
        }

        public virtual bool IsSynchronized
        {
            get { return true; }
        }

        public virtual object SyncRoot
        {
            get { return true; }
        }

        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        public virtual bool IsFixedSize
        {
            get { return true; }
        }

        public virtual IEnumerator GetEnumerator()
        {
            return null;
        }

        public virtual void CopyTo(Array array, int fIndex)
        {

        }

        public virtual int Add(object val)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }


        object IList.this[int fIndex]
        {
            get { return fIndex; }
            set { }
        }
    }


    public struct Location
    {
        public int Row { get; set; }
        public string Name { get; set; }
    }
}
