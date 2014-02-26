using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.UI.Backstage
{
    public class BackstageViewModel : Data.Utils.PropertyChangedHelper, Interface.IViewModel
    {
        public UI.ViewModel.MainWindowViewModel parentViewModel { get; private set; }

        public BackstageViewModel(UI.ViewModel.MainWindowViewModel parent)
        {
            parentViewModel = parent;
            BackstageItems = new ObservableCollection<Interface.IBackstageContentViewModel>();
            ReceiveData(new NewCompany.NewCompanyViewModel(this));
        }

        public Task LoadData()
        {
            throw new NotImplementedException();
        }

        public void ReceiveData(object data)
        {
            if (data is Interface.IBackstageContentViewModel)
            {
                BackstageItems.Add(data as Interface.IBackstageContentViewModel);
                parentViewModel.RibbonFactory.AddBackStageTabItem((data as Interface.IBackstageContentViewModel).BackstageTabItem);
            }
        }

        public ObservableCollection<Interface.IBackstageContentViewModel> BackstageItems { get { return GetValue(() => BackstageItems); } set { SetValue(value); } }
    }
}
