using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import_Biller
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
            get { return "ImportModules@Biller"; }
        }

        public string Description
        {
            get { return "Implementiert Module zum importieren von externen Daten"; }
        }

        public double Version
        {
            get { return 0.1; }
        }

        public void Activate()
        {
            ParentViewModel.SettingsTabViewModel.SettingsList.Add(new UI.Tab() { DataContext = this });
        }

        private List<Biller.UI.Interface.IViewModel> internalViewModels;
        public List<Biller.UI.Interface.IViewModel> ViewModels()
        {
            return internalViewModels;
        }
    }
}
