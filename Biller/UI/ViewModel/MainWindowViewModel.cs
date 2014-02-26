using NLog;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace Biller.UI.ViewModel
{
    /// <summary>
    /// The MainWindowViewModel is the central control station for modifing the MainWindow.\n
    /// Handling plugin-windows is also a part of the this class.
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        private System.Windows.UIElement _selectedContent;

        /// <summary>
        /// Creates a new instance of <see cref="MainWindowViewModel"/>.\n
        /// This initalization also creates all basic TabContentViewModels as well as the RibbonFactory.
        /// </summary>
        /// <param name="MainWindow">The current instance of the <see cref="MainWindow"/>.</param>
        public MainWindowViewModel(MainWindow MainWindow)
        {
            logger.Debug("Constructor of MainWindowViewModel called");

            logger.Debug("Initializing RibbonFactory");
            RibbonFactory = new Ribbon.RibbonFactory(MainWindow.ribbon);

            TabContentViewModels = new ObservableCollection<UI.Interface.ITabContentViewModel>();

            logger.Debug("Initializing OrderTabViewModel");
            OrderTabViewModel = new OrderView.OrderTabViewModel(this);
            logger.Debug("Initializing ArticleTabViewModel");
            ArticleTabViewModel = new ArticleView.ArticleTabViewModel(this);
            logger.Debug("Initializing CustomerTabViewModel");
            CustomerTabViewModel = new CustomerView.CustomerTabViewModel(this);
            logger.Debug("Initializing SettingsTabViewModel");
            SettingsTabViewModel = new SettingsView.SettingsTabViewModel(this);
            BackstageViewModel = new Backstage.BackstageViewModel(this);

            logger.Debug("Adding Viewmodels to the collection");
            AddTabContentViewModel(OrderTabViewModel);
            AddTabContentViewModel(ArticleTabViewModel);
            AddTabContentViewModel(CustomerTabViewModel);
            AddTabContentViewModel(SettingsTabViewModel);

            logger.Debug("Finished constructor of MainWindowViewModel");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The current instance of <see cref="Biller.UI.OrderView.ArticleTabViewModel"/>.
        /// </summary>
        public Biller.UI.ArticleView.ArticleTabViewModel ArticleTabViewModel { get; private set; }

        /// <summary>
        /// The current instance of <see cref="Biller.UI.OrderView.OrderTabViewModel"/>.
        /// </summary>
        public Biller.UI.OrderView.OrderTabViewModel OrderTabViewModel { get; private set; }

        /// <summary>
        /// The current instance of <see cref="Biller.UI.OrderView.CustomerTabViewModel"/>.
        /// </summary>
        public Biller.UI.CustomerView.CustomerTabViewModel CustomerTabViewModel { get; private set; }

        /// <summary>
        /// The current instance of <see cref="Biller.UI.Backstage.BackstageViewModel"/>.
        /// </summary>
        public Biller.UI.Backstage.BackstageViewModel BackstageViewModel { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Biller.UI.SettingsView.SettingsTabViewModel SettingsTabViewModel { get; private set; }

        /// <summary>
        /// The current instance of <see cref="Biller.UI.Ribbon.RibbonFactory"/>.
        /// </summary>
        public Biller.UI.Ribbon.RibbonFactory RibbonFactory { get; private set; }

        /// <summary>
        /// SelectedContent is the UIElement showing below the <see cref="MainWindow.ribbon"/>.\n
        /// Usually it is depended on the selected RibbonTabItem.
        /// </summary>
        public System.Windows.UIElement SelectedContent
        {
            get { return this._selectedContent; }
            set
            {
                if (value != this._selectedContent)
                {
                    this._selectedContent = value;
                    NotifyPropertyChanged("SelectedContent");
                }
            }
        }

        /// <summary>
        /// Holds a list of all registered <see cref="Interface.ITabContentViewModel"/>
        /// </summary>
        private ObservableCollection<UI.Interface.ITabContentViewModel> TabContentViewModels { get; set; }

        /// <summary>
        /// Registers a <see cref="Interface.ITabContentViewModel"/>.\n
        /// The <see cref="Interface.ITabContentViewModel.RibbonTabItem"/> will be added to the Ribbon.\n
        /// Be sure to set the RibbonTabItem's <see cref="RibbonTabItem.TabContent"/> as this methode adds an event handler when the TabItem gets selected
        /// </summary>
        /// <param name="TabContentViewModel"></param>
        public void AddTabContentViewModel(UI.Interface.ITabContentViewModel TabContentViewModel)
        {
            TabContentViewModels.Add(TabContentViewModel);
            RibbonFactory.AddTabItem(TabContentViewModel.RibbonTabItem);
            TabContentViewModel.RibbonTabItem.OnIsSelected += new RoutedEventHandler(tabItemGotFocus);
        }

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private void tabItemGotFocus(object sender, EventArgs e)
        {
            SelectedContent = (sender as Biller.UI.Interface.IRibbonTabItem).ParentViewModel.TabContent;
        }

        private Data.Interfaces.IDatabase database;
        public Data.Interfaces.IDatabase Database
        {
            get
            {
                return database;
            }
        }

        public async Task LoadData()
        {
            string AssemblyLocation = (Assembly.GetExecutingAssembly().Location).Replace(System.IO.Path.GetFileName(Assembly.GetExecutingAssembly().Location), "") + "Data\\";
            logger.Info("Assembly location is: " + AssemblyLocation);

            logger.Debug("Connecting database");
            database = new Data.Database.XDatabase(AssemblyLocation);
            if (await database.Connect() == true)
            {
                await database.AddAdditionalPreviewDocumentParser(new Data.Orders.DocumentParsers.InvoiceParser());
                logger.Info("Connecting to database was successfull");

                await SettingsTabViewModel.LoadData();
                await ArticleTabViewModel.LoadData();
                await CustomerTabViewModel.LoadData();
                await OrderTabViewModel.LoadData();
            }
            else
            {
                logger.Info("Connecting to database was not successfull");
                if (database.IsFirstLoad)
                {
                    logger.Info("First Load of the Database");
                }
            }
        }
    }
}