﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Biller.UI.Backstage.NewCompany
{
    /// <summary>
    /// UI Control for the basic data of a new company
    /// </summary>
    public partial class Content : UserControl
    {
        public Content()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Office2013Button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as NewCompanyViewModel).BackstageTabItem.Focus(); //For MVVM //
            (DataContext as NewCompanyViewModel).CompanyInformation.GenerateNewID();
            (DataContext as NewCompanyViewModel).ParentViewModel.parentViewModel.Database.AddCompany((DataContext as NewCompanyViewModel).CompanyInformation);
            await (DataContext as NewCompanyViewModel).ParentViewModel.parentViewModel.Database.ChangeCompany((DataContext as NewCompanyViewModel).CompanyInformation);
            
        }
    }
}
