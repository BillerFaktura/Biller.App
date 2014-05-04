using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTypes_Biller.Export.Settings.Controls
{
    public interface IExportControl
    {
        string Description { get; set; }

        ObservableCollection<IExportControl> Children { get; set; }
    }
}
