using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Biller.UI.SettingsView
{
    public class SettingsTabViewModel : Core.Utils.PropertyChangedHelper, Biller.UI.Interface.ITabContentViewModel
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public SettingsTabViewModel(ViewModel.MainWindowViewModel parentViewModel)
        {
            logger.Debug("Constructor of SettingsTabViewModel called");
            logger.Debug("Is parent viewmodel null: " + (parentViewModel == null ? "true" : "false"));

            SettingsList = new ObservableCollection<TabItem>();
            ArticleUnits = new ObservableCollection<Core.Utils.Unit>();
            PaymentMethodes = new ObservableCollection<Core.Utils.PaymentMethode>();
            DocumentFolder = new ObservableCollection<Core.Models.DocumentFolderModel>();
            RegisteredExportClasses = new ObservableCollection<Core.Interfaces.IExport>();
            PreferedExportClasses = new ObservableCollection<Core.Models.DocumentExportModel>();
            RegisteredPlugins = new ObservableCollection<Interface.IPlugIn>();
            RegisteredDatabases = new ObservableCollection<Core.Models.DatabaseUIModel>();
            KeyValueStore = new Core.Utils.KeyValueStore();

            logger.Debug("Creating new UnitTabItem");
            SettingsList.Add(new SettingsList.UnitSettings.UnitTabItem());
            SettingsList.Add(new SettingsList.PaymentSettings.PaymentTabItem());
            SettingsList.Add(new SettingsList.TaxClassSettings.TaxClassTabItem());
            SettingsList.Add(new SettingsList.ShipmentSettings.ShipmentTabItem());
            TabContent = new SettingsTabContent() { DataContext = this };
            RibbonTabItem = new SettingsTabRibbonTabItem(this) { DataContext = this };
            ParentViewModel = parentViewModel;

            SelectedUnit = new Core.Utils.Unit();
            SelectedPaymentMethode = new Core.Utils.PaymentMethode();
            SelectedTaxClass = new Core.Utils.TaxClass();
            SelectedShipment = new Core.Utils.Shipment();

            logger.Info("Finished constructor of SettingsTabViewModel");
        }

        /// <summary>
        /// Loads all kind of data asynchronously.\n
        /// Registers following <see cref="IStorageableItem"/>s:
        /// <list type="bullet">
        /// <item><see cref="Core.Utils.Shipment"/></item>
        /// <item><see cref="Core.Models.DocumentFolderModel"/></item>
        /// <item><see cref="Core.Models.CompanySettings"/></item>
        /// <item><see cref="Core.Models.DocumentExportModel"/></item>
        /// </list>
        /// \n
        /// Loads following Lists / Collections:
        /// <list type="bullet">
        /// <item><see cref="ArticleUnits"/></item>
        /// <item><see cref="PaymentMethodes"/></item>
        /// <item><see cref="TaxClasses"/></item>
        /// <item><see cref="Shipments"/></item>
        /// <item><see cref="DocumentFolder"/></item>
        /// </list>
        /// </summary>
        /// <returns></returns>
        public async Task LoadData()
        {
            logger.Debug("Start loading data in SettingsTabViewModel");
            await ParentViewModel.Database.RegisterStorageableItem(new Core.Utils.Shipment());
            await ParentViewModel.Database.RegisterStorageableItem(new Core.Models.DocumentFolderModel());
            await ParentViewModel.Database.RegisterStorageableItem(new Core.Models.CompanySettings());
            await ParentViewModel.Database.RegisterStorageableItem(new Core.Models.DocumentExportModel());

            ArticleUnits = new ObservableCollection<Core.Utils.Unit>(await ParentViewModel.Database.ArticleUnits());
            PaymentMethodes = new ObservableCollection<Core.Utils.PaymentMethode>(await ParentViewModel.Database.PaymentMethodes());
            TaxClasses = new ObservableCollection<Core.Utils.TaxClass>(await ParentViewModel.Database.TaxClasses());

            var result = await ParentViewModel.Database.AllStorageableItems(new Core.Utils.Shipment());
            Shipments = new ObservableCollection<Core.Utils.Shipment>();
            foreach (Core.Utils.Shipment item in result)
                Shipments.Add(item);

            var resultFolder = await ParentViewModel.Database.AllStorageableItems(new Core.Models.DocumentFolderModel());
            DocumentFolder = new ObservableCollection<Core.Models.DocumentFolderModel>();
            foreach (Core.Models.DocumentFolderModel item in resultFolder)
                DocumentFolder.Add(item);

            var resultExport = await ParentViewModel.Database.AllStorageableItems(new Core.Models.DocumentExportModel());
            foreach (Core.Models.DocumentExportModel item in resultExport)
                RegisterPreferedExportClass(item);

            KeyValueStore = await ParentViewModel.Database.GetSettings();
            KeyValueStore.PropertyChanged += KeyValueStore_PropertyChanged;
            logger.Debug("Finished loading data in SettingsTabViewModel");
        }

        void KeyValueStore_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ParentViewModel.Database.SaveOrUpdateSettings(KeyValueStore);
        }

        /// <summary>
        /// The <see cref="Fluent.RibbonTabItem"/> that is shown to the user as "Settings" tab.
        /// </summary>
        public Fluent.RibbonTabItem RibbonTabItem { get { return GetValue(() => RibbonTabItem); } private set { SetValue(value); } }

        /// <summary>
        /// The content shown to the user. Used for data binding.
        /// </summary>
        public System.Windows.UIElement TabContent { get { return GetValue(() => TabContent); } private set { SetValue(value); } }

        /// <summary>
        /// Holds a list of all registered SettingsTabs as <see cref="TabItem"/>.
        /// </summary>
        public ObservableCollection<TabItem> SettingsList { get { return GetValue(() => SettingsList); } set { SetValue(value); } }

        /// <summary>
        /// Holds a list of all saved <see cref="Core.Utils.Unit"/>.\n
        /// Initial loading of the content happens in <see cref="LoadData"/>.
        /// </summary>
        public ObservableCollection<Core.Utils.Unit> ArticleUnits { get { return GetValue(() => ArticleUnits); } set { SetValue(value); } }

        /// <summary>
        /// Holds a list of all saved <see cref="Core.Utils.PaymentMethode"/>.\n
        /// Initial loading of the content happens in <see cref="LoadData"/>.
        /// </summary>
        public ObservableCollection<Core.Utils.PaymentMethode> PaymentMethodes { get { return GetValue(() => PaymentMethodes); } set { SetValue(value); } }

        /// <summary>
        /// Holds a list of all saved <see cref="Core.Utils.TaxClass"/>.\n
        /// Initial loading of the content happens in <see cref="LoadData"/>.
        /// </summary>
        public ObservableCollection<Core.Utils.TaxClass> TaxClasses { get { return GetValue(() => TaxClasses); } set { SetValue(value); } }

        /// <summary>
        /// Holds a list of all saved <see cref="Core.Utils.Shipment"/>.\n
        /// Initial loading of the content happens in <see cref="LoadData"/>.
        /// </summary>
        public ObservableCollection<Core.Utils.Shipment> Shipments { get { return GetValue(() => Shipments); } set { SetValue(value); } }

        /// <summary>
        /// Holds a list of all saved <see cref="Core.Models.DocumentFolderModel"/>.\n
        /// Initial loading of the content happens in <see cref="LoadData"/>.
        /// </summary>
        public ObservableCollection<Core.Models.DocumentFolderModel> DocumentFolder { get { return GetValue(() => DocumentFolder); } set { SetValue(value); } }

        /// <summary>
        /// Holds a list of all registered <see cref="Core.Interfaces.IExport"/>.\n
        /// Initial loading of the content happens in <see cref="LoadData"/>.
        /// </summary>
        public ObservableCollection<Core.Interfaces.IExport> RegisteredExportClasses { get { return GetValue(() => RegisteredExportClasses); } set { SetValue(value); } }

        public ObservableCollection<Core.Models.DatabaseUIModel> RegisteredDatabases { get { return GetValue(() => RegisteredDatabases); } set { SetValue(value); } }

        public ObservableCollection<Interface.IPlugIn> RegisteredPlugins { get { return GetValue(() => RegisteredPlugins); } set { SetValue(value); } }

        private ObservableCollection<Core.Models.DocumentExportModel> PreferedExportClasses { get { return GetValue(() => PreferedExportClasses); } set { SetValue(value); } }

        /// <summary>
        /// The <see cref="KeyValueStore"/> is designed to hold multiple configuration values from any class. Values inside the <see cref="KeyValueStore"/> will not be saved and loaded with closing / starting the app!
        /// </summary>
        public Biller.Core.Utils.KeyValueStore KeyValueStore { get { return GetValue(() => KeyValueStore); } set { SetValue(value); } }

        public Core.Utils.Unit SelectedUnit { get { return GetValue(() => SelectedUnit); } set { SetValue(value); } }

        public Core.Utils.PaymentMethode SelectedPaymentMethode { get { return GetValue(() => SelectedPaymentMethode); } set { SetValue(value); } }

        public Core.Utils.TaxClass SelectedTaxClass { get { return GetValue(() => SelectedTaxClass); } set { SetValue(value); } }

        public Core.Utils.Shipment SelectedShipment { get { return GetValue(() => SelectedShipment); } set { SetValue(value); } }

        public ViewModel.MainWindowViewModel ParentViewModel { get { return GetValue(() => ParentViewModel); } set { SetValue(value); } }

        /// <summary>
        /// Saves a new object if it does not exist or updates an existing one.
        /// </summary>
        /// <param name="source"></param>
        public void SaveOrUpdateArticleUnit(Core.Utils.Unit source)
        {
            if (ArticleUnits.Contains(source))
            {
                var index = ArticleUnits.IndexOf(source);
                ArticleUnits.RemoveAt(index);
                ArticleUnits.Insert(index, source);
            }
            else
            {
                ArticleUnits.Add(source);
            }
            ParentViewModel.Database.SaveOrUpdateArticleUnit(source);
        }

        /// <summary>
        /// Saves a new object if it does not exist or updates an existing one.
        /// </summary>
        /// <param name="source"></param>
        public void SaveOrUpdatePaymentMethode(Core.Utils.PaymentMethode source)
        {
            if (PaymentMethodes.Contains(source))
            {
                var index = PaymentMethodes.IndexOf(source);
                PaymentMethodes.RemoveAt(index);
                PaymentMethodes.Insert(index, source);
            }
            else
            {
                PaymentMethodes.Add(source);
            }
            ParentViewModel.Database.SaveOrUpdatePaymentMethode(source);
        }

        /// <summary>
        /// Saves a new object if it does not exist or updates an existing one.
        /// </summary>
        /// <param name="source"></param>
        public void SaveOrUpdateTaxClass(Core.Utils.TaxClass source)
        {
            if (TaxClasses.Contains(source))
            {
                var index = TaxClasses.IndexOf(source);
                TaxClasses.RemoveAt(index);
                TaxClasses.Insert(index, source);
            }
            else
            {
                TaxClasses.Add(source);
            }
            ParentViewModel.Database.SaveOrUpdateTaxClass(source);
        }

        /// <summary>
        /// Saves a new object if it does not exist or updates an existing one.
        /// </summary>
        /// <param name="source"></param>
        public void SaveOrUpdateShipment(Core.Utils.Shipment source)
        {
            if (Shipments.Contains(source))
            {
                var index = Shipments.IndexOf(source);
                Shipments.RemoveAt(index);
                Shipments.Insert(index, source);
            }
            else
            {
                Shipments.Add(source);
            }
            ParentViewModel.Database.SaveOrUpdateStorageableItem(source);
        }

        public void SaveOrUpdateDocumentFolder(Core.Models.DocumentFolderModel source)
        {
            if (DocumentFolder.Contains(source))
            {
                var index = DocumentFolder.IndexOf(source);
                DocumentFolder.RemoveAt(index);
                DocumentFolder.Insert(index, source);
            }
            else
            {
                DocumentFolder.Add(source);
            }
            ParentViewModel.Database.SaveOrUpdateStorageableItem(source);
        }

        public void ReceiveData(object data)
        {
            
        }

        public void RegisterPreferedExportClass(Core.Models.DocumentExportModel source)
        {
            var exportList = RegisteredExportClasses.Where(x => x.GuID == source.Export.GuID);
            if (exportList.Count() > 0)
            {
                source.Export = exportList.First();
                PreferedExportClasses.Add(source);
            }
            else
            {
                var LayoutList = RegisteredExportClasses.Where(x => x.AvailableDocumentTypes().Contains(source.Document.DocumentType));
                if (LayoutList.Count() > 0)
                {
                    ParentViewModel.NotificationManager.ShowNotification("Layout festgelegt", "Für " + source.Document.LocalizedDocumentType + " wurde automatisch ein bevorzugtes Layout festgelegt.");
                    source.Export = LayoutList.First();
                    PreferedExportClasses.Add(source);
                }
                else
                {
                    ParentViewModel.NotificationManager.ShowNotification("Kein Layout verfügbar", "Für " + source.Document.LocalizedDocumentType + " existiert kein Layout mehr. Dokumente können nicht mehr gedruckt und exportiert werden.");
                }
            }
        }

        /// <summary>
        /// Returns null if there is no <see cref="IExport"/> inherited class available that also supports the given <see cref="Document.DocumentType"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public Core.Interfaces.IExport GetPreferedExportClass(Core.Document.Document source)
        {
            var preferedList = PreferedExportClasses.Where(x => x.Document.DocumentType == source.DocumentType);
            if (preferedList.Count() > 0)
            {
                return preferedList.First().Export;
            }
            var exportList = RegisteredExportClasses.Where(x => x.AvailableDocumentTypes().Contains(source.DocumentType));
            if (exportList.Count() > 0)
            {
                ParentViewModel.NotificationManager.ShowNotification("Layout festgelegt", "Für " + source.LocalizedDocumentType + " wurde automatisch ein bevorzugtes Layout festgelegt.");

                var preferedModel = new Core.Models.DocumentExportModel() { Export = exportList.First(), Document = source };
                PreferedExportClasses.Add(preferedModel);
                ParentViewModel.Database.SaveOrUpdateStorageableItem(preferedModel);
                return preferedModel.Export;
            }

            return null;
        }
    }
}
