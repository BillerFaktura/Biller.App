using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Threading;

namespace Biller.Controls.Notification
{
    /// <summary>
    /// Based on WPFGrowlNotification
    /// </summary>
    public class Notification : INotifyPropertyChanged
    {
        private string message;
        public string Message
        {
            get { return message; }

            set
            {
                if (message == value) return;
                message = value;
                OnPropertyChanged("Message");
            }
        }

        private int id;
        public int Id
        {
            get { return id; }

            set
            {
                if (id == value) return;
                id = value;
                OnPropertyChanged("Id");
            }
        }

        private string imageUrl;
        public string ImageUrl
        {
            get { return imageUrl; }

            set
            {
                if (imageUrl == value) return;
                imageUrl = value;
                OnPropertyChanged("ImageUrl");
            }
        }

        private string title;
        public string Title
        {
            get { return title; }

            set
            {
                if (title == value) return;
                title = value;
                OnPropertyChanged("Title");
            }
        }

        private int duration = 10000;
        public int Duration
        {
            get { return duration; }

            set
            {
                if (duration == value) return;
                duration = value;
                OnPropertyChanged("Duration");
            }
        }

        private bool durationExpired = false;
        public bool DurationExpired
        {
            get { return durationExpired; }

            set
            {
                if (durationExpired == value) return;
                durationExpired = value;
                OnPropertyChanged("DurationExpired");
            }
        }

        public void Shown()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(duration);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            DurationExpired = true;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Notifications : ObservableCollection<Notification> { }
}
