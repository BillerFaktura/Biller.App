using Biller.Data;
using Biller.Data.Articles;
using Biller.Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTypes_Biller.Calculations
{
    public class EUInternationalCalculation : DefaultOrderCalculation
    {
        public EUInternationalCalculation(Order.Order parentOrder) : base(parentOrder)
        {
            _parentOrder = parentOrder;
        }

        private Order.Order _parentOrder;

        public override void CalculateValues()
        {
            TaxValues.Clear();
            ArticleSummary.Amount = 0;
            // iterate through each article and add its value
            foreach (Biller.Data.Articles.OrderedArticle article in _parentOrder.OrderedArticles)
            {
                NetArticleSummary.Amount += article.RoundedNetOrderValue.Amount;
            }

            //NetArticleSummary
            NetArticleSummary.Amount = ArticleSummary.Amount;

            NetOrderSummary.Amount = NetArticleSummary.Amount;

            // Discount
            if (_parentOrder.OrderRebate.Amount > 0)
            {
                var temp = OrderSummary.Amount;
                NetOrderRebate.Amount = NetOrderSummary.Amount - NetOrderSummary.Amount * (1 - _parentOrder.OrderRebate.Amount);
                NetOrderSummary.Amount = NetOrderSummary.Amount * (1 - _parentOrder.OrderRebate.Amount);
                OrderRebate = new EMoney(temp - OrderSummary.Amount);
                foreach (var item in TaxValues)
                    item.Value = item.Value * (1 - _parentOrder.OrderRebate.Amount);
            }
            else
            {
                OrderRebate = new EMoney(0);
            }

            // Shipping
            if (!String.IsNullOrEmpty(_parentOrder.OrderShipment.Name))
            {
                var shipment = new OrderedArticle(new Article());
                shipment.TaxClass = GlobalSettings.ShipmentTaxClass;
                shipment.OrderedAmount = 1;
                shipment.OrderPrice.Price1 = _parentOrder.OrderShipment.DefaultPrice;
                TaxValues.Add(new Biller.Data.Models.TaxClassMoneyModel() { Value = new Money(shipment.ExactVAT), TaxClass = shipment.TaxClass, TaxClassAddition = GlobalSettings.LocalizedOnSupplementaryWork });
                NetShipment.Amount = _parentOrder.OrderShipment.DefaultPrice.Amount - shipment.ExactVAT;
                NetOrderSummary.Amount += NetShipment.Amount;
            }

            // Skonto
            if (_parentOrder.PaymentMethode.Discount.Amount > 0)
            {
                CashBack = new EMoney(_parentOrder.PaymentMethode.Discount.Amount * ArticleSummary.Amount);
            }
            else
            {
                CashBack = new EMoney(0);
            }


            RaiseUpdateManually("ArticleSummary");
            RaiseUpdateManually("TaxValues");
            RaiseUpdateManually("OrderRebate");
            RaiseUpdateManually("CashBack");
            RaiseUpdateManually("NetArticleSummary");
            RaiseUpdateManually("NetOrderSummary");
            RaiseUpdateManually("OrderSummary");
            RaiseUpdateManually("NetShipment");
            RaiseUpdateManually("NetOrderRebate");
        }
    }
}
