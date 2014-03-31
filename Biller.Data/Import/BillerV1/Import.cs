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
        }

        private FuncClasses.User user = new FuncClasses.User();

        public string DataDirectory { get; set; }

        public Interfaces.IDatabase Database { get; set; }

        public async Task<bool> ImportCustomers()
        {
            return await Task<bool>.Run(() => importCustomers());
        }

        private bool importCustomers()
        {
            var bdb = new FuncClasses.FastXML(DataDirectory);
            bdb.Connect();
            var list = bdb.GetAllCustomers(user);
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
            }
            return true;
        }
    }
}
