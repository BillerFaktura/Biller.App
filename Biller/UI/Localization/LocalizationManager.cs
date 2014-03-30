using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Biller.UI.Localization
{
    public class LocalizationManager
    {
        public static string LocalizedStringFetcher(string key, Control control)
        {
            var resource = control.FindResource(key);
            if (resource is string)
                return resource as string;
            return key;
        }
    }
}
