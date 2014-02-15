using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Linq;
using NLog;

namespace Biller.UI.OrderView
{
    public class OrderTabViewModel : Biller.Data.Utils.PropertyChangedHelper, Biller.UI.Interface.ITabContentViewModel
    {
        private bool firstStart = true;
        private Collection<Data.Interfaces.DocumentFactory> documentFactories;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates a new instance of the OrderTabViewModel. This is the controller and datastorage for <see cref="OrderRibbonTabItem"/> and <see cref="OrderTabContent"/>
        /// </summary>
        public OrderTabViewModel(ViewModel.MainWindowViewModel parentViewModel)
        {
            this.ParentViewModel = parentViewModel;
            ContextualTabGroup = new Fluent.RibbonContextualTabGroup() { Header = "Auftragsmappen", Background = Brushes.LimeGreen, BorderBrush = Brushes.LimeGreen, Visibility=System.Windows.Visibility.Visible};
            DisplayedOrders = new ObservableCollection<Data.Document.PreviewDocument>();
            
            OrderRibbonTabItem = new OrderRibbonTabItem(this);
            OrderTabContent = new OrderTabContent(this);

            parentViewModel.RibbonFactory.AddContextualGroup(ContextualTabGroup);

            documentFactories = new Collection<Data.Interfaces.DocumentFactory>();
            documentFactories.Add(new Data.Factories.InvoiceFactory());
        }

        /// <summary>
        /// Holds a reference to <see cref="OrderRibbonTabItem"/>
        /// </summary>
        public OrderRibbonTabItem OrderRibbonTabItem { get; private set; }

        /// <summary>
        /// Holds a reference to <see cref="OrderTabContent"/>
        /// </summary>
        public OrderTabContent OrderTabContent { get; private set; }

        public ObservableCollection<Data.Document.PreviewDocument> AllOrders { get { return GetValue(() => AllOrders); } set { SetValue(value); } }

        public ObservableCollection<Data.Document.PreviewDocument> DisplayedOrders { get { return GetValue(() => DisplayedOrders); } set { SetValue(value); } }

        public Data.Document.PreviewDocument SelectedDocument { get { return GetValue(() => SelectedDocument); } set { SetValue(value); } }

        public ObservableCollection<string> YearRange { get { return GetValue(() => YearRange); } set { SetValue(value); } }

        public ViewModel.MainWindowViewModel ParentViewModel { get; private set; }

        public Fluent.RibbonContextualTabGroup ContextualTabGroup { get; private set; }

        public Fluent.RibbonTabItem RibbonTabItem
        {
            get
            {
                return OrderRibbonTabItem;
            }
        }

        public System.Windows.UIElement TabContent
        {
            get
            {
                return OrderTabContent;
            }
        }

        /// <summary>
        /// Create a new <see cref="Document"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="DocoumentType"></param>
        /// <returns></returns>
        public async Task ReceiveNewDocumentCommand(object sender, string DocumentType)
        {
            var list = from factories in documentFactories where factories.DocumentType == DocumentType select factories;
            if (list.Count() > 0)
            {
                var factory = list.First();
                var document = factory.GetNewDocument();
                var orderEditControl = new UI.OrderView.Contextual.OrderEditViewModel(this, document, true);
                foreach (var tab in factory.GetEditContentTabs())
                {
                    orderEditControl.EditContentTabs.Add(tab);
                }
                orderEditControl.ExportClass = factory.GetNewExportClass();
                await orderEditControl.LoadData();
                ParentViewModel.AddTabContentViewModel(orderEditControl);
                orderEditControl.RibbonTabItem.IsSelected = true;
            }
            
            //TODO: Messagebox for missing module
        }

