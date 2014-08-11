using NLog;
using System;
using System.Diagnostics;
using System.Windows;
namespace Biller
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Core.Performance.Stopwatch sw;

        public UI.ViewModel.MainWindowViewModel MainWindowViewModel { get; private set; }

        public MainWindow()
        {
            logger.Debug("Application started");
            sw = new Core.Performance.Stopwatch("Application loaded after ");
            sw.Start();

            InitializeComponent();

            MainWindowViewModel = new UI.ViewModel.MainWindowViewModel(this);
            DataContext = MainWindowViewModel;
        }

        private async void RibbonWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await MainWindowViewModel.LoadData();
            sw.Stop();
            logger.Debug(sw.Result());
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Process.Start("https://github.com/LastElb/BillerV2/issues");
        }

        protected override void OnClosed(System.EventArgs e)
        {
            MainWindowViewModel.MainWindowCloseActions(e);
            base.OnClosed(e);
        }
    }
}