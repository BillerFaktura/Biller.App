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

namespace Biller.UI.DocumentView.Contextual.EditTabs.Settings
{
    /// <summary>
    /// Interaktionslogik für Content.xaml
    /// </summary>
    public partial class Content : UserControl
    {
        private string previousID = "";

        public Content()
        {
            InitializeComponent();
        }

        private void Office2013Button_Click(object sender, RoutedEventArgs e)
        {
            if (CalendarPopup.IsOpen)
                CalendarPopup.IsOpen = false;
            else
                CalendarPopup.IsOpen = true;
        }

        private async void WatermarkTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(previousID))
                previousID = (sender as TextBox).Text;

            try
            {
                if ((sender as TextBox).IsFocused == true)
                {
                    bool result = true;
                    var doc = (DataContext as DocumentEditViewModel).Document;
                    doc.DocumentID = (sender as TextBox).Text;

                    try { result = await (DataContext as DocumentEditViewModel).ParentViewModel.ParentViewModel.Database.DocumentExists(doc); }
                    catch { }
                    if (result && (DataContext as DocumentEditViewModel).EditMode == true)
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
                    result = await (DataContext as DocumentEditViewModel).ParentViewModel.ParentViewModel.Database.UpdateTemporaryUsedDocumentID(previousID, (sender as TextBox).Text, doc.DocumentType);
                    previousID = (sender as TextBox).Text;
                }
            }
            catch { }
        }

        private void ButtonShowCalendar2_Click(object sender, RoutedEventArgs e)
        {
            if (CalendarPopup2.IsOpen)
                CalendarPopup2.IsOpen = false;
            else
                CalendarPopup2.IsOpen = true;
        }
    }
}
