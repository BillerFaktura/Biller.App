using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biller.UI.Backstage.NewCompany
{
    public class EditCompanyViewModel : Core.Utils.PropertyChangedHelper, Interface.IBackstageContentViewModel
    {
        /// <summary>
        /// Creates a new Viewmodel.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="EditMode">If set to to true, the fields contains values from the current company</param>
        public EditCompanyViewModel(BackstageViewModel parent, bool EditMode)
        {
            ParentViewModel = parent;
            if (EditMode)
                BackstageTabItem = new EditContentTabItem() { DataContext = this };
            else
                BackstageTabItem = new ContentTabItem() { DataContext = this };
            this.EditMode = EditMode;
            CompanyInformation = new Core.Models.CompanyInformation();
            CompanySettings = new Core.Models.CompanySettings();
        }

        public bool EditMode { get { return GetValue(() => EditMode); } private set { SetValue(value); } }

        public Fluent.BackstageTabItem BackstageTabItem { get; private set; }

        public BackstageViewModel ParentViewModel { get; private set; }

        public async Task LoadData()
        {
            if (EditMode)
            {
                CompanyInformation = ParentViewModel.ParentViewModel.Database.CurrentCompany;
                CompanySettings = (Biller.Core.Models.CompanySettings)(await ParentViewModel.ParentViewModel.Database.AllStorageableItems(new Biller.Core.Models.CompanySettings())).FirstOrDefault();
                if (CompanySettings == null)
                    CompanySettings = new Biller.Core.Models.CompanySettings();
            }
            else
            {
                CompanyInformation = new Core.Models.CompanyInformation();
                CompanySettings = new Core.Models.CompanySettings();
            }
        }

        public void ReceiveData(object data)
        {
            // Do nothing
        }

        public async void ResetCompanyInformation()
        {
            await LoadData();
        }

        public Biller.Core.Models.CompanyInformation CompanyInformation { get { return GetValue(() => CompanyInformation); } private set { SetValue(value); } }

        public Biller.Core.Models.CompanySettings CompanySettings { get { return GetValue(() => CompanySettings); } private set { SetValue(value); } }
    }
}
