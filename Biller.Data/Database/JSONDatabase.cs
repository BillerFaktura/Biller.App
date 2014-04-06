using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Data.Database
{
    /// <summary>
    /// Not yet implemented
    /// </summary>
    public class JSONDatabase : Interfaces.IDatabase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public JSONDatabase(string path)
        {
            if (!path.EndsWith("\\"))
                path = path + "\\";
            DatabasePath = path;
            logger.Info("JSONDatabase path:" + path);
            RegisteredAdditionalDBs = new List<Interfaces.IXMLStorageable>();
            AdditionalPreviewParsers = new List<Interfaces.DocumentParser>();
        }

        private string DatabasePath { get; set; }
        private JArray DocumentDB { get; set; }
        private JArray ArticleDB { get; set; }
        private JArray CustomerDB { get; set; }
        private JArray SettingsDB { get; set; }
        private List<JArray> OtherDBs { get; set; }
        private List<Interfaces.DocumentParser> AdditionalPreviewParsers { get; set; }
        private List<Interfaces.IXMLStorageable> RegisteredAdditionalDBs { get; set; }

        public bool IsFirstLoad { get; private set; }

        public bool IsLoaded { get; private set; }

        public Task<bool> Connect()
        {
            throw new NotImplementedException();
        }

        private bool Initialize()
        {
            IsFirstLoad = false;
            
            if (!File.Exists(DatabasePath + "Company.json"))
            {
                logger.Debug(DatabasePath + "Company.json" + " didn't exist -> First load");
                IsFirstLoad = true;
                return false;
            }

            try
            {
                CurrentCompany = JsonConvert.DeserializeObject<Interfaces.CompanyInformation>(File.ReadAllText(DatabasePath + "Settings.json"));
            }
            catch (Exception e)
            {
                logger.FatalException("Parsing LastCompany failed!", e);
                return false;
            }

            try
            {
                //Orders
                if (!File.Exists(DatabasePath + CurrentCompany.CompanyID + "\\Documents.json"))
                    using (StreamWriter writer = File.CreateText(DatabasePath + CurrentCompany.CompanyID + "\\Documents.json"))
                    {
                        DocumentDB = new JArray();
                        writer.Write(JsonConvert.SerializeObject(DocumentDB));
                    }
                DocumentDB = JsonConvert.DeserializeObject<JArray>(File.ReadAllText(DatabasePath + "\\Documents.json"));
                logger.Debug("DocumentDB successfully loaded");
            }
            catch (Exception e)
            {
                logger.FatalException("Initializing DocumentDB failed!", e);
                return false;
            }

            try
            {
                // Articles
                if (!File.Exists(DatabasePath + CurrentCompany.CompanyID + "\\Articles.json"))
                    using (StreamWriter writer = File.CreateText(DatabasePath + CurrentCompany.CompanyID + "\\Articles.json"))
                    {
                        ArticleDB = new JArray();
                        writer.Write(JsonConvert.SerializeObject(ArticleDB));
                    }
                ArticleDB = JsonConvert.DeserializeObject<JArray>(File.ReadAllText(DatabasePath + "\\Articles.json"));
                logger.Debug("ArticleDB successfully loaded");
            }
            catch (Exception e)
            {
                logger.FatalException("Initializing ArticleDB failed!", e);
                return false;
            }

            try
            {
                // Customers
                if (!File.Exists(DatabasePath + CurrentCompany.CompanyID + "\\Customers.json"))
                    using (StreamWriter writer = File.CreateText(DatabasePath + CurrentCompany.CompanyID + "\\Customers.json"))
                    {
                        CustomerDB = new JArray();
                        writer.Write(JsonConvert.SerializeObject(CustomerDB));
                    }
                CustomerDB = JsonConvert.DeserializeObject<JArray>(File.ReadAllText(DatabasePath + "\\Customers.json"));
                logger.Debug("CustomerDB successfully loaded");
            }
            catch (Exception e)
            {
                logger.FatalException("Initializing CustomerDB failed!", e);
                return false;
            }

            logger.Info("XDatabase connected");
            return true;
        }

        public Interfaces.CompanyInformation CurrentCompany { get; private set; }

        public void AddCompany(Interfaces.CompanyInformation source)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ChangeCompany(Interfaces.CompanyInformation target)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Interfaces.CompanyInformation>> GetCompanyList()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Utils.Unit>> ArticleUnits()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Utils.PaymentMethode>> PaymentMethodes()
        {
            throw new NotImplementedException();
        }

        public void SaveOrUpdateArticleUnit(Utils.Unit source)
        {
            throw new NotImplementedException();
        }

        public void SaveOrUpdatePaymentMethode(Utils.PaymentMethode source)
        {
            throw new NotImplementedException();
        }

        public void SaveOrUpdateTaxClass(Utils.TaxClass source)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Utils.TaxClass>> TaxClasses()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Articles.PreviewArticle>> AllArticles()
        {
            throw new NotImplementedException();
        }

        public Task<bool> ArticleExists(string ArticleID)
        {
            throw new NotImplementedException();
        }

        public Task<Articles.Article> GetArticle(string ArticleID)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNextArticleID()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveOrUpdateArticle(Articles.Article source)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveOrUpdateArticle(IEnumerable<Articles.Article> source)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTemporaryUsedArticleID(string oldvalue, string newvalue)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Customers.PreviewCustomer>> AllCustomers()
        {
            throw new NotImplementedException();
        }

        public Task<bool> CustomerExists(string CustomerID)
        {
            throw new NotImplementedException();
        }

        public Task<Customers.Customer> GetCustomer(string CustomerID)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNextCustomerID()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveOrUpdateCustomer(Customers.Customer source)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTemporaryUsedCustomerID(string oldvalue, string newvalue)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveOrUpdateCustomer(IEnumerable<Customers.Customer> source)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Document.PreviewDocument>> DocumentsInInterval(DateTime IntervalStart, DateTime IntervalEnd)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Document.PreviewDocument>> DocumentsInInterval(DateTime IntervalStart, DateTime IntervalEnd, string DocumentType)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DocumentExists(Document.Document source)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetNextDocumentID(string DocumentType)
        {
            throw new NotImplementedException();
        }

        public Task<Document.Document> GetDocument(Document.Document source)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveOrUpdateDocument(Document.Document source)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTemporaryUsedDocumentID(string oldID, string newID, string DocumentType)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddAdditionalPreviewDocumentParser(Interfaces.DocumentParser parser)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Interfaces.IXMLStorageable>> AllStorageableItems(Interfaces.IXMLStorageable referenceStorageable)
        {
            throw new NotImplementedException();
        }

        public Task<bool> StorageableItemExists(Interfaces.IXMLStorageable referenceStorageable)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveOrUpdateStorageableItem(Interfaces.IXMLStorageable StorageableItem)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegisterStorageableItem(Interfaces.IXMLStorageable StorageableItem)
        {
            throw new NotImplementedException();
        }
    }
}
