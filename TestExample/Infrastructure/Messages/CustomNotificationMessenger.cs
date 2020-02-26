using System;
using GalaSoft.MvvmLight.Messaging;

namespace TestExample.Infrastructure.Messages
{
    public static class CustomNotificationMessenger
    {
        public static void Register<T>(object recipient, object token, Action<NotificationMessage<T>> action)
        {
            Messenger.Default.Register(recipient, token, action);
        }

        public static void Send(object token)
        {
            var notificationMessage = new NotificationMessage<string>(string.Empty, string.Empty);
            Messenger.Default.Send(notificationMessage, token);
        }

        public static void Send<T>(T content, string notification, object token)
        {
            var notificationMessage = new NotificationMessage<T>(content, notification);
            Messenger.Default.Send(notificationMessage, token);
        }

        public static void Unregister<T>(object recipient)
        {
            Messenger.Default.Unregister<NotificationMessage<T>>(recipient);
        }
    }
}
