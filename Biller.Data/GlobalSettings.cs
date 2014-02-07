using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Data
{
    public static class GlobalSettings
    {
        public static bool TaxSupplementaryWorkSeperate { get; set; }

        public static bool UseGermanSupplementaryTaxRegulation { get; set; }

        public static string LocalizedOnSupplementaryWork { get; set; }

        public static Utils.TaxClass ShipmentTaxClass { get; set; }
    }
}
