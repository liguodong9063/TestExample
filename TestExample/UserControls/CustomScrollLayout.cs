using System;
using System.Windows.Controls;
using TestExample.Infrastructure.Messages;
using TestExample.Infrastructure.Messages.Token;

namespace TestExample.UserControls
{
    public class CustomScrollLayout : ScrollViewer
    {
        public CustomScrollLayout()
        {
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            ScrollChanged += CustomScrollViewer_ScrollChanged;
        }

        private void CustomScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.Source is CustomScrollLayout && Math.Abs(e.VerticalChange) > 0)
            {
                CustomNotificationMessenger.Send(NotificationMessageToken.CloseComboBoxItemArea);
            }
        }
    }
}
