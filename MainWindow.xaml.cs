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

namespace TracktorTagger
{



    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainWindowViewModel();
        }


 

        private void openPageMenu_Click(object sender, RoutedEventArgs e)
        {
            if (searchResultDataGrid.SelectedItem != null)
            {
                TrackData data = (TrackData)searchResultDataGrid.SelectedItem;
                System.Diagnostics.Process.Start(data.URL.AbsoluteUri);
            }

        }


    }
}
