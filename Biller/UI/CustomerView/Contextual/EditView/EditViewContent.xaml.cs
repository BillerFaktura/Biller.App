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

namespace Biller.UI.CustomerView.Contextual.EditView
{
    /// <summary>
    /// Interaktionslogik für EditViewContent.xaml
    /// </summary>
    public partial class EditViewContent : UserControl
    {
        private string previousID = "";

        public EditViewContent()
        {
            InitializeComponent();
        }

        private async void WatermarkTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if ((sender as TextBox).IsFocused == true)
                {
                    bool result = true;
                    try { result = await (DataContext as CustomerEditViewModel).ParentViewModel.ParentViewModel.Database.CustomerExists((sender as TextBox).Text); }
                    catch { }
                    
                    if (result && (DataContext as CustomerEditViewModel).EditMode == true)
                    {
                        (sender as TextBox).BorderBrush = Brushes.Red;
                        (sender as TextBox).BorderThickness = new System.Windows.Thickness(2);
                    }
                    else
                    {
                        var converter = new System.Windows.Media.BrushConverter();
                        var brush = (Brush)converter.ConvertFromString("#FFABADB3");
                        (sender as TextBox).BorderBrush = brush;
                        (sender as TextBox).BorderThickness = new System.Windows.Thickness(1);
                    }

                    result = await (DataContext as CustomerEditViewModel).ParentViewModel.ParentViewModel.Database.UpdateTemporaryUsedCustomerID(previousID, (sender as TextBox).Text);
                    previousID = (sender as TextBox).Text;
                }
                
            }
            catch { }
        }

        private void Office2013Button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as CustomerEditViewModel).Customer.ExtraAddresses.Add(new Core.Utils.EAddress());
        }

        private void Office2013Button_Click_1(object sender, RoutedEventArgs e)
        {
            (DataContext as CustomerEditViewModel).Customer.ExtraAddresses.Remove((sender as Biller.Controls.Office2013Button).DataContext as Core.Utils.EAddress);
        }
    }
}
