using NLog;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;

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
        private MainWindow window;

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
            NotificationManager = new Notifications.NotificationManager();
            UpdateManager = new Core.Update.UpdateManager();
            TabContentViewModels = new ObservableCollection<UI.Interface.ITabContentViewModel>();

            logger.Debug("Initializing OrderTabViewModel");
            DocumentTabViewModel = new DocumentView.DocumentTabViewModel(this);
            logger.Debug("Initializing ArticleTabViewModel");
            ArticleTabViewModel = new ArticleView.ArticleTabViewModel(this);
            logger.Debug("Initializing CustomerTabViewModel");
            CustomerTabViewModel = new CustomerView.CustomerTabViewModel(this);
            logger.Debug("Initializing SettingsTabViewModel");
            SettingsTabViewModel = new SettingsView.SettingsTabViewModel(this);
            BackstageViewModel = new Backstage.BackstageViewModel(this);

            logger.Debug("Finished constructor of MainWindowViewModel");
            vm = this;
            window = MainWindow;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The current instance of <see cref="Biller.UI.DocumentView.ArticleTabViewModel"/>.
        /// </summary>
        public Biller.UI.ArticleView.ArticleTabViewModel ArticleTabViewModel { get; private set; }

        /// <summary>
        /// The current instance of <see cref="Biller.UI.DocumentView.DocumentTabViewModel"/>.
        /// </summary>
        public Biller.UI.DocumentView.DocumentTabViewModel DocumentTabViewModel { get; private set; }

        /// <summary>
        /// The current instance of <see cref="Biller.UI.DocumentView.CustomerTabViewModel"/>.
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

        public Biller.Core.Update.UpdateManager UpdateManager { get; private set; }

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
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }

        private void tabItemGotFocus(object sender, EventArgs e)
        {
            SelectedContent = (sender as Biller.UI.Interface.IRibbonTabItem).ParentViewModel.TabContent;
        }

        public Core.Interfaces.IDatabase Database { get; private set; }

        public Biller.UI.Notifications.NotificationManager NotificationManager { get; set; }

        public async Task LoadData(bool CompanyChanged = false)
        {
            if (!CompanyChanged)
            {
                var assemblyLocation = (Assembly.GetExecutingAssembly().Location).Replace(System.IO.Path.GetFileName(Assembly.GetExecutingAssembly().Location), "");

                UpdateManager.Register(new Core.Models.AppModel() { Title = "Biller", Description = "Hauptprogramm", GuID = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), true)[0]).Value.ToLower(), Version = 2.000814, UpdateSource = "https://raw.githubusercontent.com/LastElb/BillerV2/master/update.json" });

                logger.Info("Assembly location is: " + assemblyLocation);
                ProfileOptimization.SetProfileRoot(assemblyLocation);
                ProfileOptimization.StartProfile("DataLoading.Profile");

                foreach (var currentfile in Directory.GetFiles(assemblyLocation, "*@Biller.dll"))
                {
                    var plugin = LoadAssembly(currentfile);
                    try
                    {
                        plugin.Activate();
                        SettingsTabViewModel.RegisteredPlugins.Add(plugin);
                    }
                    catch (Exception e)
                    {
                        logger.Fatal("Error integrating plugin " + plugin.Name, e);
                        NotificationManager.ShowNotification("Fehler beim Laden von " + plugin.Name, "Das Plugin konnte nicht geladen werden. Eine genaue Fehlerbeschreibung wurde in die Logdatei geschrieben.");
                    }
                }

                var settings = new Biller.Core.Database.AppSettings();
                settings.Load();
                if (String.IsNullOrEmpty(settings.Database))
                {
                    // We need to setup a database
                    // Something with ShowDialog so we can resume from a later point
                    var setup = new StartUp.StartUpWindow(this);
                    window.Hide();
                    setup.ShowDialog();
                    settings.Load();
                    window.Show();
                    NotificationManager.ShowNotification("Hilfe & Erste Schritte", "Klicken Sie auf den Hilfebutton oben rechts, um sich über die ersten Schritte in Biller zu informieren");
                }

                logger.Debug("Connecting to database");
                var databases = SettingsTabViewModel.RegisteredDatabases;
                Database = databases.FirstOrDefault(x => x.Database.GuID == settings.Database).Database;
                if (Database == null)
                {
                    logger.Error("We could not load the database. It was not registered. Starting a new database setup!");
                    settings.Database = "";
                    settings.DatabaseOptions = "";

                    var setup = new StartUp.StartUpWindow(this);
                    window.Hide();
                    setup.ShowDialog();
                    settings.Load();
                    window.Show();
                }
            }
            if (await Database.Connect() == true)
            {
                logger.Info("Connection to database established");
                if (Database.IsFirstLoad)
                    RibbonFactory.OpenBackstage();

                TabContentViewModels.Clear();
                RibbonFactory.ClearRibbonTabItems();
                AddTabContentViewModel(DocumentTabViewModel);
                AddTabContentViewModel(ArticleTabViewModel);
                AddTabContentViewModel(CustomerTabViewModel);
                AddTabContentViewModel(SettingsTabViewModel);

                DocumentTabViewModel.RibbonTabItem.IsSelected = true;
                SelectedContent = DocumentTabViewModel.TabContent;

                foreach (var plugin in SettingsTabViewModel.RegisteredPlugins)
                    foreach (UI.Interface.IViewModel vm in plugin.ViewModels())
                        await vm.LoadData();

                await SettingsTabViewModel.LoadData();
                await ArticleTabViewModel.LoadData();
                await CustomerTabViewModel.LoadData();
                await DocumentTabViewModel.LoadData();
                await BackstageViewModel.LoadData();
            }
            else if (Database.IsFirstLoad)
                RibbonFactory.OpenBackstage();
            else
            {
                // Show an exception
            }

            UpdateManager.CheckForUpdatesCompleted += UpdateManager_CheckForUpdatesCompleted;
            UpdateManager.CheckForUpdates();
        }

        private UI.Interface.IPlugIn LoadAssembly(string assemblyPath)
        {
            string assembly = Path.GetFullPath(assemblyPath);
            Assembly ptrAssembly = Assembly.LoadFile(assembly);
            foreach (Type item in ptrAssembly.GetTypes())
            {
                if (!item.IsClass) continue;
                foreach (var interfaceitem in item.GetInterfaces())
                {
                    try
                    {
                        object[] @params = new object[1];
                        @params[0] = this;
                        bool ignoreCase = true;
                        BindingFlags bindingAttr = BindingFlags.Default;
                        Binder binder = null;
                        object[] args = @params;
                        CultureInfo culture = null;
                        object[] activationAttributes = null;

                        object instance = ptrAssembly.CreateInstance(item.FullName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes);
                        UI.Interface.IPlugIn plugin = (UI.Interface.IPlugIn)instance;
                        return plugin;
                    }
                    catch (Exception e)
                    { }
                }
            }
            throw new Exception("Invalid DLL, Interface not found!");
        }

        public void MainWindowCloseActions(System.EventArgs e)
        {
            NotificationManager.Close();
            if (SettingsTabViewModel.KeyValueStore != null)
                Database.SaveOrUpdateSettings(SettingsTabViewModel.KeyValueStore);
        }

        private static MainWindowViewModel vm;
        /// <summary>
        /// Returns the current instance of the <see cref="MainWindowViewModel"/>.\n
        /// For use in modules that don't have access to a viewmodel.
        /// </summary>
        /// <returns><see cref="MainWindowViewModel"/></returns>
        public static MainWindowViewModel GetCurrentMainWindowViewModel()
        {
            return vm;
        }

        void UpdateManager_CheckForUpdatesCompleted(object sender, EventArgs e)
        {
            foreach(var app in UpdateManager.UpdateableApps)
            {
                var update = new Biller.Controls.Notification.UpdateNotification() { Title = "Update für " + app.Title, Message = "Es ist ein Update auf Version " + app.Version.ToString() + " verfügbar." };
                Action<object> changelogAction = new Action<object>((object x) => 
                {
                    if (!String.IsNullOrEmpty(app.ChangelogURL))
                        Process.Start(app.ChangelogURL);
                });
                Action<object> updateNowAction = new Action<object>((object x) =>
                {
                    UpdateManager.Update(app, update);
                    update.ProgressBarVisibility = Visibility.Visible;
                    update.TextVisibility = Visibility.Collapsed;
                });
                Action<object> updateLaterAction = new Action<object>((object x) => 
                {
                    
                });
                update.SetActions(changelogAction, updateNowAction, updateLaterAction);
                _selectedContent.Dispatcher.BeginInvoke((Action)(() => NotificationManager.ShowNotification(update)));
                
            }
        }
    }
}