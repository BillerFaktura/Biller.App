using Biller.Data.Articles;
using Biller.Data.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Data.Orders
{
    public class OrderCalculation : Utils.PropertyChangedHelper
    {
        private Order _parentOrder;

        /// <summary>
        /// Default constructor for <see cref="OrderCalculation"/>.
        /// </summary>
        /// <param name="parentOrder">The calculations are based on the <see cref="Order"/> passed with the constructor.</param>
        public OrderCalculation(Order parentOrder)
        {
            _parentOrder = parentOrder;
            ArticleSummary = new EMoney(0, true, Currency.EUR);
            NetArticleSummary = new EMoney(0, false);
            TaxValues = new ObservableCollection<Models.TaxClassMoneyModel>();
            parentOrder.OrderedArticles.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(OnOrderedArticleCollectionChanged);
            parentOrder.OrderRebate.PropertyChanged += article_PropertyChanged;
            parentOrder.PaymentMethode.PropertyChanged += article_PropertyChanged;
            parentOrder.OrderShipment.PropertyChanged += article_PropertyChanged;
            parentOrder.PropertyChanged += parentOrder_PropertyChanged;
        }

        void parentOrder_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PaymentMethode")
            {
                _parentOrder.PaymentMethode.PropertyChanged += article_PropertyChanged;
                //CalculateValues();
            }
            if (e.PropertyName == "OrderShipment")
            {
                _parentOrder.OrderShipment.PropertyChanged += article_PropertyChanged;
                //CalculateValues();
            }
        }

        void OnOrderedArticleCollectionChanged(Object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                PropertyChangedHelper article = (PropertyChangedHelper)e.NewItems[0];
                article.PropertyChanged += article_PropertyChanged;
            }
            CalculateValues();
        }

        void article_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == "ArticleRecalculation")
                CalculateValues();
        }

        void CalculateValues()
        {
            TaxValues.Clear();
            ArticleSummary.Amount = 0;
            // iterate through each article and add its value
            foreach (Data.Articles.OrderedArticle article in _parentOrder.OrderedArticles)
            {
                ArticleSummary.Amount += article.RoundedGrossOrderValue.Amount;

                // taxclass listing
                if (TaxValues.Any(x => x.TaxClass == article.TaxClass))
                {
                    TaxValues.Single(x => x.TaxClass == article.TaxClass).Value += article.ExactVAT;
                }
                else
                {
                    TaxValues.Add(new Models.TaxClassMoneyModel() { Value = new Money(article.ExactVAT), TaxClass = article.TaxClass });
                }
            }
            
            // Discount
            if (_parentOrder.OrderRebate.Amount > 0)
            {
                var temp = ArticleSummary.Amount;
                ArticleSummary.Amount = ArticleSummary.Amount * (1 - _parentOrder.OrderRebate.Amount);
                OrderRebate = new EMoney(temp - ArticleSummary.Amount);
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
                // Just for Germany
                // We need to split the taxes with the ratio it is before
                // Austrian: Shipping has reduced taxes
                ArticleSummary.Amount += _parentOrder.OrderShipment.DefaultPrice.Amount;

                if (GlobalSettings.UseGermanSupplementaryTaxRegulation)
                {
                    var wholetax = 0.0;
                    foreach (var item in TaxValues)
                        wholetax += item.Value.Amount;

                    foreach (var item in TaxValues)
                    {
                        var ratio = 1 / (wholetax / item.Value.Amount);
                        var shipment = new OrderedArticle(new Article());
                        shipment.TaxClass = item.TaxClass; ;
                        shipment.OrderedAmount = 1;
                        shipment.OrderPrice.Price1 = _parentOrder.OrderShipment.DefaultPrice;
                        if (GlobalSettings.TaxSupplementaryWorkSeperate)
                        {
                            TaxValues.Add(new Models.TaxClassMoneyModel() { Value = new Money(ratio * shipment.ExactVAT), TaxClass = item.TaxClass, TaxClassAddition = GlobalSettings.LocalizedOnSupplementaryWork });
                        }
                        else
                        {
                            item.Value += (ratio * shipment.ExactVAT);
                        }
                    }
                }
            }

            // Skonto
            if (_parentOrder.PaymentMethode.Discount > 0)
            {
                CashBack = new EMoney(_parentOrder.PaymentMethode.Discount * ArticleSummary.Amount);
            }
            else
            {
                CashBack = new EMoney(0);
            }

            //NetArticleSummary
            NetArticleSummary = ArticleSummary;
            foreach (var item in TaxValues)
            {
                NetArticleSummary.Amount -= item.Value.Amount;
            }
            RaiseUpdateManually("ArticleSummary");
            RaiseUpdateManually("TaxValues");
            RaiseUpdateManually("OrderRebate");
            RaiseUpdateManually("CashBack");
        }

        /// <summary>
        /// With taxes
        /// </summary>
        public EMoney ArticleSummary { get { return GetValue(() => ArticleSummary); } set { SetValue(value); } }

        public EMoney NetArticleSummary { get { return GetValue(() => NetArticleSummary); } set { SetValue(value); } }

        /// <summary>
        /// Skonto
        /// </summary>
        public EMoney CashBack { get { return GetValue(() => CashBack); } set { SetValue(value); } }

        public EMoney OrderRebate { get { return GetValue(() => OrderRebate); } set { SetValue(value); } }

        public ObservableCollection<Models.TaxClassMoneyModel> TaxValues { get { return GetValue(() => TaxValues); } set { SetValue(value); } }

    }
}