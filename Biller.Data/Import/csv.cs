using CsvHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Data.Import
{
    public class csv
    {
        public csv()
        { }

        public async Task<bool> ImportArticles(string filepath, Interfaces.IDatabase database)
        {
            return await Task.Run(() => importArticles(filepath, database));
        }

        private bool importArticles(string filepath, Interfaces.IDatabase database)
        {
            using (System.IO.TextReader textreader = System.IO.File.OpenText(filepath))
            {
                var csv = new CsvReader(textreader);
                csv.Configuration.Delimiter = ";";

                var taskedUnits = database.ArticleUnits();
                var unitlist = taskedUnits.Result;

                var output = new List<Data.Articles.Article>();

                while (csv.Read())
                {
                    string ID;
                    if (!csv.TryGetField("Artikelnummer", out ID))
                    {
                        var task = database.GetNextArticleID();
                        ID = (task.Result).ToString();
                    }

                    string description;
                    if (!csv.TryGetField("Bezeichnung", out description))
                    {
                        description = "" ;
                    }

                    string text;
                    if (!csv.TryGetField("Text", out text))
                    {
                        text = "";
                    }

                    string unit;
                    if (!csv.TryGetField("Einheit", out unit))
                    {
                        unit = "";
                    }

                    string price1;
                    if (!csv.TryGetField("Preis 1", out price1))
                    {
                        price1 = "";
                    }

                    string tax;
                    if (!csv.TryGetField("Steuersatz", out tax))
                    {
                        tax = "";
                    }

                    var article = new Articles.Article();
                    article.ArticleID = ID;
                    article.ArticleDescription = description;
                    article.ArticleText = text;
                    article.Price1.Price1.AmountString = price1;

                    var list = from units in unitlist where units.ShortName.ToLower().Contains(unit.ToLower()) select units;
                    if (list.Count() > 0)
                        article.ArticleUnit = list.First();

                    //var saveresult = database.SaveOrUpdateArticle(article);
                    //if (!saveresult.Result)
                    //    database.SaveOrUpdateArticle(article);

                    output.Add(article);
                }
                database.SaveOrUpdateArticle(output);
            }
            return true;
        }
    }
}
