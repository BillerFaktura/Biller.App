using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.UI.Interface
{
    /// <summary>
    /// Use the <see cref="IEditObserver"/> to hook up with events of the user, like editing and saving <see cref="Article"/>s, <see cref="Customer"/>s and <see cref="Document"/>s.
    /// </summary>
    public interface IEditObserver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="articleEditViewModel"></param>
        void ReceiveArticleEditViewModel(Biller.UI.ArticleView.Contextual.ArticleEditViewModel articleEditViewModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerEditViewModel"></param>
        void ReceiveCustomerEditViewModel(Biller.UI.CustomerView.Contextual.CustomerEditViewModel customerEditViewModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="documentEditViewModel"></param>
        void ReceiveDocumentEditViewModel(Biller.UI.DocumentView.Contextual.DocumentEditViewModel documentEditViewModel);
    }
}
