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
using System.Reflection;

namespace TraktorTagger
{



    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindowViewModel ViewModel { get; private set; }

        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(MainWindow));


        public MainWindow()
        {

            InitializeComponent();

            ViewModel = new MainWindowViewModel();
            this.DataContext = ViewModel;
        }





        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {

            log.Debug("OnClosing() called..");

            //prompts user to save unsaved changes
            if(this.ViewModel.Collection != null && this.ViewModel.Collection.HasUnsavedChanges)
            {
                log.Debug("Active collection has unsaved changes. Prompting user...");

                var res = System.Windows.MessageBox.Show("There are unsaved changes made to the collection.\n\n Do you want to save these changes?", "Traktor Tagger", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                if(res == MessageBoxResult.Yes)
                {
                    log.Debug("Saving changes..");
                    this.ViewModel.Collection.SaveCollection();
                }
                else if(res == MessageBoxResult.Cancel)
                {
                    log.Debug("Canceling close..");
                    e.Cancel = true;
                }
                else if(res == MessageBoxResult.No)
                {
                    log.Debug("Closing with out saving changse...");
                    //just continue to close
                }
            }


            base.OnClosing(e);

        }

        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            foreach(DataGridColumn col in traktorColDataGrid.Columns)
            {
                col.SetValue(FrameworkElement.DataContextProperty, e.NewValue);
            }

            foreach(DataGridColumn col in searchResultDataGrid.Columns)
            {                
                col.SetValue(FrameworkElement.DataContextProperty, e.NewValue);

            }

        }









    }
}
