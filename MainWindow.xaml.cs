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


            TracktorCollection col = new TracktorCollection(@"C:\Users\Eric\Desktop\TraktorTagger\collection.nml");


            
                      
            traktorColDataGrid.ItemsSource = col.Entries;
            








            
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            BeatPortTrackDataProvider prov = new BeatPortTrackDataProvider();

            searchResultDataGrid.ItemsSource = null;
            searchResultDataGrid.ItemsSource = prov.GetTracks(searchTextBox.Text);
        }
    }
}
