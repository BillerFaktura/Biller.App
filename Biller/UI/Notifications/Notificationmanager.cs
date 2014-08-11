using Biller.Controls.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Biller.UI.Notifications
{
    public class NotificationManager
    {
        private const double topOffset = 100;
        private const double leftOffset = 385;
        readonly Biller.Controls.Notification.NotificationWindow notificationWindow= new Biller.Controls.Notification.NotificationWindow();

        public NotificationManager()
        {
            notificationWindow.Top = SystemParameters.WorkArea.Top + topOffset;
            notificationWindow.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - leftOffset;
        }

        /// <summary>
        /// Thread Safe
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        public void ShowNotification(string title, string description)
        {
            notificationWindow.Dispatcher.Invoke(new Action(() => notificationWindow.AddNotification(new Notification { Title = title, Message = description })));
        }

        /// <summary>
        /// Thread Safe
        /// </summary>
        /// <param name="notification"></param>
        public void ShowNotification(Notification notification)
        {
            notificationWindow.Dispatcher.Invoke(new Action(() => notificationWindow.AddNotification(notification)));
        }

        public void Close()
        {
            notificationWindow.Close();
        }
    }
}
