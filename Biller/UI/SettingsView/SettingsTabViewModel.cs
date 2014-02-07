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
    public class SettingsTabViewModel : Data.Utils.PropertyChangedHelper, Biller.UI.Interface.ITabContentViewModel
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public SettingsTabViewModel(ViewModel.MainWindowViewModel parentViewModel)
        {
            logger.Debug("Constructor of SettingsTabViewModel called");
            logger.Debug("Is parent viewmodel null: " + (parentViewModel == null ? "true" : "false"));

            SettingsList = new ObservableCollection<TabItem>();
            ArticleUnits = new ObservableCollection<Data.Utils.Unit>();
            PaymentMethodes = new ObservableCollection<Data.Utils.PaymentMethode>();

            logger.Debug("Creating new UnitTabItem");
            SettingsList.Add(new SettingsList.UnitSettings.UnitTabItem());
            SettingsList.Add(new SettingsList.PaymentSettings.PaymentTabItem());
            SettingsList.Add(new SettingsList.TaxClassSettings.TaxClassTabItem());
            SettingsList.Add(new SettingsList.ShipmentSettings.ShipmentTabItem());
            TabContent = new SettingsTabContent() { DataContext = this };
            RibbonTabItem = new SettingsTabRibbonTabItem(this) { DataContext = this };
            ParentViewModel = parentViewModel;

            SelectedUnit = new Data.Utils.Unit();
            SelectedPaymentMethode = new Data.Utils.PaymentMethode();
            SelectedTaxClass = new Data.Utils.TaxClass();
            SelectedShipment = new Data.Utils.Shipment();

            logger.Info("Finished constructor of SettingsTabViewModel");
        }

        public async Task LoadData()
        {
            logger.Debug("Start loading data in SettingsTabViewModel");
            await ParentViewModel.Database.RegisterStorageableItem(new Data.Utils.Shipment());
            ArticleUnits = new ObservableCollection<Data.Utils.Unit>(await ParentViewModel.Database.ArticleUnits());
            PaymentMethodes = new ObservableCollection<Data.Utils.PaymentMethode>(await ParentViewModel.Database.PaymentMethodes());
            TaxClasses = new ObservableCollection<Data.Utils.TaxClass>(await ParentViewModel.Database.TaxClasses());
            var result = await ParentViewModel.Database.AllStorageableItems(new Data.Utils.Shipment());
            Shipments = new ObservableCollection<Data.Utils.Shipment>();
            foreach (Data.Utils.Shipment item in result)
                Shipments.Add(item);
            Data.GlobalSettings.UseGermanSupplementaryTaxRegulation = true;
            Data.GlobalSettings.TaxSupplementaryWorkSeperate = true;
            Data.GlobalSettings.LocalizedOnSupplementaryWork = "auf Nebenleistung";
            logger.Debug("Finished loading data in SettingsTabViewModel");
        }

        public Fluent.RibbonTabItem RibbonTabItem { get { return GetValue(() => RibbonTabItem); } private set { SetValue(value); } }

        public System.Windows.UIElement TabContent { get { return GetValue(() => TabContent); } private set { SetValue(value); } }

        public ObservableCollection<TabItem> SettingsList { get { return GetValue(() => SettingsList); } set { SetValue(value); } }

        public ObservableCollection<Data.Utils.Unit> ArticleUnits { get { return GetValue(() => ArticleUnits); } set { SetValue(value); } }

        public ObservableCollection<Data.Utils.PaymentMethode> PaymentMethodes { get { return GetValue(() => PaymentMethodes); } set { SetValue(value); } }

        public ObservableCollection<Data.Utils.TaxClass> TaxClasses { get { return GetValue(() => TaxClasses); } set { SetValue(value); } }

        public ObservableCollection<Data.Utils.Shipment> Shipments { get { return GetValue(() => Shipments); } set { SetValue(value); } }

        public Data.Utils.Unit SelectedUnit { get { return GetValue(() => SelectedUnit); } set { SetValue(value); } }

        public Data.Utils.PaymentMethode SelectedPaymentMethode { get { return GetValue(() => SelectedPaymentMethode); } set { SetValue(value); } }

        public Data.Utils.TaxClass SelectedTaxClass { get { return GetValue(() => SelectedTaxClass); } set { SetValue(value); } }

        public Data.Utils.Shipment SelectedShipment { get { return GetValue(() => SelectedShipment); } set { SetValue(value); } }

        public ViewModel.MainWindowViewModel ParentViewModel { get { return GetValue(() => ParentViewModel); } set { SetValue(value); } }

        /// <summary>
        /// Saves a new object if it does not exist or updates an existing one.
        /// </summary>
        /// <param name="source"></param>
        public void SaveOrUpdateArticleUnit(Data.Utils.Unit source)
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
        public void SaveOrUpdatePaymentMethode(Data.Utils.PaymentMethode source)
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
        public void SaveOrUpdateTaxClass(Data.Utils.TaxClass source)
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
        public void SaveOrUpdateShipment(Data.Utils.Shipment source)
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

        public void ReceiveData(object data)
        {
            throw new NotImplementedException();
        }
    }
}
