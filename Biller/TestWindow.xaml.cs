using System.Reflection;
using System.Windows;

namespace Biller
{
    /// <summary>
    /// Interaktionslogik für TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Fluent.MetroWindow
    {
        private Data.Interfaces.IDatabase database;

        public TestWindow()
        {
            InitializeComponent();
        }

        private async void window_Loaded(object sender, RoutedEventArgs e)
        {
            string AssemblyLocation = (Assembly.GetExecutingAssembly().Location).Replace(System.IO.Path.GetFileName(Assembly.GetExecutingAssembly().Location), "") + "Data\\";
            database = new Data.Database.XDatabase(AssemblyLocation);
            await database.Connect();
            //await database.AddAdditionalPreviewDocumentParser(new Data.Orders.DocumentParsers.InvoiceParser());
            var unit = new Biller.Data.Utils.Unit();
            unit.DecimalDigits=3;
            unit.DecimalSeperator = ",";
            unit.Name = "Kilogramm";
            unit.ShortName = "kg";
            await database.RegisterStorageableItem(unit);

            await database.SaveOrUpdateStorageableItem(unit);

            var list = await database.AllStorageableItems(unit);
        }

        //private async void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    string AssemblyLocation = (Assembly.GetExecutingAssembly().Location).Replace(System.IO.Path.GetFileName(Assembly.GetExecutingAssembly().Location), "") + "Data\\";
        //    database = new Data.Database.XDatabase(AssemblyLocation);
        //    await database.Connect();
        //    var csv = new Data.Import.csv();
        //    await csv.ImportArticles("C:\\Users\\Igor\\Desktop\\listenpreise 2014 gefiltertArtikel Excel.txt", database);
        //}
    }
}