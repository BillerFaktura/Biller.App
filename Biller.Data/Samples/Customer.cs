using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Data.Samples
{
    public class Customer : Data.Customers.Customer
    {
        public Customer()
        {
            CustomerID = "1000";
            IsCompany = false;
            MainAddress = new Utils.Address() { Salutation = "Herr", Forname = "Max", Surname = "Mustermann",
                Street = "Musterstraße", HouseNumber = "1", Zip = "12345", City = "Musterstadt" };
        }

    }
}
