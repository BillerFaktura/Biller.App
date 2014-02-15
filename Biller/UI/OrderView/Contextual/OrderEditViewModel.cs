using Biller.UI.OrderView.Contextual.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Biller.UI.OrderView.Contextual 
{
    public class OrderEditViewModel : Data.Utils.PropertyChangedHelper, Biller.UI.Interface.ITabContentViewModel
    {
        public OrderEditViewModel(OrderView.OrderTabViewModel parentViewModel)
        {
            ContextualTabGroup = parentViewModel.ContextualTabGroup;
            this.ParentViewModel = parentViewModel;
            LinkedOrders = new ObservableCollection<Data.Document.PreviewDocument>();
            OrderEditRibbonTabItem = new OrderEditRibbonTabItem(this);
            OrderEditTabHolder = new OrderEditTabHolder() { DataContext = this };
            OrderFolderControl = new Controls.OrderFolder() { DataContext = this };
            DisplayedTabContent = OrderFolderControl;
            EditMode = true;

            // Sample Data //
            LinkedOrders.Add(new Data.Document.PreviewDocument("Rechnung") { DocumentID = "1000" });
            LinkedOrders.Add(new Data.Document.PreviewDocument("Lieferschein") { DocumentID = "1023" });
            LinkedOrders.Add(new Data.Document.PreviewDocument("Gutschrift") { DocumentID = "1430" });

            EditContentTabs = new ObservableCollection<UIElement>();
        }

        public OrderEditViewModel(OrderView.OrderTabViewModel parentViewModel, Data.Document.Document document, bool editEnabled)
        {
            ContextualTabGroup = parentViewModel.ContextualTabGroup;
            this.ParentViewModel = parentViewModel;
            LinkedOrders = new ObservableCollection<Data.Document.PreviewDocument>();
            OrderEditRibbonTabItem = new OrderEditRibbonTabItem(this);
            OrderEditTabHolder = new OrderEditTabHolder() { DataContext = this };
            OrderFolderControl = new Controls.OrderFolder() { DataContext = this };
            DisplayedTabContent = OrderEditTabHolder;
            this.Document = document;
            EditMode = editEnabled;
            EditContentTabs = new ObservableCollection<UIElement>();
            OrderEditRibbonTabItem.ShowDocumentControls();
        }

        public OrderView.OrderTabViewModel ParentViewModel { get; private set; }

        public Fluent.RibbonContextualTabGroup ContextualTabGroup { get; private set; }
        public OrderEditRibbonTabItem OrderEditRibbonTabItem { get; private set; }
        public OrderEditTabHolder OrderEditTabHolder { get; private set; }
        public Controls.OrderFolder OrderFolderControl { get; private set; }

        public bool EditMode { get { return GetValue(() => EditMode); } private set { SetValue(value); } }

        public ObservableCollection<UIElement> EditContentTabs { get; private set; }

        public Fluent.RibbonTabItem RibbonTabItem
        {
            get { return OrderEditRibbonTabItem; }
        }

        public Data.Document.Document Document
        {
            get { return GetValue(() => Document); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Instance to the page which is showed with the according <see cref="RibbonTabItem"/>.
        /// </summary>
        public System.Windows.UIElement TabContent
        {
            get { return new OrderEditTabContent(this) { DataContext = this }; ; }
        }

        /// <summary>
        /// Link to the current content of <see cref="OrderEditTabContent"/>'s ItemsControl.
        /// </summary>
        public System.Windows.UIElement DisplayedTabContent
        {
            get { return GetValue(() => DisplayedTabContent); }
            set { SetValue(value); }
        }

        public async Task ReceiveInternalDocumentCreation(object sender, string DocumentType)
        {
            var factory = ParentViewModel.GetFactory(DocumentType);
            Document = factory.GetNewDocument();
            foreach (var tab in factory.GetEditContentTabs())
            {
                EditContentTabs.Add(tab);
            }
            ExportClass = factory.GetNewExportClass();
            await LoadData();
            await ParentViewModel.ParentViewModel.Database.UpdateTemporaryUsedDocumentID("", Document.DocumentID, Document.DocumentType);
            DisplayedTabContent = OrderEditTabHolder;
            EditMode = true;
        }

        public async Task ReceiveCloseCommand()
        {
            OrderEditRibbonTabItem.Focus();
            if (Document != null)
                await ParentViewModel.ParentViewModel.Database.UpdateTemporaryUsedDocumentID(Document.DocumentID, "", Document.DocumentType);
            await ParentViewModel.ReceiveCloseEditControl(this);
        }

        public ObservableCollection<Data.Document.PreviewDocument> LinkedOrders { get; set; }

        public async Task LoadData()
        {
            if (Document != null && EditMode == true)
                this.Document.DocumentID = (await ParentViewModel.ParentViewModel.Database.GetNextDocumentID(this.Document.DocumentType)).ToString();
        }

        public void ReceiveData(object data)
        {
            if (data is Data.Customers.Customer)
            {
                (Document as Data.Orders.Order).Customer = (data as Data.Customers.Customer);
            }

            if (data is Data.Articles.Article)
            {
                if (Document is Data.Orders.Order)
                {
                    if (!String.IsNullOrEmpty((Document as Data.Orders.Order).Customer.MainAddress.OneLineString))
                    {
                        //Check pricegroup
                        var customer = (Document as Data.Orders.Order).Customer;
                        var orderedArticle = new Data.Articles.OrderedArticle(data as Data.Articles.Article);
                        orderedArticle.OrderedAmount = 1;
                        
                        switch (customer.Pricegroup)
                        {
                            case 0:
                                orderedArticle.OrderPrice = orderedArticle.Price1;
                                break;
                            case 1:
                                orderedArticle.OrderPrice = orderedArticle.Price2;
                                break;
                            case 2:
                                orderedArticle.OrderPrice = orderedArticle.Price3;
                                break;
                        }
                        (Document as Data.Orders.Order).OrderedArticles.Add(orderedArticle);
                        
                    }
                    else
                    {
                        var orderedArticle = new Data.Articles.OrderedArticle(data as Data.Articles.Article);
                        orderedArticle.OrderedAmount = 1;
                        orderedArticle.OrderPrice = orderedArticle.Price1;
                        (Document as Data.Orders.Order).OrderedArticles.Add(orderedArticle);
                    }
                }
            }
        }

        public Data.Customers.Customer PreviewCustomer { get { return GetValue(() => PreviewCustomer); } set { SetValue(value); } }

        public Data.Articles.Article PreviewArticle { get { return GetValue(() => PreviewArticle); } set { SetValue(value); } }

        public Data.Interfaces.IExport ExportClass { get { return GetValue(() => ExportClass); } set { SetValue(value); } }
    }
}
