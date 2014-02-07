using System;
using System.Collections.Generic;
using System.Linq;
namespace Biller.UI.Ribbon
{
    public class RibbonFactory
    {
        public Fluent.Ribbon Ribbon { get; private set; }

        public Biller.UI.ViewModel.MainWindowViewModel MainWindowViewModel
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public RibbonFactory(Fluent.Ribbon ribbon)
        {
            Ribbon = ribbon;
        }

        /// <summary>
        /// Adds a <see cref="RibbonTabItem"/> to the collection. Considers the ContextualTabGroup
        /// </summary>
        /// <param name="RibbonTabItem"></param>
        public void AddTabItem(Fluent.RibbonTabItem RibbonTabItem)
        {
            if (RibbonTabItem.Group == null)
            {
                Ribbon.Tabs.Insert(GetIndexOfLastNormalTab(), RibbonTabItem);
            }
            else
            {
                var indexgroup = Ribbon.ContextualGroups.IndexOf(RibbonTabItem.Group);
                var insertindex = -1;
                while (insertindex == -1 && indexgroup >= 0)
                {
                    var list = (from item in Ribbon.Tabs orderby Ribbon.Tabs.IndexOf(item) where item.Group == Ribbon.ContextualGroups[indexgroup] select Ribbon.Tabs.IndexOf(item)).ToList();
                    if (list.Count == 0)
                    {
                        indexgroup -= 1;
                    }
                    else
                    {
                        insertindex = list.Last() + 1;
                    }
                }
                if (indexgroup == -1)
                    insertindex = GetIndexOfLastNormalTab();

                Ribbon.Tabs.Insert(insertindex, RibbonTabItem);
            }
        }

        public void RemoveTabItem(Fluent.RibbonTabItem RibbonTabItem)
        {
            Ribbon.Tabs.Remove(RibbonTabItem);
        }

        /// <summary>
        /// Adds a specified <see cref="Fluent.RibbonContextualTabGroup"/>
        /// </summary>
        /// <param name="ContextualTabGroup"></param>
        public void AddContextualGroup(Fluent.RibbonContextualTabGroup ContextualTabGroup)
        {
            Ribbon.ContextualGroups.Add(ContextualTabGroup);
        }

        private int GetIndexOfLastNormalTab()
        {
            var list = from item in Ribbon.Tabs orderby Ribbon.Tabs.IndexOf(item) where item.Group == null select Ribbon.Tabs.IndexOf(item);
            try
            {
                var lastitem = list.Last();
                return lastitem + 1;
            }
            catch (Exception e)
            {
                //empty list
                return 0;
            }
        }
    }
}