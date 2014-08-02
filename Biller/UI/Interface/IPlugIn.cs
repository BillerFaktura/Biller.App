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

        /// <summary>
        /// During the activation process you add your own handlers, classes, databases, controls etc. to the main app.\n
        /// You cannot load any data from a database (or add own data models) as it is not initialized yet.
        /// </summary>
        void Activate();

        List<IViewModel> ViewModels();
    }
}
