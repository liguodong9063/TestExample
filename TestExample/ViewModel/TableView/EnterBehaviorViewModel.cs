using System.Collections.Generic;
using GalaSoft.MvvmLight;
using TestExample.Common;
using TestExample.Utilities;

namespace TestExample.ViewModel.TableView
{
    public class EnterBehaviorViewModel : ViewModelBase
    {
        private IdNameSuggestionProvider _suggestionProvider;

        public EnterBehaviorViewModel()
        {
            _suggestionProvider=new IdNameSuggestionProvider(new List<IdNameObject>
            {
                new IdNameObject(1,"颜照"),
                new IdNameObject(2,"李荣"),
                new IdNameObject(3,"李国栋"),
                new IdNameObject(4,"刘文浩"),
                new IdNameObject(5,"龙升"),
                new IdNameObject(6,"汤龙"),
                new IdNameObject(7,"张三"),
                new IdNameObject(8,"李四"),
                new IdNameObject(9,"王五"),
                new IdNameObject(10,"王亚"),
            });
        }

        public IdNameSuggestionProvider SuggestionProvider
        {
            get => _suggestionProvider;
            set
            {
                if (_suggestionProvider == value) return;
                _suggestionProvider = value;
                RaisePropertyChanged(nameof(SuggestionProvider));
            }
        }
    }
}
