﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Linq;
using NLog;

namespace Biller.UI.DocumentView
{
    public class DocumentTabViewModel : Biller.Data.Utils.PropertyChangedHelper, Biller.UI.Interface.ITabContentViewModel
    {
        private Collection<Data.Interfaces.DocumentFactory> documentFactories;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates a new instance of the OrderTabViewModel. This is the controller and datastorage for <see cref="DocumentRibbonTabItem"/> and <see cref="DocumentTabContent"/>
        /// </summary>
        public DocumentTabViewModel(ViewModel.MainWindowViewModel parentViewModel)
        {
            this.ParentViewModel = parentViewModel;
            ContextualTabGroup = new Fluent.RibbonContextualTabGroup() { Header = "Auftragsmappen", Background = Brushes.LimeGreen, BorderBrush = Brushes.LimeGreen, Visibility=System.Windows.Visibility.Visible};
            DisplayedDocuments = new ObservableCollection<Data.Document.PreviewDocument>();
            
            DocumentRibbonTabItem = new DocumentRibbonTabItem(this);
            DocumentTabContent = new DocumentTabContent(this);

            parentViewModel.RibbonFactory.AddContextualGroup(ContextualTabGroup);

            documentFactories = new Collection<Data.Interfaces.DocumentFactory>();

            registeredObservers = new ObservableCollection<Interface.IEditObserver>();
        }

        /// <summary>
        /// Holds a reference to <see cref="DocumentRibbonTabItem"/>
        /// </summary>
        public DocumentRibbonTabItem DocumentRibbonTabItem { get; private set; }

        /// <summary>
        /// Holds a reference to <see cref="DocumentTabContent"/>
        /// </summary>
        public DocumentTabContent DocumentTabContent { get; private set; }

        public DateTime IntervalStart { get { return GetValue(() => IntervalStart); } set { SetValue(value); ShowDocumentsInInterval(IntervalStart, IntervalEnd); } }

        public DateTime IntervalEnd { get { return GetValue(() => IntervalEnd); } set { SetValue(value); ShowDocumentsInInterval(IntervalStart, IntervalEnd); } }

        public ObservableCollection<Data.Document.PreviewDocument> AllDocuments { get { return GetValue(() => AllDocuments); } set { SetValue(value); } }

        public ObservableCollection<Data.Document.PreviewDocument> DisplayedDocuments { get { return GetValue(() => DisplayedDocuments); } set { SetValue(value); } }

        public Data.Document.PreviewDocument SelectedDocument
        {
            get { return GetValue(() => SelectedDocument); }
            set
            {
                SetValue(value);
                if (value is Data.Document.PreviewDocument)
                {
                    SetEditButtonEnabled(true);
                }
                else
                {
                    SetEditButtonEnabled(false);
                }
            }
        }

        public ObservableCollection<string> YearRange { get { return GetValue(() => YearRange); } set { SetValue(value); } }

        public ViewModel.MainWindowViewModel ParentViewModel { get; private set; }

        public Fluent.RibbonContextualTabGroup ContextualTabGroup { get; private set; }

        public Fluent.RibbonTabItem RibbonTabItem
        {
            get
            {
                return DocumentRibbonTabItem;
            }
        }

        public System.Windows.UIElement TabContent
        {
            get
            {
                return DocumentTabContent;
            }
        }

        void SetEditButtonEnabled(bool isEnabled)
        {
            DocumentRibbonTabItem.buttonEditOrder.IsEnabled = isEnabled;
            DocumentRibbonTabItem.buttonPrintOrder.IsEnabled = isEnabled;
            DocumentRibbonTabItem.buttonOrderPDF.IsEnabled = isEnabled;
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
                var documentEditViewModel = new UI.DocumentView.Contextual.DocumentEditViewModel(this, document, true);
                foreach (var tab in factory.GetEditContentTabs())
                    documentEditViewModel.EditContentTabs.Add(tab);

                documentEditViewModel.ExportClass = ParentViewModel.SettingsTabViewModel.PreferedExportClasses.Where(x => x.Document.DocumentType == (DocumentType)).First().GetExport();
                await documentEditViewModel.LoadData();
                ParentViewModel.AddTabContentViewModel(documentEditViewModel);
                documentEditViewModel.RibbonTabItem.IsSelected = true;

                foreach (var observer in registeredObservers)
                    observer.ReceiveDocumentEditViewModel(documentEditViewModel);
            }
            else
            {
                var ErrorNotification = new Controls.Notification.Notification();
                ErrorNotification.ImageUrl = "..\\..\\Images\\appbar.app.remove.png";
                ErrorNotification.Title = "Fehler beim Laden";
                ErrorNotification.Message = "Für " + DocumentType + " existiert kein registrierter Dienst.";
                ParentViewModel.Notificationmanager.ShowNotification(ErrorNotification);
            }
        }

