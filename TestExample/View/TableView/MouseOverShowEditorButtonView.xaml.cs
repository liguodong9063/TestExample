using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using TestExample.Common;
using TestExample.Utilities;

namespace TestExample.View.TableView
{
    /// <summary>
    /// FocusShowEditButtonView.xaml 的交互逻辑
    /// </summary>
    public partial class MouseOverShowEditorButtonView : Window
    {
        public MouseOverShowEditorButtonView()
        {
            InitializeComponent();
            GridControl.ItemsSource = new ObservableCollection<IdNameObject>
            {
                new IdNameObject(1,"张三"),
                new IdNameObject(2,"李四"),
                new IdNameObject(3,"王五")
            };
        }

        private void ShowEditor_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button)) return;
            button.Tag = 1;
            var grid = VisualSearchHelper.GetParentObject<Grid>(button, "ParentGrid");
            var textBox = VisualSearchHelper.GetChildObject<TextBox>(grid, "EditorBox");
            textBox.Focus();
            textBox.SelectionStart = 0;
            textBox.SelectionLength = textBox.Text.Length;
        }

        private void TextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            var grid=VisualSearchHelper.GetParentObject<Grid>(textBox,"ParentGrid");
            var button=VisualSearchHelper.GetChildObject<Button>(grid, "EditorButton");
            button.Tag = 0;
        }
    }
}