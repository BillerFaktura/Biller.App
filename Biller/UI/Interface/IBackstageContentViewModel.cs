using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Biller.UI.Interface
{
    public interface IBackstageContentViewModel : IViewModel
    {
        Fluent.BackstageTabItem BackstageTabItem { get; }
    }
}
