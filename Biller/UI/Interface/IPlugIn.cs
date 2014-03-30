using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.UI.Interface
{
    public interface IPlugIn
    {
        string Name { get; }

        string Description { get; }

        double Version { get; }

        void Activate();

        List<IViewModel> ViewModels();
    }
}
