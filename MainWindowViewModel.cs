using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TracktorTagger
{
    public class MainWindowViewModel
    {

        public System.Collections.ObjectModel.ObservableCollection<TrackData> TrackDataSearchResults { get; private set; }
        public System.Collections.ObjectModel.ObservableCollection<TracktorTrack> TraktorTracks { get; private set; }


        public TracktorCollection Collection { get; private set; }

        public MainWindowViewModel()
        {
            this.TrackDataSearchResults = new System.Collections.ObjectModel.ObservableCollection<TrackData>();
            this.TraktorTracks = new System.Collections.ObjectModel.ObservableCollection<TracktorTrack>();




            this.OpenCollectionCommand = new RelayCommand(new Action<object>(this.OpenCollection));
            this.SaveCollectionCommand = new RelayCommand(new Action<object>(this.OpenCollection), new Predicate<object>(this.CanSaveCollection));


        }


        private void SaveCollection(object o)
        {
            if (this.Collection != null)
            {
                this.Collection.SaveCollection();
            }
        }

        private bool CanSaveCollection(object o)
        {

            if (this.Collection != null) return true;
            else return false;


        }




        private void OpenCollection(object o)
        {


            Microsoft.Win32.OpenFileDialog odiag = new Microsoft.Win32.OpenFileDialog();
            odiag.Filter = "Traktor Collection (*.nml)|*.nml";

            bool? res = odiag.ShowDialog();

            if (res.HasValue && res.Value)
            {
                Collection = new TracktorCollection(odiag.FileName);



                this.TraktorTracks.Clear();

                foreach (TracktorTrack t in Collection.Entries)
                {
                    this.TraktorTracks.Add(t);
                }



            }


        }



        public ICommand SaveCollectionCommand { get; private set; }

        public ICommand OpenCollectionCommand { get; private set; }


    }
}

