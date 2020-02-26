using System.Windows;
using System.Windows.Input;
using TestExample.ViewModel.LookUpEdit;

namespace TestExample.View.LookUpEdit
{
    /// <summary>
    /// LookUpEditView.xaml 的交互逻辑
    /// </summary>
    public partial class LookUpEditView : Window
    {
        public LookUpEditView()
        {
            InitializeComponent();
            this.DataContext = new LookUpEditViewModel();
        }

        private void LookUpEdit_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                LookUpEdit.EditValue = null;
                e.Handled = true;
            }
        }
    }
}