        /// <summary>
        /// Open new Document Folder
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public async Task ReceiveNewDocumentCommand(object sender)
        {
            var documentEditControl = new DocumentView.Contextual.DocumentEditViewModel(this);
            foreach (var factory in documentFactories)
            {
                Fluent.Button button = factory.GetCreationButton();
                button.DataContext = documentEditControl;
                documentEditControl.DocumentEditRibbonTabItem.AddDocumentButton(button);
            }
            await documentEditControl.LoadData();

            ParentViewModel.AddTabContentViewModel(documentEditControl);
            documentEditControl.RibbonTabItem.IsSelected = true;
        }

        /// <summary>
        /// Opens the selected <see cref="Document"/> to allowd editing.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public async Task ReceiveEditDocumentCommand(object sender)
        {
            if (SelectedDocument != null)
            {
                if (ViewModelRequestingDocument != null)
                {
                    ViewModelRequestingDocument.ReceiveData(SelectedDocument);
                    ParentViewModel.SelectedContent = ViewModelRequestingDocument.TabContent;
                    ViewModelRequestingDocument = null;
                }
                else
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

                            var documentEditViewModel = new UI.DocumentView.Contextual.DocumentEditViewModel(this, loadingDocument, false);
                            foreach (var tab in factory.GetEditContentTabs())
                            {
                                documentEditViewModel.EditContentTabs.Add(tab);
                            }

                            try
                            {
                                documentEditViewModel.ExportClass = ParentViewModel.SettingsTabViewModel.PreferedExportClasses.Where(x => x.Document.DocumentType == (loadingDocument.DocumentType)).First().GetExport();
                            }
                            catch(Exception e)
                            {
                                var ErrorNotification = new Controls.Notification.Notification();
                                ErrorNotification.ImageUrl = "..\\..\\Images\\appbar.app.remove.png";
                                ErrorNotification.Title = "Fehler beim Laden";
                                ErrorNotification.Message = "Für " + SelectedDocument.DocumentType + " existiert keine registrierte Exportfunktion.";
                                ParentViewModel.Notificationmanager.ShowNotification(ErrorNotification);
                            }
                                                        
                            await documentEditViewModel.LoadData();
                            ParentViewModel.AddTabContentViewModel(documentEditViewModel);
                            documentEditViewModel.RibbonTabItem.IsSelected = true;

                            foreach (var observer in registeredObservers)
                                observer.ReceiveDocumentEditViewModel(documentEditViewModel);
                        }
                        catch (Exception e)
                        {
                            logger.ErrorException("Error loading document. Sender was " + sender.ToString(), e);
                        }
                    }
                    else
                    {
                        var ErrorNotification = new Controls.Notification.Notification();
                        ErrorNotification.ImageUrl = "..\\..\\Images\\appbar.app.remove.png";
                        ErrorNotification.Title = "Fehler beim Laden";
                        ErrorNotification.Message = "Für " + SelectedDocument.DocumentType + " existiert kein registrierter Dienst.";
                        ParentViewModel.Notificationmanager.ShowNotification(ErrorNotification);
                    }
                }
            }
        }

        public async Task ReceiveEditOrderCommand(object sender, Data.Document.PreviewDocument document)
        {
            if (document != null)
            {
                if (ViewModelRequestingDocument != null)
                {
                    ViewModelRequestingDocument.ReceiveData(SelectedDocument);
                    ParentViewModel.SelectedContent = ViewModelRequestingDocument.TabContent;
                    ViewModelRequestingDocument = null;
                }
                else
                {
                    var list = from factories in documentFactories where factories.DocumentType == document.DocumentType select factories;
                    Data.Document.Document loadingDocument;
                    if (list.Count() > 0)
                    {
                        try
                        {
                            var factory = list.First();

                            loadingDocument = factory.GetNewDocument();
                            loadingDocument.DocumentID = document.DocumentID;
                            loadingDocument = await ParentViewModel.Database.GetDocument(loadingDocument);

                            var documentEditViewModel = new UI.DocumentView.Contextual.DocumentEditViewModel(this, loadingDocument, false);
                            foreach (var tab in factory.GetEditContentTabs())
                            {
                                documentEditViewModel.EditContentTabs.Add(tab);
                            }
                            documentEditViewModel.ExportClass = ParentViewModel.SettingsTabViewModel.PreferedExportClasses.Where(x => x.Document.DocumentType == (loadingDocument.DocumentType)).First().GetExport();
                            await documentEditViewModel.LoadData();
                            ParentViewModel.AddTabContentViewModel(documentEditViewModel);
                            documentEditViewModel.RibbonTabItem.IsSelected = true;

                            foreach (var observer in registeredObservers)
                                observer.ReceiveDocumentEditViewModel(documentEditViewModel);
                        }
                        catch (Exception e)
                        {
                            logger.ErrorException("Error loading document. Sender was " + sender.ToString(), e);
                        }
                    }
                    else
                    {
                        var ErrorNotification = new Controls.Notification.Notification();
                        ErrorNotification.ImageUrl = "..\\..\\Images\\appbar.app.remove.png";
                        ErrorNotification.Title = "Fehler beim Laden";
                        ErrorNotification.Message = "Für " + SelectedDocument.DocumentType + " existiert kein registrierter Dienst.";
                        ParentViewModel.Notificationmanager.ShowNotification(ErrorNotification);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EditOrderControlViewModel"></param>
        /// <returns></returns>
        public async Task ReceiveCloseEditControl(DocumentView.Contextual.DocumentEditViewModel EditOrderControlViewModel)
        {
            //We need to change the visibility during a bug in the RibbonControl which shows the contextual TabHeader after removing a visible item
            EditOrderControlViewModel.RibbonTabItem.Visibility = System.Windows.Visibility.Collapsed;

            ParentViewModel.RibbonFactory.RemoveTabItem(EditOrderControlViewModel.RibbonTabItem);
            DocumentRibbonTabItem.IsSelected = true;
        }

        public async Task LoadData()
        {
            //await ParentViewModel.Database.AddAdditionalPreviewDocumentParser(new Data.Orders.DocumentParsers.InvoiceParser());
            AllDocuments = new ObservableCollection<Data.Document.PreviewDocument>();
            IntervalStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            IntervalEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, IntervalStart.AddMonths(1).AddDays(-1).Day, 23, 59, 59);
            foreach (var item in await ParentViewModel.Database.DocumentsInInterval(new DateTime(DateTime.Now.Year, 1, 1), new DateTime(DateTime.Now.Year, 12, 31)))
                AllDocuments.Add(item);
        }

        public async Task ShowDocumentsInInterval(DateTime start, DateTime end)
        {
            var result = await ParentViewModel.Database.DocumentsInInterval(IntervalStart, IntervalEnd);
            DisplayedDocuments.Clear();
            foreach (Data.Document.PreviewDocument item in result)
                DisplayedDocuments.Add(item);
        }

        /// <summary>
        /// Does nothing here
        /// </summary>
        /// <param name="data"></param>
        public void ReceiveData(object data)
        {
            // Do nothing
        }

        public async Task SaveOrUpdateDocument(Data.Document.Document source)
        {
            dynamic tempPreview = new Data.Document.PreviewDocument(source.DocumentType);
            //if (source is Data.Orders.Order)
            //    tempPreview = Data.Orders.Order.PreviewFromOrder(source as Data.Orders.Order);

            if (AllDocuments.Contains(tempPreview))
            {
                var index = AllDocuments.IndexOf(tempPreview);
                AllDocuments.RemoveAt(index); AllDocuments.Insert(index, tempPreview);
            }
            else
            {
                AllDocuments.Add(tempPreview);
            }

            bool result = await ParentViewModel.Database.SaveOrUpdateDocument(source);

            if (DisplayedDocuments.Contains(tempPreview))
            {
                var index = DisplayedDocuments.IndexOf(tempPreview);
                DisplayedDocuments.RemoveAt(index); DisplayedDocuments.Insert(index, tempPreview);
            }
            else
            {
                DisplayedDocuments.Add(tempPreview);
            }
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

        Biller.UI.Interface.ITabContentViewModel ViewModelRequestingDocument;
        public void ReceiveRequestDocumentCommand(Biller.UI.Interface.ITabContentViewModel source)
        {
            ViewModelRequestingDocument = source;
            ParentViewModel.SelectedContent = TabContent;
        }

        ObservableCollection<Interface.IEditObserver> registeredObservers;

        public void RegisterObserver(Interface.IEditObserver observer)
        {
            registeredObservers.Add(observer);
        }

        public void LetObserverWatch(Contextual.DocumentEditViewModel documentEditViewModel)
        {
            foreach (var observer in registeredObservers)
                observer.ReceiveDocumentEditViewModel(documentEditViewModel);
        }
    }
}