using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Data.Import.BillerV1
{
    public class Import
    {
        public Import()
        {
            bdb = new FuncClasses.FastXML(DataDirectory);
            bdb.Connect();
        }

        private FuncClasses.User user = new FuncClasses.User();

        FuncClasses.FastXML bdb;

        public string DataDirectory { get; set; }

        public Interfaces.IDatabase Database { get; set; }

        public async Task<bool> ImportCustomers()
        {
            return await Task<bool>.Run(() => importCustomers(bdb));
        }

        private bool importCustomers(FuncClasses.FastXML bdb)
        {
            var list = bdb.GetAllCustomers(user);
            var savingList = new List<Customers.Customer>();
            foreach (var cusprev in list)
            {
                var importedCustomer = new Data.Customers.Customer();

                //Load full user
                var customer = bdb.GetCustomer(cusprev.CustomerID, user);

                // Payment methode
                var payment = new Utils.PaymentMethode() {Name=customer.PaymentMethode.Name, Text=customer.PaymentMethode.Text, Discount= new Utils.Percentage() {PercentageString = customer.PaymentMethode.ReductionString}};
                Database.SaveOrUpdatePaymentMethode(payment);
                importedCustomer.DefaultPaymentMethode = payment;

                // Pricegroup
                switch(customer.Preisgruppe)
                {
                    case FuncClasses.Preisgruppe.Preisgruppe1:
                        importedCustomer.Pricegroup = 0;
                        break;
                    case FuncClasses.Preisgruppe.Preisgruppe2:
                        importedCustomer.Pricegroup = 1;
                        break;
                    case FuncClasses.Preisgruppe.Preisgruppe3:
                        importedCustomer.Pricegroup = 2;
                        break;
                }

                // ID
                importedCustomer.CustomerID = customer.CustomerID;

                // MainAddress
                var MainAddress = new Utils.Address();
                MainAddress.Addition = customer.Address.Addition;
                MainAddress.City = customer.Address.City;
                MainAddress.CompanyName = customer.Address.CompanyName;
                MainAddress.Country = customer.Address.Country;
                MainAddress.Forname = customer.Address.Forname;
                MainAddress.Surname = customer.Address.Surname;
                MainAddress.Title = customer.Address.Title;
                MainAddress.Zip = customer.Address.ZipCode;
                MainAddress.Street = customer.Address.Street;
                MainAddress.HouseNumber = customer.Address.No;
                MainAddress.Salutation = customer.Address.Salutation;
                importedCustomer.MainAddress = MainAddress;

                // Contact
                var Contact = new Utils.Contact();
                Contact.Facebook = customer.Contact.Facebook;
                Contact.Fax1 = customer.Contact.Telefax1;
                Contact.Fax2 = customer.Contact.Telefax2;
                Contact.Mail1 = customer.Contact.Mail1;
                Contact.Mail2 = customer.Contact.Mail2;
                Contact.Mobile1 = customer.Contact.Mobil1;
                Contact.Mobile2 = customer.Contact.Mobil2;
                Contact.Phone1 = customer.Contact.Telefon1;
                Contact.Phone2 = customer.Contact.Telefon2;
                Contact.Twitter = customer.Contact.Twitter;
                importedCustomer.Contact = Contact;

                // Additional adresses
                foreach(var address in customer.Addresses)
                {
                    var eAddress = new Utils.EAddress();
                    eAddress.AddressDescription = address.AddressDescription;
                    eAddress.Addition = address.Addition;
                    eAddress.City = address.City;
                    eAddress.CompanyName = address.CompanyName;
                    eAddress.Country = address.Country;
                    eAddress.Forname = address.Forname;
                    eAddress.Surname = address.Surname;
                    eAddress.Title = address.Title;
                    eAddress.Zip = address.ZipCode;
                    eAddress.Street = address.Street;
                    eAddress.HouseNumber = address.No;
                    eAddress.Salutation = address.Salutation;
                    importedCustomer.ExtraAddresses.Add(eAddress);
                }
                savingList.Add(importedCustomer);
            }
            Database.SaveOrUpdateCustomer(savingList);
            return true;
        }

        public async Task<bool> ImportArticle()
        {
            return await Task<bool>.Run(() => importArticle(bdb));
        }

        private bool importArticle(FuncClasses.FastXML bdb)
        {
            var list = bdb.GetAllArticles(user);
            var savingList = new List<Articles.Article>();
            foreach(var prevart in list)
            {
                var article = bdb.GetArticle(prevart.ArticleID, user);
                var outputArticle = new Articles.Article();

                outputArticle.ArticleID = article.ArticleID;
                outputArticle.ArticleDescription = article.ArticleDescription;
                outputArticle.ArticleCategory = article.ArticleCategory;
                outputArticle.ArticleText = article.ArticleText;
                //outputArticle.ArticleWeight = article.WeightString;
                outputArticle.Price1 = new Models.PriceModel(outputArticle) { Price1 = new Utils.EMoney(article.ArticlePrice1.Amount, article.ArticlePrice1.IsGross) };
                outputArticle.Price2 = new Models.PriceModel(outputArticle) { Price1 = new Utils.EMoney(article.ArticlePrice2.Amount, article.ArticlePrice2.IsGross) };
                outputArticle.Price3 = new Models.PriceModel(outputArticle) { Price1 = new Utils.EMoney(article.ArticlePrice3.Amount, article.ArticlePrice3.IsGross) };
                
                // TaxClass
                var TaxClass = new Utils.TaxClass();
                TaxClass.Name = article.TaxClass.Name;
                TaxClass.TaxRate = new Utils.Percentage() { PercentageString = article.TaxClass.TaxRateString };
                Database.SaveOrUpdateTaxClass(TaxClass);
                outputArticle.TaxClass = TaxClass;

                // Unit
                var ArticleUnit = new Utils.Unit();
                ArticleUnit.DecimalSeperator = ",";
                // Gets the count of digits after the seperating "."
                ArticleUnit.DecimalDigits = article.ArticleUnit.UnitFormat.Split(new Char[] { '.' })[1].Length;
                ArticleUnit.Name = article.ArticleUnit.Name;
                outputArticle.ArticleUnit = ArticleUnit;

                savingList.Add(outputArticle);
            }
            Database.SaveOrUpdateArticle(savingList);
            return true;
        }

        public async Task<bool> ImportDocuments()
        {
            return await Task<bool>.Run(() => importDocuments(bdb));
        }

        private bool importDocuments(FuncClasses.FastXML bdb)
        {
            var list = bdb.GetAllOrders(user);
            foreach (var prevOrder in list)
            {
                // We just want to import invoices
                if (prevOrder.OrderTyp != "1")
                    continue;
                var invoice = bdb.GetOrder(prevOrder.OrderID, FuncClasses.Dokumentart.Rechnung, user);
                var output = new Orders.Invoice();
                
                // Customer
                var task = Database.GetCustomer(prevOrder.CustomerID);
                output.Customer = task.Result;

                output.Date = invoice.Datum.ToDate();
                output.DocumentID = invoice.OrderID.ToString();
                output.OrderClosingText = invoice.OrderText;
                output.OrderOpeningText = invoice.TextBefore;

                // Rebate
                //!## Fixed reduction not supported ##!\\
                //!## Collect those orders and notify the user he has to adjust the rebate ##!\\
                output.OrderRebate = new Utils.Percentage() { Amount = Convert.ToDouble(invoice.Rebate) };
                
                //Payment methode
                var payment = new Utils.PaymentMethode() { Name = invoice.PaymentMethode.Name, Text = invoice.PaymentMethode.Text, Discount = new Utils.Percentage() { PercentageString = invoice.PaymentMethode.ReductionString } };
                Database.SaveOrUpdatePaymentMethode(payment);
                output.PaymentMethode = payment;

                // Articles
                foreach(var Article in invoice.OrderedArticles)
                {
                    var taskArticle = Database.GetArticle(Article.ArticleID);
                    var basearticle = taskArticle.Result;
                    var orderedArticle = new Articles.OrderedArticle(basearticle);
                    orderedArticle.ArticleText = Article.OrderText;
                    orderedArticle.OrderedAmount = Convert.ToDouble(Article.OrderedAmount);
                    orderedArticle.OrderRebate.Amount = Convert.ToDouble(Article.OrderRebate);
                    orderedArticle.OrderPrice.Price1.AmountString = Article.OrderPrice.AmountString;
                    orderedArticle.OrderPosition = Article.OrderPosition;
                    output.OrderedArticles.Add(orderedArticle);
                }
                Database.SaveOrUpdateDocument(output);
            }
            return true;
        }
    }
}
