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
        TracktorCollection collection;





        public MainWindow()
        {
            InitializeComponent();


  

            
        }



        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            BeatPortTrackDataProvider prov = new BeatPortTrackDataProvider();

            searchResultDataGrid.ItemsSource = null;
            searchResultDataGrid.ItemsSource = prov.SearchTracks(searchTextBox.Text);
        }

        private void openMenu_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog odiag = new Microsoft.Win32.OpenFileDialog();
            odiag.Filter = "Traktor Collection (*.nml)|*.nml";

            bool? res = odiag.ShowDialog();

            if (res.HasValue && res.Value)
            {
                
                collection = new TracktorCollection(odiag.FileName);

                this.traktorColDataGrid.BeginInit();


                this.traktorColDataGrid.ItemsSource = null;
                this.traktorColDataGrid.ItemsSource = collection.Entries;

                this.traktorColDataGrid.EndInit();
            

            
            }

        }

        private void saveMenu_Click(object sender, RoutedEventArgs e)
        {
            if (collection != null)
            {
                collection.SaveCollection();
            }

        }

        private void searchResultDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (searchResultDataGrid.SelectedItem !=null)
            {
                TrackData data = (TrackData)searchResultDataGrid.SelectedItem;

                if (!string.IsNullOrEmpty(data.URL))
                {


                    
                }            
            }
        }
    }
}
