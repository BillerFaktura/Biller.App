using System;
using System.Reflection;
using System.Threading.Tasks;
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
            //var folder = new Biller.Data.Models.DocumentFolderModel();
            //folder.Documents.Add(new Data.Document.PreviewDocument("Rechnung") { DocumentID = "1000" });
            //folder.Documents.Add(new Data.Document.PreviewDocument("Lieferschein") { DocumentID = "1001" });
            //MessageBox.Show(folder.GetXElement().ToString());

            Console.WriteLine(await GetOuterString());
        }

        //private async void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    string AssemblyLocation = (Assembly.GetExecutingAssembly().Location).Replace(System.IO.Path.GetFileName(Assembly.GetExecutingAssembly().Location), "") + "Data\\";
        //    database = new Data.Database.XDatabase(AssemblyLocation);
        //    await database.Connect();
        //    var csv = new Data.Import.csv();
        //    await csv.ImportArticles("C:\\Users\\Igor\\Desktop\\listenpreise 2014 gefiltertArtikel Excel.txt", database);
        //}

        private async Task<string> GetOuterString()
        {
            return await Task<string>.Run(() => getOuterString());
        }

        private string getOuterString()
        {
            var task = GetInnerString();
            //task.RunSynchronously();
            var inner = task.Result;
            return "outer " + inner;
        }

        private async Task<string> GetInnerString()
        {
            return await Task<string>.Run(() => getInnerString());
        }

        private string getInnerString()
        {
            return "inner";
        }
    }
}