        /// <summary>
        /// Open new Document Folder
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public async Task ReceiveNewDocumentCommand(object sender)
        {
            var orderEditControl = new OrderView.Contextual.OrderEditViewModel(this);
            foreach (var factory in documentFactories)
            {
                Fluent.Button button = factory.GetCreationButton();
                button.DataContext = orderEditControl;
                orderEditControl.OrderEditRibbonTabItem.AddDocumentButton(button);

            }
            await orderEditControl.LoadData();

            ParentViewModel.AddTabContentViewModel(orderEditControl);
            orderEditControl.RibbonTabItem.IsSelected = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public async Task ReceiveEditOrderCommand(object sender)
        {
            if (SelectedDocument != null)
            {
                var list = from factories in documentFactories where factories.DocumentType == SelectedDocument.DocumentType select factories;
                Data.Document.Document loadingDocument;
                if (list.Count() > 0)
                {
                    try
                    {
                        var factory = list.First();

                        loadingDocument = factory.GetNewDocument();
                        loadingDocument.DocumentID = SelectedDocument.DocumentID;
                        loadingDocument = await ParentViewModel.Database.GetDocument(loadingDocument);

                        var orderEditControl = new UI.OrderView.Contextual.OrderEditViewModel(this, loadingDocument, false);
                        foreach (var tab in factory.GetEditContentTabs())
                        {
                            orderEditControl.EditContentTabs.Add(tab);
                        }
                        orderEditControl.ExportClass = factory.GetNewExportClass();
                        await orderEditControl.LoadData();
                        ParentViewModel.AddTabContentViewModel(orderEditControl);
                        orderEditControl.RibbonTabItem.IsSelected = true;
                    }
                    catch (Exception e)
                    {
                        logger.ErrorException("Error loading document. Sender was " + sender.ToString(), e);
                    }
                }
                //TODO: Messagebox for missing module
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EditOrderControlViewModel"></param>
        /// <returns></returns>
        public async Task ReceiveCloseEditControl(OrderView.Contextual.OrderEditViewModel EditOrderControlViewModel)
        {
            //We need to change the visibility during a bug in the RibbonControl which shows the contextual TabHeader after removing a visible item
            EditOrderControlViewModel.RibbonTabItem.Visibility = System.Windows.Visibility.Collapsed;

            ParentViewModel.RibbonFactory.RemoveTabItem(EditOrderControlViewModel.RibbonTabItem);
            OrderRibbonTabItem.IsSelected = true;
        }

        public async Task LoadData()
        {
            AllOrders = new ObservableCollection<Data.Document.PreviewDocument>();
            if (firstStart)
            {
                DisplayedOrders.Clear();
                var monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
                var monthEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, monthStart.AddMonths(1).AddDays(-1).Day, 23, 59, 59);

                var result = await ParentViewModel.Database.DocumentsInInterval(monthStart, monthEnd);
                foreach (Data.Document.PreviewDocument item in result)
                    DisplayedOrders.Add(item);

                firstStart = false;
            }
            else
            {
                DisplayedOrders = new ObservableCollection<Data.Document.PreviewDocument>();
            }
            await ParentViewModel.Database.AddAdditionalPreviewDocumentParser(new Data.Orders.DocumentParsers.InvoiceParser());
        }

        public void ReceiveData(object data)
        {
            throw new System.NotImplementedException();
        }

        public async Task SaveOrUpdateDocument(Data.Document.Document source)
        {
            dynamic tempPreview = new Data.Document.PreviewDocument(source.DocumentType);
            if (source is Data.Orders.Order)
                tempPreview = Data.Orders.Order.PreviewFromOrder(source as Data.Orders.Order);

            if (AllOrders.Contains(tempPreview))
            {
                var index = AllOrders.IndexOf(tempPreview);
                AllOrders.RemoveAt(index); AllOrders.Insert(index, tempPreview);
            }
            else
            {
                AllOrders.Add(tempPreview);
            }

            bool result = await ParentViewModel.Database.SaveOrUpdateDocument(source);
        }

        public void AddDocumentFactory(Data.Interfaces.DocumentFactory DocumentFactory)
        {
            documentFactories.Add(DocumentFactory);
        }

        /// <summary>
        /// Returns the first factory associated with the DocumentType. Returns null if no factory matches the DocumentType.
        /// </summary>
        /// <param name="DocumentType"></param>
        /// <returns></returns>
        public Data.Interfaces.DocumentFactory GetFactory(string DocumentType)
        {
            var list = from factories in documentFactories where factories.DocumentType == DocumentType select factories;
            if (list.Count() > 0)
                return list.First();
            return null;
        }
    }
}