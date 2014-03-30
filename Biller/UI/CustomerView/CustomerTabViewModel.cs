using NLog;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Biller.UI.CustomerView
{
    /// <summary>
    /// ViewModel for the CutomerView. This class manages all behaviour regarding customers.
    /// </summary>
    public class CustomerTabViewModel : Biller.Data.Utils.PropertyChangedHelper, Biller.UI.Interface.ITabContentViewModel
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public CustomerTabViewModel(ViewModel.MainWindowViewModel parentViewModel)
        {
            this.ParentViewModel = parentViewModel;
            tabContent = new CustomerTabContent(this) { DataContext = this };
            tabItem = new CustomerTabTabItem(this) { DataContext = this };

            //Contextual Tab Group
            ContextualTabGroup = new Fluent.RibbonContextualTabGroup() { Header = "Kunde bearbeiten", Background = Brushes.Tomato, BorderBrush = Brushes.Tomato, Visibility = System.Windows.Visibility.Visible };
            parentViewModel.RibbonFactory.AddContextualGroup(ContextualTabGroup);

            registeredObservers = new ObservableCollection<Interface.IEditObserver>();
        }

        private CustomerTabContent tabContent { get; set; }
        private CustomerTabTabItem tabItem { get; set; }

        public ObservableCollection<Data.Customers.PreviewCustomer> AllCustomers { get { return GetValue(() => AllCustomers); } set { SetValue(value); } }

        public ObservableCollection<Data.Customers.PreviewCustomer> DisplayedCustomers { get { return GetValue(() => DisplayedCustomers); } set { SetValue(value); } }

        public ObservableCollection<object> AllCategories { get { return GetValue(() => AllCategories); } set { SetValue(value); } }

        public Data.Customers.PreviewCustomer SelectedCustomer { get { return GetValue(() => SelectedCustomer); } set { SetValue(value); } }

        public object SelectedCategory { get; set; }

        public string SearchString { get; set; }

        public ViewModel.MainWindowViewModel ParentViewModel { get; private set; }

        public Fluent.RibbonContextualTabGroup ContextualTabGroup { get; private set; }

        public Fluent.RibbonTabItem RibbonTabItem
        {
            get
            {
                return tabItem;
            }
        }

        public System.Windows.UIElement TabContent
        {
            get
            {
                return tabContent;
            }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        private Biller.UI.Interface.ITabContentViewModel ViewModelRequestingCustomer { get; set; }

        public async Task LoadData()
        {
            AllCustomers = new ObservableCollection<Data.Customers.PreviewCustomer>(await ParentViewModel.Database.AllCustomers());
            DisplayedCustomers = AllCustomers;
        }

        public async Task ApplyFilter()
        {
            await Task.Run(() => applyFilter());
        }

        private void applyFilter()
        {
            DisplayedCustomers = new ObservableCollection<Data.Customers.PreviewCustomer>(AllCustomers);
        }

        public async Task ReceiveNewCustomerCommand()
        {
            logger.Debug("New Customer command called");

            var tempid = await ParentViewModel.Database.GetNextCustomerID();
            var CustomerEditViewModel = new CustomerView.Contextual.CustomerEditViewModel(this, tempid);
            ParentViewModel.AddTabContentViewModel(CustomerEditViewModel);
            CustomerEditViewModel.RibbonTabItem.IsSelected = true;

            foreach (var observer in registeredObservers)
                observer.ReceiveCustomerEditViewModel(CustomerEditViewModel);
        }

        public async Task ReceiveEditCustomerCommand(object source)
        {
            if (SelectedCustomer != null)
            {
                if (ViewModelRequestingCustomer == null)
                {
                    var temp = await ParentViewModel.Database.GetCustomer(SelectedCustomer.CustomerID);
                    var customerEditViewModel = new Contextual.CustomerEditViewModel(this, temp);
                    ParentViewModel.AddTabContentViewModel(customerEditViewModel);
                    customerEditViewModel.RibbonTabItem.IsSelected = true;

                    foreach (var observer in registeredObservers)
                        observer.ReceiveCustomerEditViewModel(customerEditViewModel);
                }
                else
                {
                    var temp = await ParentViewModel.Database.GetCustomer(SelectedCustomer.CustomerID);

                    ViewModelRequestingCustomer.ReceiveData(temp);
                    ParentViewModel.SelectedContent = ViewModelRequestingCustomer.TabContent;
                    ViewModelRequestingCustomer = null;
                }
            }
        }

        public void ReceiveCloseArticleControl(Contextual.CustomerEditViewModel customerEditViewModel)
        {
            //We need to change the visibility due to a bug in the RibbonControl which shows the contextual TabHeader after removing a visible item
            customerEditViewModel.RibbonTabItem.Visibility = System.Windows.Visibility.Collapsed;

            ParentViewModel.RibbonFactory.RemoveTabItem(customerEditViewModel.RibbonTabItem);
            RibbonTabItem.IsSelected = true;
        }

        public async Task SaveOrUpdateCustomer(Data.Customers.Customer source)
        {
            var tempPreview = Data.Customers.PreviewCustomer.FromCustomer(source);
            if (AllCustomers.Contains(tempPreview))
            {
                var index = AllCustomers.IndexOf(tempPreview);
                AllCustomers.RemoveAt(index);
                AllCustomers.Insert(index, tempPreview);
            }
            else
            {
                AllCustomers.Add(tempPreview);
            }
            await ParentViewModel.Database.SaveOrUpdateCustomer(source);
            await ApplyFilter();
        }

        public void ReceiveData(object data)
        {
            throw new NotImplementedException();
        }

        public void ReceiveRequestCustomerCommand(Biller.UI.Interface.ITabContentViewModel source)
        {
            ViewModelRequestingCustomer = source;
            ParentViewModel.SelectedContent = TabContent;
        }

        ObservableCollection<Interface.IEditObserver> registeredObservers;

        public void RegisterObserver(Interface.IEditObserver observer)
        {
            registeredObservers.Add(observer);
        }
    }
}