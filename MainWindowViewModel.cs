using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TracktorTagger
{
    public class MainWindowViewModel : System.ComponentModel.INotifyPropertyChanged
    {

        public System.Collections.ObjectModel.ObservableCollection<TrackData> TrackDataSearchResults { get; private set; }
        public System.Collections.ObjectModel.ObservableCollection<TracktorTrack> TraktorTracks { get; private set; }


        public System.Collections.ObjectModel.ObservableCollection<ITrackDataSource> TrackDataSources { get; private set; }


        public TracktorCollection Collection { get; private set; }

        private ITrackDataSource _selectedDataSource;
        public ITrackDataSource SelectedDataSource 
        {
            get
            {
                return _selectedDataSource;
            }
            set
            {

                if(_selectedDataSource != value)
                {
                    _selectedDataSource = value;
                    RaisePropertyChanged("SelectedDataSource");
                
                }
            
            }
        }



        private string _trackDataSearchText;
        public string TrackDataSearchText
        {
            get
            {
                return _trackDataSearchText;
            }
            set
            {

                if(_trackDataSearchText != value)
                {
                    _trackDataSearchText = value;
                    RaisePropertyChanged("TrackDataSearchText");

                }

            }

        }

        public MainWindowViewModel()
        {
            this.TrackDataSearchResults = new System.Collections.ObjectModel.ObservableCollection<TrackData>();
            this.TraktorTracks = new System.Collections.ObjectModel.ObservableCollection<TracktorTrack>();

            this.TrackDataSources = new System.Collections.ObjectModel.ObservableCollection<ITrackDataSource>();

            this.TrackDataSources.Add(new PlaceHolderTrackDataSource(50));
            this.TrackDataSources.Add(new BeatportTrackDataSource());
            this.TrackDataSources.Add(new DiscogsTrackDataSource());


            this.SelectedDataSource = this.TrackDataSources[0];


            //commands
            this.OpenCollectionCommand = new RelayCommand(new Action<object>(this.OpenCollection));
            this.SaveCollectionCommand = new RelayCommand(new Action<object>(this.OpenCollection), new Predicate<object>(this.CanSaveCollection));
            this.SearchTrackDataCommand = new RelayCommand(new Action<object>(this.SearchTrackData), new Predicate<object>(this.CanSearchTrackData));


            


        }


        private bool CanSearchTrackData(object o)
        {
            if(!string.IsNullOrEmpty(TrackDataSearchText)) return true;
            else return false;
        }

        private TrackData _selectedTrackData;
        public TrackData SelectedTrackData 
        {
            get
            {
                return _selectedTrackData;
            }
            set
            {

                if(_selectedTrackData != value)
                {
                    _selectedTrackData = value;

                    RaisePropertyChanged("SelectedTrackData");    
                }
            
            }
        }

        private void SearchTrackData(object o)
        {

            TrackDataSearchResults.Clear();

            var trackData = this.SelectedDataSource.SearchTracks(this.TrackDataSearchText);


            foreach(var t in trackData) 
            {
                TrackDataSearchResults.Add(t);
            }

            

        }


        private void SaveCollection(object o)
        {
            if(this.Collection != null)
            {
                this.Collection.SaveCollection();
            }
        }

        private bool CanSaveCollection(object o)
        {

            if(this.Collection != null) return true;
            else return false;


        }




        private void OpenCollection(object o)
        {


            Microsoft.Win32.OpenFileDialog odiag = new Microsoft.Win32.OpenFileDialog();
            odiag.Filter = "Traktor Collection (*.nml)|*.nml";

            bool? res = odiag.ShowDialog();

            if(res.HasValue && res.Value)
            {
                Collection = new TracktorCollection(odiag.FileName);



                this.TraktorTracks.Clear();

                foreach(TracktorTrack t in Collection.Entries)
                {
                    this.TraktorTracks.Add(t);
                }



            }


        }



        public ICommand SaveCollectionCommand { get; private set; }

        public ICommand OpenCollectionCommand { get; private set; }


        public ICommand SearchTrackDataCommand { get; private set; }


        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if(handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

