using System.Collections.Generic;
using System.Collections.ObjectModel;
using TestExample.Common;
using TestExample.Infrastructure;
using TestExample.Model;

namespace TestExample.ViewModel.TableView
{
    public class ColumnWidthModeViewModel: CustomViewModelBase<IdNameObject>
    {
        public override bool IsWindow => true;

        public override string CustomColumnDisplayTypeKey => "Test";

        public ColumnWidthModeViewModel()
        {
            InitializeColumnDisplayConfigurations();
            InitializeCustomFields();
            RefreshAsync();
        }

        protected override PagedData<IdNameObject> Find()
        {
            var itemsSource = InitializeItemsSource();
            return new PagedData<IdNameObject>(itemsSource,itemsSource.Count);
        }

        protected override void Bind(PagedData<IdNameObject> result)
        {
            DataItems = new ObservableCollection<IdNameObject>(result.Items);
        }

        private List<IdNameObject> InitializeItemsSource()
        {
            return new List<IdNameObject>
            {
                new IdNameObject(11111111,"龙升"),
                new IdNameObject(22222222,"汤龙"),
                new IdNameObject(333333333,"李国栋李国栋李国栋李国栋李国栋李国栋李国栋"),
                new IdNameObject(4,"刘文浩"),
                new IdNameObject(5,"张勇"),
                new IdNameObject(6,"陈驰"),
                new IdNameObject(7,"颜照")
            };
        }
    }
}
