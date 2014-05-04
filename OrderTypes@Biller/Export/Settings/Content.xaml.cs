using System;
using System.Collections.Generic;
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

namespace OrderTypes_Biller.Export.Settings
{
    /// <summary>
    /// Interaktionslogik für Content.xaml
    /// </summary>
    public partial class Content : UserControl
    {
        public Content()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as Export.Settings.ViewModel);
            var control = new Controls.TextElement();
            control.MouseDown += control_MouseUp;
            viewModel.Elements.Add(control);
        }

        void control_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (DataContext as Export.Settings.ViewModel);
            viewModel.SelectedElement = sender as Settings.Controls.IExportControl;
        }
    }
}
