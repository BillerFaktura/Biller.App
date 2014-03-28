using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Biller.UI.DocumentView.Contextual 
{
    public class DocumentEditViewModel : Data.Utils.PropertyChangedHelper, Biller.UI.Interface.ITabContentViewModel
    {
        /// <summary>
        /// Use this constructor if you want to show the document creation introduction.
        /// </summary>
        /// <param name="parentViewModel">The parent ViewModel</param>
        public DocumentEditViewModel(DocumentView.DocumentTabViewModel parentViewModel)
        {
            ContextualTabGroup = parentViewModel.ContextualTabGroup;
            this.ParentViewModel = parentViewModel;
            LinkedDocuments = new Data.Models.DocumentFolderModel();
            DocumentEditRibbonTabItem = new DocumentEditRibbonTabItem(this);
            DocumentEditTabHolder = new DocumentEditTabHolder() { DataContext = this };
            DocumentFolderControl = new DocumentFolder() { DataContext = this };
            DisplayedTabContent = DocumentFolderControl;
            EditMode = true;

            // Sample Data //
            //LinkedOrders.Add(new Data.Document.PreviewDocument("Rechnung") { DocumentID = "1000" });
            //LinkedOrders.Add(new Data.Document.PreviewDocument("Lieferschein") { DocumentID = "1023" });
            //LinkedOrders.Add(new Data.Document.PreviewDocument("Gutschrift") { DocumentID = "1430" });

            EditContentTabs = new ObservableCollection<UIElement>();
        }

        /// <summary>
        /// Use this constructor if you have a document loaded or you have already created an empty existing document. The UI shows the edit tabs.
        /// </summary>
        /// <param name="parentViewModel">The parent ViewModel</param>
        /// <param name="document">The loaded document</param>
        /// <param name="editEnabled">Determines wheter you can change the <see cref="Document"/>s ID.</param>
        public DocumentEditViewModel(DocumentView.DocumentTabViewModel parentViewModel, Data.Document.Document document, bool editEnabled)
        {
            ContextualTabGroup = parentViewModel.ContextualTabGroup;
            this.ParentViewModel = parentViewModel;
            LinkedDocuments = new Data.Models.DocumentFolderModel();
            DocumentEditRibbonTabItem = new DocumentEditRibbonTabItem(this);
            DocumentEditTabHolder = new DocumentEditTabHolder() { DataContext = this };
            DocumentFolderControl = new DocumentFolder(this) { DataContext = this };
            DisplayedTabContent = DocumentEditTabHolder;
            this.Document = document;
            EditMode = editEnabled;
            EditContentTabs = new ObservableCollection<UIElement>();
            DocumentEditRibbonTabItem.ShowDocumentControls();
        }

        public DocumentView.DocumentTabViewModel ParentViewModel { get; private set; }

        public Fluent.RibbonContextualTabGroup ContextualTabGroup { get; private set; }

        public DocumentEditRibbonTabItem DocumentEditRibbonTabItem { get; private set; }

        public DocumentEditTabHolder DocumentEditTabHolder { get; private set; }

        public DocumentFolder DocumentFolderControl { get; private set; }

        public Data.Models.DocumentFolderModel LinkedDocuments { get; private set; }

        public bool EditMode { get { return GetValue(() => EditMode); } private set { SetValue(value); } }

        public ObservableCollection<UIElement> EditContentTabs { get; private set; }

        public Fluent.RibbonTabItem RibbonTabItem
        {
            get { return DocumentEditRibbonTabItem; }
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
            get { return new DocumentEditTabContent(this) { DataContext = this }; ; }
        }

        /// <summary>
        /// Link to the current content of <see cref="DocumentEditTabContent"/>'s ItemsControl.
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
            LinkedDocuments.GenerateID();
            DisplayedTabContent = DocumentEditTabHolder;
            EditMode = true;
        }

        public async Task ReceiveCloseCommand()
        {
            DocumentEditRibbonTabItem.Focus();
            if (Document != null)
                await ParentViewModel.ParentViewModel.Database.UpdateTemporaryUsedDocumentID(Document.DocumentID, "", Document.DocumentType);
            await ParentViewModel.ReceiveCloseEditControl(this);
        }

        public async Task LoadData()
        {
            if (Document != null && EditMode == true)
                this.Document.DocumentID = (await ParentViewModel.ParentViewModel.Database.GetNextDocumentID(this.Document.DocumentType)).ToString();

            if (Document != null)
            {
                // Loads the DocumentFolder containing the current Document.
                var list = from Data.Models.DocumentFolderModel folder in ParentViewModel.ParentViewModel.SettingsTabViewModel.DocumentFolder where folder.Documents.Contains(new Data.Document.PreviewDocument(this.Document.DocumentType) { DocumentID = this.Document.DocumentID }) select folder;
                if (list.Count() > 0)
                    LinkedDocuments = list.First();
            }
        }

        public void ReceiveData(object data)
        {
            if (data is Data.Document.PreviewDocument)
            {
                //ToDo: Load DocumentFolder from SettingsTabViewModel
            }
            else
            {
                var factory = ParentViewModel.GetFactory(Document.DocumentType);
                factory.ReceiveData(data, Document);
            }
        }

        public Data.Customers.Customer PreviewCustomer { get { return GetValue(() => PreviewCustomer); } set { SetValue(value); } }

        public Data.Articles.Article PreviewArticle { get { return GetValue(() => PreviewArticle); } set { SetValue(value); } }

        public Data.Interfaces.IExport ExportClass { get { return GetValue(() => ExportClass); } set { SetValue(value); } }
    }
}
