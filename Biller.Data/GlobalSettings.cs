//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Biller.Data
//{
//    public class GlobalSettings
//    {
//        private static GlobalSettings currentInstance;
//        public static GlobalSettings GetInstance()
//        {
//            if (currentInstance == null)
//                currentInstance = new GlobalSettings();
//            return currentInstance;
//        }

//        public bool TaxSupplementaryWorkSeperate { get; set; }

//        public bool UseGermanSupplementaryTaxRegulation { get; set; }

//        public string LocalizedOnSupplementaryWork { get; set; }

//        public Utils.TaxClass ShipmentTaxClass { get; set; }

//        public ObservableCollection<Data.Utils.Unit> ArticleUnits { get; set; }

//        public ObservableCollection<Data.Utils.PaymentMethode> PaymentMethodes { get; set; }

//        public ObservableCollection<Data.Utils.TaxClass> TaxClasses { get; set; }
//    }
//}
