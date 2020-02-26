using System.Collections.Generic;
using DevExpress.Data.Filtering;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid.LookUp;

namespace TestExample.Utilities
{
    public class CustomLookUpEdit: LookUpEdit
    {
        protected override EditStrategyBase CreateEditStrategy()
        {
            return new MyLookUpEditStrategy(this);
        }
    }

    class MyLookUpEditStrategy : LookUpEditStrategy
    {
        public MyLookUpEditStrategy(LookUpEdit editor)
            : base(editor)
        {
        }
        public override void UpdateDisplayFilter()
        {
            ItemsProvider.SetDisplayFilterCriteria(CreateDisplayFilterCriteria(AutoSearchText), CurrentDataViewHandle);
        }
        protected CriteriaOperator CreateDisplayFilterCriteria(string text)
        {
            GroupOperator groupOperator = new GroupOperator() { OperatorType = GroupOperatorType.Or };
            List<string> searchProperties = new List<string> { "Id", "Name"};
            foreach (string prop in searchProperties)
            {
                List<CriteriaOperator> list = new List<CriteriaOperator>();
                list.Add(new OperandProperty() { PropertyName = prop });
                list.Add(new OperandValue() { Value = text });
                groupOperator.Operands.Add(new FunctionOperator(FunctionOperatorType.Contains, list));
            }
            return groupOperator;
        }
    }
}
