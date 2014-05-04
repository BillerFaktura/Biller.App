using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OrderTypes_Biller.Export.Settings
{
    public class ViewModel : Biller.Data.Utils.PropertyChangedHelper
    {
        static List<string> AllowedDataFields = new List<string>(new[] { "{CustomerAddressWithLineBreak}", "{CustomerAddressOneLine}", "{CustomerName}", "{SenderAddressWithLineBreak}",
        "{SenderAddressOneLine}", "{LocalizedDocumentType}", "{DocumentID}", "{DocumentType}", "{DocumentDate}", "{DateOfDelivery}", "{PaymentMethodeName}", "{PaymentMethodeText}",
        "{DocumentOpeningText}", "{DocumentClosingText}"});

        public ViewModel()
        {
            Elements = new ObservableCollection<Controls.IExportControl>();
        }

        public ObservableCollection<Controls.IExportControl> Elements { get { return GetValue(() => Elements); } private set { SetValue(value); } }

        public Controls.IExportControl SelectedElement { get { return GetValue(() => SelectedElement); } set { SetValue(value); } } 
    }
}
