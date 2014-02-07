namespace Biller.UI.Interface
{
    internal interface IRibbonTabItem
    {
        Biller.UI.Interface.ITabContentViewModel ParentViewModel { get; }
    }
}