using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTypes_Biller
{
    public class EntryPoint : Biller.UI.Interface.IPlugIn
    {
        public EntryPoint(Biller.UI.ViewModel.MainWindowViewModel parentViewModel)
        {
            this.ParentViewModel = parentViewModel;
            internalViewModels = new List<Biller.UI.Interface.IViewModel>();
        }

        public Biller.UI.ViewModel.MainWindowViewModel ParentViewModel { get; private set; }

        public string Name
        {
            get { return "Ordertypes @ Biller"; }
        }

        public string Description
        {
            get { return "Implements different order types to save and load from database"; }
        }

        public double Version
        {
            get { return 0.1; }
        }

        public async void Activate()
        {
            await ParentViewModel.Database.AddAdditionalPreviewDocumentParser(new Docket.DocketParser());
            ParentViewModel.OrderTabViewModel.AddDocumentFactory(new Docket.DocketFactory());
        }

        public List<Biller.UI.Interface.IViewModel> ViewModels()
        {
            return internalViewModels;
        }

        private List<Biller.UI.Interface.IViewModel> internalViewModels { get; set; }
    }
}
