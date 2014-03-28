﻿using MigraDoc.DocumentObjectModel.IO;
using System;
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

namespace Biller.UI.DocumentView.Contextual.EditTabs.PrintPreview
{
    /// <summary>
    /// Interaktionslogik für Content.xaml
    /// </summary>
    public partial class Content : UserControl
    {
        public Content()
        {
            InitializeComponent();
        }

        private void UpdatePreview()
        {
            var viewmodel = (DataContext as DocumentEditViewModel);
            viewmodel.ExportClass.RenderDocumentPreview(viewmodel.Document);
        }

        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            UpdatePreview();
        }
    }
}