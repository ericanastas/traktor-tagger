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

            ITrackDataSource source = null;

            if (dataSourceCombo.SelectedIndex == 0)
            {


                source = new DiscogsTrackDataSource();
            }
            else if (dataSourceCombo.SelectedIndex == 1)
            {
                source = new BeatportTrackDataSource();
            }
            else
            {
                return;
            }








            searchResultDataGrid.ItemsSource = null;
            searchResultDataGrid.ItemsSource = source.SearchTracks(searchTextBox.Text);
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
            if (searchResultDataGrid.SelectedItem != null)
            {
                TrackData data = (TrackData)searchResultDataGrid.SelectedItem;

                if (!string.IsNullOrEmpty(data.URL))
                {



                }
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.MenuItem mi = (System.Windows.Controls.MenuItem)sender;



        }

        private void tagSelectedButton_Click(object sender, RoutedEventArgs e)
        {

            if (searchResultDataGrid.SelectedItem != null && traktorColDataGrid.SelectedItem != null)
            {

                TracktorTrack selTraktorTrack = (TracktorTrack)traktorColDataGrid.SelectedItem;
                TrackData selTrackData = (TrackData)searchResultDataGrid.SelectedItem;





                selTraktorTrack.Title = selTrackData.Title;
                selTraktorTrack.Mix = selTrackData.Mix;
                selTraktorTrack.Artist = selTrackData.Artist;

                selTraktorTrack.Remixer = selTrackData.Remixer;
                selTraktorTrack.Producer = selTrackData.Producer;
                selTraktorTrack.Release = selTrackData.Release;
                selTraktorTrack.ReleaseDate = selTrackData.ReleaseDate;
                selTraktorTrack.Label = selTrackData.Label;
                selTraktorTrack.Genre = selTrackData.Genre;
                selTraktorTrack.CatalogNumber = selTrackData.CatalogNumber;
                selTraktorTrack.Key = selTrackData.Key;


                selTraktorTrack.DataSourceTag = selTrackData.DataSourceTag;







            }

        }

        private void copyUrlMenu_Click(object sender, RoutedEventArgs e)
        {
            if (searchResultDataGrid.SelectedItem != null)
            {
                TrackData data = (TrackData)searchResultDataGrid.SelectedItem;
                System.Windows.Clipboard.SetText(data.URL);
            }

        }

        private void openPageMenu_Click(object sender, RoutedEventArgs e)
        {
            if (searchResultDataGrid.SelectedItem != null)
            {
                TrackData data = (TrackData)searchResultDataGrid.SelectedItem;
                System.Diagnostics.Process.Start(data.URL);
            }

        }
    }
}
