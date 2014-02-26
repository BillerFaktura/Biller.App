using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Biller.UI.Interface
{
    public interface ITabContentViewModel : IViewModel
    {
        Fluent.RibbonTabItem RibbonTabItem { get; }

        UIElement TabContent { get; }
    }
}
