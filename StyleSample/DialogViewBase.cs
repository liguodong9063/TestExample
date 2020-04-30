using System.Windows;
using System.Windows.Media;

namespace StyleSample
{
    public class DialogViewBase : Window
    {
        public DialogViewBase()
        {
            var color = Color.FromRgb(245, 245, 245);
            ShowInTaskbar = false;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ResizeMode = ResizeMode.NoResize;
            SizeToContent = SizeToContent.WidthAndHeight;
            Background = new SolidColorBrush(color);
            Owner = FindOwner();
            Loaded += DialogViewBase_Loaded;
            Unloaded += DialogViewBase_Unloaded;
        }

        private void DialogViewBase_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterMessages();
        }

        private void DialogViewBase_Unloaded(object sender, RoutedEventArgs e)
        {
            UnregisterMessages();
        }

        protected virtual void RegisterMessages()
        {
        }

        protected virtual void UnregisterMessages()
        {
        }

        private Window FindOwner()
        {
            for (var i = Application.Current.Windows.Count - 1; i >= 0; i--)
            {
                var window = Application.Current.Windows[i];
                if (window == null || window == this || window.ToString().Contains("AdornerLayerWindow") || window.ToString().Contains("ProgressBarWindow")) continue;
                if (window.IsVisible)
                {
                    return window;
                }
            }
            return null;
        }
    }
}