﻿using NLog;
using System.Diagnostics;
namespace Biller
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private Data.Performance.Stopwatch sw;

        public UI.ViewModel.MainWindowViewModel MainWindowViewModel { get; private set; }

        public MainWindow()
        {
            logger.Debug("Application started");
            sw = new Data.Performance.Stopwatch("Application loaded after ");
            sw.Start();

            InitializeComponent();

            MainWindowViewModel = new UI.ViewModel.MainWindowViewModel(this);
            DataContext = MainWindowViewModel;
            //if (MainWindowViewModel.Database.IsFirstLoad == true)
            //{
            //    var temp = new Data.Models.CompanyInformation("Test");
            //    MainWindowViewModel.Database.AddCompany(temp);
            //    MainWindowViewModel.Database.ChangeCompany(temp);
            //}

            //MainWindowViewModel.OrderTabViewModel.OrderTabContent.AddNewColumn("Auftragsstatus", "State", 0.8);

            //dynamic sampleOrder = new Data.Orders.PreviewOrder();
            //sampleOrder.State = "Printed";

            //MainWindowViewModel.OrderTabViewModel.DisplayedOrders.Add(sampleOrder);
            //var window = new TestWindow() { DataContext = MainWindowViewModel };
            //window.Show();
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