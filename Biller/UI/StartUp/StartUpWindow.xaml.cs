using Biller.UI.ViewModel;
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
using System.Windows.Shapes;

namespace Biller.UI.StartUp
{
    /// <summary>
    /// Interaktionslogik für StartUpWindow.xaml
    /// </summary>
    public partial class StartUpWindow : Window
    {
        private MainWindowViewModel vm;
        private object previousControl;
        private bool canClose;
        public StartUpWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            this.vm = vm;
            DataContext = vm;
            contentPresenter.Content = new DatabaseIntroduction() { DataContext = vm };
            canClose = false;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = !canClose;
        }

        /// <summary>
        /// Next button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Office2013Button_Click(object sender, RoutedEventArgs e)
        {
            if (previousControl == null)
            {
                previousControl = contentPresenter.Content;
                try
                { contentPresenter.Content = ((previousControl as DatabaseIntroduction).CBDatabases.SelectedValue as Core.Models.DatabaseUIModel).Settings; }
                catch
                { }
            }
            else
            {
                canClose = true;
                this.Close();
            }
        }

        /// <summary>
        /// Previous button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Office2013Button_Click_1(object sender, RoutedEventArgs e)
        {
            contentPresenter.Content = previousControl;
        }
    }
}
