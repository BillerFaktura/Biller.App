using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.UI.Backstage
{
    public class BackstageViewModel : Core.Utils.PropertyChangedHelper, Interface.IViewModel
    {
        public UI.ViewModel.MainWindowViewModel ParentViewModel { get; private set; }

        public BackstageViewModel(UI.ViewModel.MainWindowViewModel parent)
        {
            ParentViewModel = parent;
            BackstageItems = new ObservableCollection<Interface.IBackstageContentViewModel>();
            ReceiveData(new NewCompany.EditCompanyViewModel(this, false));
           
        }

        public async Task LoadData()
        {
            ReceiveData(new NewCompany.EditCompanyViewModel(this, true));
            ReceiveData(new ChangeCompany.ChangeCompanyViewModel(this));
            foreach (var item in BackstageItems)
                await item.LoadData();
        }

        public void ReceiveData(object data)
        {
            if (data is Interface.IBackstageContentViewModel)
            {
                BackstageItems.Add(data as Interface.IBackstageContentViewModel);
                ParentViewModel.RibbonFactory.AddBackStageTabItem((data as Interface.IBackstageContentViewModel).BackstageTabItem);
            }
        }

        public ObservableCollection<Interface.IBackstageContentViewModel> BackstageItems { get { return GetValue(() => BackstageItems); } set { SetValue(value); } }
    }
}
