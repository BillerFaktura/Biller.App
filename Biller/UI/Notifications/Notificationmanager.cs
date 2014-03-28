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

        public void ShowNotification()
        {
            notificationWindow.AddNotification(new Notification { Title = "Mesage #1", Message = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." });
        }
    }
}
