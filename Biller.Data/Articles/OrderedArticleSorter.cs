using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.Data.Articles
{
    /// <summary>
    /// IComparer to order objects of the type <see cref="OrderedArticle"/> by their <see cref="OrderPosition"/>.
    /// </summary>
    class OrderedArticleSorter : System.Collections.Generic.IComparer<OrderedArticle>
    {
        /// <summary>
        /// Compares two objects and returns the difference between x's and y's <see cref="OrderPosition"/>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(OrderedArticle x, OrderedArticle y)
        {
            return x.OrderPosition - y.OrderPosition;
        }
    }
}
