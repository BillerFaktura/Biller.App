namespace Biller.UI.Interface
{
    public interface IRibbonTabItem
    {
        Biller.UI.Interface.ITabContentViewModel ParentViewModel { get; }
    }
}