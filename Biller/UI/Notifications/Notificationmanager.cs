using Biller.Controls.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Biller.UI.Notifications
{
    public class Notificationmanager
    {
        private const double topOffset = 100;
        private const double leftOffset = 385;
        readonly Biller.Controls.Notification.NotificationWindow notificationWindow= new Biller.Controls.Notification.NotificationWindow();

        public Notificationmanager()
        {
            notificationWindow.Top = SystemParameters.WorkArea.Top + topOffset;
            notificationWindow.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - leftOffset;
        }

        public void ShowNotification(string title, string description)
        {
            notificationWindow.AddNotification(new Notification { Title = title, Message = description });
        }

        public void ShowNotification(Notification notification)
        {
            notificationWindow.AddNotification(notification);
        }

        public void Close()
        {
            notificationWindow.Close();
        }
    }
}
