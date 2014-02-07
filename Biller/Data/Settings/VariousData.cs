using System.Collections.ObjectModel;

namespace Biller.Data.Settings
{
    public static class VariousData
    {
        public static void LoadData(Data.Interfaces.IDatabase database)
        {
        }

        public static ObservableCollection<Data.Utils.Unit> ArticleUnits { get; private set; }

        public static ObservableCollection<Data.Utils.PaymentMethode> PaymentMethodes { get; private set; }

        public static ObservableCollection<Data.Utils.TaxClass> TaxClasses { get; private set; }
    }
}