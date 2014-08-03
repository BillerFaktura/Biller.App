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

            var editData = new NewCompany.EditCompanyViewModel(this, true);
            editData.BackstageTabItem.IsEnabled = false;
            ReceiveData(editData);

            var changeCompany = new ChangeCompany.ChangeCompanyViewModel(this);
            changeCompany.BackstageTabItem.IsEnabled = false;
            ReceiveData(changeCompany);
        }

        public async Task LoadData()
        {
            foreach (var item in BackstageItems)
            {
                await item.LoadData();
                item.BackstageTabItem.IsEnabled = true;
            }
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
