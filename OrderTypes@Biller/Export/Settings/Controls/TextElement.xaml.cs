using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OrderTypes_Biller.Export.Settings.Controls
{
    /// <summary>
    /// Interaktionslogik für TextElement.xaml
    /// </summary>
    public partial class TextElement : ContentControl, INotifyPropertyChanged, IExportControl
    {
        public TextElement()
        {
            InitializeComponent();
            Children = new System.Collections.ObjectModel.ObservableCollection<IExportControl>();
        }

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                if (text == value) return;
                text = value;
                OnPropertyChanged("Text");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public System.Collections.ObjectModel.ObservableCollection<IExportControl> Children { get; set; }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                if (description == value) return;
                description = value;
                OnPropertyChanged("Description");
            }
        }
    }
}
