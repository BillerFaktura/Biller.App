using System.Windows;
using System.Windows.Controls;

namespace Biller.UI.Interface
{
    internal interface IArticleEditContentItem
    {
        UIElement Content { get; }

        TabItem TabItem { get; }
    }
}