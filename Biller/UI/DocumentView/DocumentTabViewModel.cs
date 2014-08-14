using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Linq;
using NLog;
using System.Collections.Generic;

namespace Biller.UI.DocumentView
{
    public class DocumentTabViewModel : Biller.Core.Utils.PropertyChangedHelper, Biller.UI.Interface.ITabContentViewModel
    {
        private Collection<Core.Interfaces.DocumentFactory> documentFactories;

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates a new instance of the OrderTabViewModel. This is the controller and datastorage for <see cref="DocumentRibbonTabItem"/> and <see cref="DocumentTabContent"/>
        /// </summary>
        public DocumentTabViewModel(ViewModel.MainWindowViewModel parentViewModel)
        {
            this.ParentViewModel = parentViewModel;
            ContextualTabGroup = new Fluent.RibbonContextualTabGroup() { Header = "Auftragsmappen", Background = Brushes.LimeGreen, BorderBrush = Brushes.LimeGreen, Visibility=System.Windows.Visibility.Visible};
            DisplayedDocuments = new ObservableCollection<Core.Document.PreviewDocument>();
            
            DocumentRibbonTabItem = new DocumentRibbonTabItem(this);
            DocumentTabContent = new DocumentTabContent(this);

            parentViewModel.RibbonFactory.AddContextualGroup(ContextualTabGroup);

            documentFactories = new Collection<Core.Interfaces.DocumentFactory>();

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

        public DateTime IntervalStart { get { return GetValue(() => IntervalStart); } set { SetValue(value); ShowDocumentsInInterval(value, IntervalEnd); } }

        public DateTime IntervalEnd { get { return GetValue(() => IntervalEnd); } set { SetValue(value); ShowDocumentsInInterval(IntervalStart, value); } }

        public ObservableCollection<Core.Document.PreviewDocument> AllDocuments { get { return GetValue(() => AllDocuments); } set { SetValue(value); } }

        public ObservableCollection<Core.Document.PreviewDocument> DisplayedDocuments { get { return GetValue(() => DisplayedDocuments); } set { SetValue(value); } }

        public IEnumerable<Core.Interfaces.IExport> AllExportClasses { get { return ParentViewModel.SettingsTabViewModel.RegisteredExportClasses; } }

        public Core.Document.PreviewDocument SelectedDocument
        {
            get { return GetValue(() => SelectedDocument); }
            set
            {
                SetValue(value);
                if (value is Core.Document.PreviewDocument)
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

                documentEditViewModel.ExportClass = ParentViewModel.SettingsTabViewModel.GetPreferedExportClass(document);
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
                ParentViewModel.NotificationManager.ShowNotification(ErrorNotification);
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
                    Core.Document.Document loadingDocument;
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
                                documentEditViewModel.ExportClass = ParentViewModel.SettingsTabViewModel.GetPreferedExportClass(loadingDocument);
                            }
                            catch(Exception e)
                            {
                                var ErrorNotification = new Controls.Notification.Notification();
                                ErrorNotification.ImageUrl = "..\\..\\Images\\appbar.app.remove.png";
                                ErrorNotification.Title = "Fehler beim Laden";
                                ErrorNotification.Message = "Für " + SelectedDocument.DocumentType + " existiert keine registrierte Exportfunktion.";
                                ParentViewModel.NotificationManager.ShowNotification(ErrorNotification);
                                logger.ErrorException("", e);
                            }
                                                        
                            await documentEditViewModel.LoadData();
                            ParentViewModel.AddTabContentViewModel(documentEditViewModel);
                            documentEditViewModel.RibbonTabItem.IsSelected = true;

                            foreach (var observer in registeredObservers)
                                observer.ReceiveDocumentEditViewModel(documentEditViewModel);
                        }
                        catch (Exception e)
                        {
                            var ErrorNotification = new Controls.Notification.Notification();
                            ErrorNotification.ImageUrl = "..\\..\\Images\\appbar.app.remove.png";
                            ErrorNotification.Title = "Fehler beim Laden";
                            dynamic document = SelectedDocument;
                            ErrorNotification.Message = "Das Dokument " + document.LocalizedDocumentType + " Nr. " + document.DocumentID + " konnte nicht geöffnet werden. Weitere Informationen im Log.";
                            ParentViewModel.NotificationManager.ShowNotification(ErrorNotification);

                            logger.ErrorException("Error loading document. Sender was " + sender.ToString(), e);
                        }
                    }
                    else
                    {
                        var ErrorNotification = new Controls.Notification.Notification();
                        ErrorNotification.ImageUrl = "..\\..\\Images\\appbar.app.remove.png";
                        ErrorNotification.Title = "Fehler beim Laden";
                        ErrorNotification.Message = "Für " + SelectedDocument.DocumentType + " existiert kein registrierter Dienst.";
                        ParentViewModel.NotificationManager.ShowNotification(ErrorNotification);
                    }
                }
            }
        }

        public async Task ReceiveEditOrderCommand(object sender, Core.Document.PreviewDocument document)
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
                    Core.Document.Document loadingDocument;
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
                            documentEditViewModel.ExportClass = ParentViewModel.SettingsTabViewModel.GetPreferedExportClass(loadingDocument);
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
                        ParentViewModel.NotificationManager.ShowNotification(ErrorNotification);
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
            AllDocuments = new ObservableCollection<Core.Document.PreviewDocument>();
            IntervalStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            IntervalEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, IntervalStart.AddMonths(1).AddDays(-1).Day, 23, 59, 59);
            foreach (var item in await ParentViewModel.Database.DocumentsInInterval(new DateTime(DateTime.Now.Year, 1, 1), new DateTime(DateTime.Now.Year, 12, 31)))
                AllDocuments.Add(item);
        }

        public async Task ShowDocumentsInInterval(DateTime start, DateTime end)
        {
            var result = await ParentViewModel.Database.DocumentsInInterval(IntervalStart, IntervalEnd);
            DisplayedDocuments.Clear();
            foreach (Core.Document.PreviewDocument item in result)
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

        public async Task SaveOrUpdateDocument(Core.Document.Document source)
        {
            dynamic tempPreview = new Core.Document.PreviewDocument(source.DocumentType);
            var factory = GetFactory(source.DocumentType);
            if (factory != null)
                tempPreview = factory.GetPreviewDocument(source);

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

        /// <summary>
        /// Add a new <see cref="DocumentFactory"/> to the central collection. Factories already registered can not be added again.\n
        /// You should register your <see cref="DocumentFactory"/> inside your <see cref="IPlugIn.LoadData()"/>
        /// </summary>
        /// <param name="DocumentFactory">The new document factory</param>
        public void AddDocumentFactory(Core.Interfaces.DocumentFactory DocumentFactory)
        {
            if (!documentFactories.Contains(DocumentFactory))
                documentFactories.Add(DocumentFactory);
        }

        /// <summary>
        /// Returns the first factory associated with the DocumentType. Returns null if no factory matches the DocumentType.
        /// </summary>
        /// <param name="DocumentType"></param>
        /// <returns></returns>
        public Core.Interfaces.DocumentFactory GetFactory(string DocumentType)
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