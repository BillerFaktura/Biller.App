using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Data.Orders
{
    public class EUInternationalInvoice : Invoice
    {
        public EUInternationalInvoice()
        {
            OrderCalculation = new Calculations.EUInternationalCalculation(this);
        }

        public override DefaultOrderCalculation OrderCalculation { get; set; }
    }
}
