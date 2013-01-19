﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TraktorTagger
{
    public class MainWindowViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {


            this.TrackDataSearchResults = new System.Collections.ObjectModel.ObservableCollection<TrackData>();
            this.TraktorTracks = new System.Collections.ObjectModel.ObservableCollection<TraktorTrack>();

            this.TrackDataSources = new System.Collections.ObjectModel.ObservableCollection<ITrackDataSource>();



#if DEBUG
            this.TrackDataSources.Add(new PlaceHolderTrackDataSource());
#endif

            this.TrackDataSources.Add(new DiscogsTrackDataSource(Properties.Settings.Default.DiscogsReleasesPerPage, Properties.Settings.Default.DiscogsFormatFilter));
            this.TrackDataSources.Add(new BeatportTrackDataSource(Properties.Settings.Default.BeatportTracksPerPage));

            this.SelectedDataSource = this.TrackDataSources[0];

            //commands
            this.OpenCollectionCommand = new RelayCommand(new Action<object>(this.OpenCollection));
            this.SaveCollectionCommand = new RelayCommand(new Action<object>(this.SaveCollection), new Predicate<object>(this.CanSaveCollection));
            this.SearchTrackDataCommand = new RelayCommand(new Action<object>(this.SearchTrackData), new Predicate<object>(this.CanSearchTrackData));
            this.LoadMoreResultsCommand = new RelayCommand(new Action<object>(this.LoadMoreResults), new Predicate<object>(this.CanLoadMoreResults));
            this.TagSelectedCommand = new RelayCommand(new Action<object>(this.TagSelected), new Predicate<object>(this.CanTagSelected));
            this.CopyTrackDataUrlCommand = new RelayCommand(new Action<object>(this.CopyTrackDataUrl), new Predicate<object>(this.CanCopyTrackDataUrl));
            this.CopyTraktorTrackUrlCommand = new RelayCommand(new Action<object>(this.CopyTraktorTrackDataUrl), new Predicate<object>(this.CanCopyTraktorTrackDataUrl));
            this.ClearTraktorTrackDataSourceTagCommand = new RelayCommand(new Action<object>(this.ClearTraktorTrackDataSourceTag), new Predicate<object>(this.CanClearTraktorTrackDataSourceTag));
            this.DonateCommand = new RelayCommand(new Action<object>(this.Donate));
            this.OpenHelpCommand = new RelayCommand(new Action<object>(this.OpenHelp));
            this.AboutCommand = new RelayCommand(new Action<object>(this.About));

            this.BrowseToTrackDataURLCommand = new RelayCommand(new Action<object>(this.BrowseToTrackDataURL),new Predicate<object>(this.CanBrowseToTrackDataURL));
            this.SearchTraktorTrackDataSourceCommand = new RelayCommand(new Action<object>(this.SearchTraktorTrackDataSource), new Predicate<object>(this.CanSearchTraktorTrackDataSource));
            this.BrowseToTraktorTrackUrlCommand = new RelayCommand(new Action<object>(this.BrowseToTraktorTrackDataURL),new Predicate<object>(CanBrowseToTraktorTrackDataURL));






            this.TagDataSource = true;

            UpdateColumnCheckBoxes();


        }


        #region Properties

        public System.Collections.ObjectModel.ObservableCollection<TrackData> TrackDataSearchResults { get; private set; }
        public System.Collections.ObjectModel.ObservableCollection<TraktorTrack> TraktorTracks { get; private set; }
        public System.Collections.ObjectModel.ObservableCollection<ITrackDataSource> TrackDataSources { get; private set; }

        public TracktorCollection Collection { get; private set; }


        string _windowTitle;
        public string WindowTitle
        {
            get
            {
                if(string.IsNullOrEmpty(_windowTitle))
                {
                    var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

                    _windowTitle = "Traktor Tagger " + version.Major.ToString() + "." + version.Minor.ToString() + "." + version.Build.ToString();
                    return _windowTitle;

                }
                else
                {
                    return _windowTitle;
                }



            }
        }



        private bool _tagTitle;
        public bool TagTitle
        {
            get
            {
                return _tagTitle;
            }
            set
            {
                if(_tagTitle != value)
                {
                    _tagTitle = value;
                    RaisePropertyChanged("TagTitle");
                }
            }
        }


        private bool _tagMix;
        public bool TagMix
        {
            get
            {
                return _tagMix;
            }
            set
            {
                if(_tagMix != value)
                {
                    _tagMix = value;
                    RaisePropertyChanged("TagMix");
                }
            }
        }


        private bool _tagArtist;
        public bool TagArtist
        {
            get
            {
                return _tagArtist;
            }
            set
            {
                if(_tagArtist != value)
                {
                    _tagArtist = value;
                    RaisePropertyChanged("TagArtist");
                }
            }
        }


        private bool _tagRemixer;
        public bool TagRemixer
        {
            get
            {
                return _tagRemixer;
            }
            set
            {
                if(_tagRemixer != value)
                {
                    _tagRemixer = value;
                    RaisePropertyChanged("TagRemixer");
                }
            }
        }



        private bool _tagProducer;
        public bool TagProducer
        {
            get
            {
                return _tagProducer;
            }
            set
            {
                if(_tagProducer != value)
                {
                    _tagProducer = value;
                    RaisePropertyChanged("TagProducer");
                }
            }
        }


        private bool _tagRelease;
        public bool TagRelease
        {
            get
            {
                return _tagRelease;
            }
            set
            {
                if(_tagRelease != value)
                {
                    _tagRelease = value;
                    RaisePropertyChanged("TagRelease");
                }
            }
        }


        private bool _tagReleased;
        public bool TagReleased
        {
            get
            {
                return _tagReleased;
            }
            set
            {
                if(_tagReleased != value)
                {
                    _tagReleased = value;
                    RaisePropertyChanged("TagReleased");
                }
            }
        }


        private bool _tagLabel;
        public bool TagLabel
        {
            get
            {
                return _tagLabel;
            }
            set
            {
                if(_tagLabel != value)
                {
                    _tagLabel = value;
                    RaisePropertyChanged("TagLabel");
                }
            }
        }





        private bool _tagCatalogNo;
        public bool TagCatalogNo
        {
            get
            {
                return _tagCatalogNo;
            }
            set
            {
                if(_tagCatalogNo != value)
                {
                    _tagCatalogNo = value;
                    RaisePropertyChanged("TagCatalogNo");
                }
            }
        }


        private bool _tagGenre;
        public bool TagGenre
        {
            get
            {
                return _tagGenre;
            }
            set
            {
                if(_tagGenre != value)
                {
                    _tagGenre = value;
                    RaisePropertyChanged("TagGenre");
                }
            }
        }



        private bool _tagKey;
        public bool TagKey
        {
            get
            {
                return _tagKey;
            }
            set
            {
                if(_tagKey != value)
                {
                    _tagKey = value;
                    RaisePropertyChanged("TagKey");
                }
            }
        }




        private bool _tagDataSource;
        public bool TagDataSource
        {
            get
            {
                return _tagDataSource;
            }
            set
            {
                if(_tagDataSource != value)
                {
                    _tagDataSource = value;
                    RaisePropertyChanged("TagDataSource");
                }
            }
        }





        //**************

        private bool _canCanTagTitle;
        public bool CanTagTitle
        {
            get
            {
                return _canCanTagTitle;
            }
            set
            {
                if(_canCanTagTitle != value)
                {
                    _canCanTagTitle = value;
                    RaisePropertyChanged("CanTagTitle");
                }
            }
        }


        private bool _canCanTagMix;
        public bool CanTagMix
        {
            get
            {
                return _canCanTagMix;
            }
            set
            {
                if(_canCanTagMix != value)
                {
                    _canCanTagMix = value;
                    RaisePropertyChanged("CanTagMix");
                }
            }
        }


        private bool _canCanTagArtist;
        public bool CanTagArtist
        {
            get
            {
                return _canCanTagArtist;
            }
            set
            {
                if(_canCanTagArtist != value)
                {
                    _canCanTagArtist = value;
                    RaisePropertyChanged("CanTagArtist");
                }
            }
        }


        private bool _canCanTagRemixer;
        public bool CanTagRemixer
        {
            get
            {
                return _canCanTagRemixer;
            }
            set
            {
                if(_canCanTagRemixer != value)
                {
                    _canCanTagRemixer = value;
                    RaisePropertyChanged("CanTagRemixer");
                }
            }
        }



        private bool _canCanTagProducer;
        public bool CanTagProducer
        {
            get
            {
                return _canCanTagProducer;
            }
            set
            {
                if(_canCanTagProducer != value)
                {
                    _canCanTagProducer = value;
                    RaisePropertyChanged("CanTagProducer");
                }
            }
        }


        private bool _canCanTagRelease;
        public bool CanTagRelease
        {
            get
            {
                return _canCanTagRelease;
            }
            set
            {
                if(_canCanTagRelease != value)
                {
                    _canCanTagRelease = value;
                    RaisePropertyChanged("CanTagRelease");
                }
            }
        }



        private bool _canCanTagReleased;
        public bool CanTagReleased
        {
            get
            {
                return _canCanTagReleased;
            }
            set
            {
                if(_canCanTagReleased != value)
                {
                    _canCanTagReleased = value;
                    RaisePropertyChanged("CanTagReleased");
                }
            }
        }


        private bool _canCanTagLabel;
        public bool CanTagLabel
        {
            get
            {
                return _canCanTagLabel;
            }
            set
            {
                if(_canCanTagLabel != value)
                {
                    _canCanTagLabel = value;
                    RaisePropertyChanged("CanTagLabel");
                }
            }
        }





        private bool _canCanTagCatalogNo;
        public bool CanTagCatalogNo
        {
            get
            {
                return _canCanTagCatalogNo;
            }
            set
            {
                if(_canCanTagCatalogNo != value)
                {
                    _canCanTagCatalogNo = value;
                    RaisePropertyChanged("CanTagCatalogNo");
                }
            }
        }


        private bool _canCanTagGenre;
        public bool CanTagGenre
        {
            get
            {
                return _canCanTagGenre;
            }
            set
            {
                if(_canCanTagGenre != value)
                {
                    _canCanTagGenre = value;
                    RaisePropertyChanged("CanTagGenre");
                }
            }
        }



        private bool _canCanTagKey;
        public bool CanTagKey
        {
            get
            {
                return _canCanTagKey;
            }
            set
            {
                if(_canCanTagKey != value)
                {
                    _canCanTagKey = value;
                    RaisePropertyChanged("CanTagKey");
                }
            }
        }


        //***************








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

                    UpdateColumnCheckBoxes();

                    RaisePropertyChanged("SelectedDataSource");
                }
            }
        }

        private void UpdateColumnCheckBoxes()
        {
            if(SelectedDataSource != null)
            {

                TagTitle = SelectedDataSource.ProvidesTitle;
                TagMix = SelectedDataSource.ProvidesMix;
                TagArtist = SelectedDataSource.ProvidesArtist;
                TagRemixer = SelectedDataSource.ProvidesRemixer;
                TagProducer = SelectedDataSource.ProvidesProducer;
                TagRelease = SelectedDataSource.ProvidesRelease;
                TagReleased = SelectedDataSource.ProvidesReleaseDate;
                TagLabel = SelectedDataSource.ProvidesLabel;
                TagCatalogNo = SelectedDataSource.ProvidesCatalogNo;
                TagGenre = SelectedDataSource.ProvidesGenre;
                TagKey = SelectedDataSource.ProvidesKey;




                //Update enable status of check boxes
                CanTagTitle = SelectedDataSource.ProvidesTitle;
                CanTagMix = SelectedDataSource.ProvidesMix;
                CanTagArtist = SelectedDataSource.ProvidesArtist;
                CanTagRemixer = SelectedDataSource.ProvidesRemixer;
                CanTagProducer = SelectedDataSource.ProvidesProducer;
                CanTagRelease = SelectedDataSource.ProvidesRelease;
                CanTagReleased = SelectedDataSource.ProvidesReleaseDate;
                CanTagLabel = SelectedDataSource.ProvidesLabel;
                CanTagCatalogNo = SelectedDataSource.ProvidesCatalogNo;
                CanTagGenre = SelectedDataSource.ProvidesGenre;
                CanTagKey = SelectedDataSource.ProvidesKey;

            }
            else
            {

                TagTitle = false;
                TagMix = false;
                TagArtist = false;
                TagRemixer = false;
                TagProducer = false;
                TagRelease = false;
                TagReleased = false;
                TagLabel = false;
                TagCatalogNo = false;
                TagGenre = false;
                TagKey = false;

                CanTagTitle = false;
                CanTagMix = false;
                CanTagArtist = false;
                CanTagRemixer = false;
                CanTagProducer = false;
                CanTagRelease = false;
                CanTagReleased = false;
                CanTagLabel = false;
                CanTagCatalogNo = false;
                CanTagGenre = false;
                CanTagKey = false;


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

        public string LastTrackDataSearchText { get; set; }

        private bool CanSearchTrackData(object o)
        {
            if(!string.IsNullOrEmpty(TrackDataSearchText)) return true;
            else return false;
        }


        private string _searchStatus;
        public string SearchStatus
        {
            get
            {
                return _searchStatus;
            }
            set
            {
                if(_searchStatus != value)
                {
                    _searchStatus = value;

                    RaisePropertyChanged("SearchStatus");
                }
            }
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

        private TraktorTrack _selectedTraktorTrack;
        public TraktorTrack SelectedTraktorTrack
        {
            get
            {
                return _selectedTraktorTrack;
            }
            set
            {
                if(_selectedTraktorTrack != value)
                {
                    _selectedTraktorTrack = value;

                    RaisePropertyChanged("SelectedTraktorTrack");
                }
            }
        }


        #endregion



        #region Methods




        private void ClearTraktorTrackDataSourceTag(object o)
        {
            this.SelectedTraktorTrack.DataSourceUri = null;
        }

        private bool CanClearTraktorTrackDataSourceTag(object o)
        {
            if(SelectedTraktorTrack != null && SelectedTraktorTrack.DataSourceUri != null) return true;
            else return false;
        }

        private void CopyTraktorTrackDataUrl(object o)
        {
            System.Windows.Clipboard.SetText(SelectedTraktorTrack.DataSourceUri.AbsoluteUri);
        }


        private bool CanCopyTraktorTrackDataUrl(object o)
        {
            if(SelectedTraktorTrack != null && SelectedTraktorTrack.DataSourceUri != null) return true;
            else return false;
        }


        private void CopyTrackDataUrl(object o)
        {
            System.Windows.Clipboard.SetText(SelectedTrackData.URL.AbsoluteUri);
        }


        private bool CanCopyTrackDataUrl(object o)
        {
            if(SelectedTrackData != null) return true;
            else return false;
        }

        private void TagSelected(object o)
        {
            if(TagDataSource)
            {
                SelectedTraktorTrack.DataSourceUri = SelectedTrackData.URL;
            }

            if(TagTitle && !string.IsNullOrEmpty(SelectedTrackData.Title))
            {
                SelectedTraktorTrack.Title = SelectedTrackData.Title;
            }

            if(TagMix && !string.IsNullOrEmpty(SelectedTrackData.Mix))
            {
                SelectedTraktorTrack.Mix = SelectedTrackData.Mix;
            }

            if(TagArtist && !string.IsNullOrEmpty(SelectedTrackData.Artist))
            {
                SelectedTraktorTrack.Artist = SelectedTrackData.Artist;
            }

            if(TagRemixer && !string.IsNullOrEmpty(SelectedTrackData.Remixer))
            {
                SelectedTraktorTrack.Remixer = SelectedTrackData.Remixer;
            }

            if(TagProducer && !string.IsNullOrEmpty(SelectedTrackData.Producer))
            {
                SelectedTraktorTrack.Producer = SelectedTrackData.Producer;
            }

            if(TagRelease && !string.IsNullOrEmpty(SelectedTrackData.Release))
            {
                SelectedTraktorTrack.Release = SelectedTrackData.Release;
            }

            if(TagReleased && SelectedTrackData.ReleaseDate.HasValue)
            {
                SelectedTraktorTrack.ReleaseDate = SelectedTrackData.ReleaseDate;
            }

            if(TagLabel && !string.IsNullOrEmpty(SelectedTrackData.Label))
            {
                SelectedTraktorTrack.Label = SelectedTrackData.Label;
            }


            if(TagCatalogNo && !string.IsNullOrEmpty(SelectedTrackData.CatalogNumber))
            {
                SelectedTraktorTrack.CatalogNumber = SelectedTrackData.CatalogNumber;
            }


            if(TagGenre && !string.IsNullOrEmpty(SelectedTrackData.Genre))
            {
                SelectedTraktorTrack.Genre = SelectedTrackData.Genre;
            }

            if(TagKey && SelectedTrackData.Key != null)
            {
                SelectedTraktorTrack.Key = SelectedTrackData.Key;
            }
        }

        private void OpenHelp(object o)
        {
            System.Diagnostics.Process.Start(Properties.Settings.Default.HelpURL);
        }


        private void About(object o)
        {
            System.Diagnostics.Process.Start(Properties.Settings.Default.AboutURL);
        }

        private void Donate(object o)
        {
            System.Diagnostics.Process.Start(Properties.Settings.Default.DonateURL);        
        }

        private bool CanTagSelected(object o)
        {
            if(SelectedTrackData != null && SelectedTraktorTrack != null) return true;
            else return false;
        }

        private bool CanLoadMoreResults(object o)
        {
            if(_currentSearch != null)
            {
                return _currentSearch.HasMoreResults;
            }
            else return false;
        }

        private void LoadMoreResults(object o)
        {
            var res = _currentSearch.LoadMoreResults();

            foreach(var r in res)
            {
                TrackDataSearchResults.Add(r);
            }

            UpdateSearchStatus();
        }

        private void UpdateSearchStatus()
        {
            string message = _currentSearch.Results.Count.ToString() + " tracks found for \"" + _currentSearch.SearchQuery + "\"";

            if(_currentSearch.HasMoreResults)
            {
                message = message + " (More results available)";
            }
            else
            {
                message = message + " (All results loaded)";
            }


            this.SearchStatus = message;
        }

        private ITrackDataSearch _currentSearch;

        private void SearchTrackData(object o)
        {
            this.TrackDataSearchResults.Clear();


            if(Uri.IsWellFormedUriString(this.TrackDataSearchText,UriKind.Absolute))
            {

                Uri searchUri = new Uri(this.TrackDataSearchText);

                foreach(var dataSource in TrackDataSources)
                {

                    if(dataSource.HostName == searchUri.Host)
                    {
                        SelectedDataSource = dataSource;

                        _currentSearch = this.SelectedDataSource.GetTrackDataSearch(searchUri);

                        break;
                    }
                }
            }
            else
            {
                _currentSearch = this.SelectedDataSource.GetTrackDataSearch(this.TrackDataSearchText);
            }








            foreach(var r in _currentSearch.Results)
            {
                TrackDataSearchResults.Add(r);
            }

            UpdateSearchStatus();
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

                foreach(TraktorTrack t in Collection.Entries)
                {
                    this.TraktorTracks.Add(t);
                }
            }
        }



        private void BrowseToTraktorTrackDataURL(object o)
        {
            System.Diagnostics.Process.Start(SelectedTraktorTrack.DataSourceUri.AbsoluteUri);
        }

        private void BrowseToTrackDataURL(object o)
        {
            System.Diagnostics.Process.Start(SelectedTrackData.URL.AbsoluteUri);
        }


        private void SearchTraktorTrackDataSource(object o)
        {
            this.TrackDataSearchText = SelectedTraktorTrack.DataSourceUri.AbsoluteUri;
            SearchTrackDataCommand.Execute(null);
        }



        private bool CanBrowseToTraktorTrackDataURL(object o)
        {
            if(SelectedTraktorTrack != null && SelectedTraktorTrack.DataSourceUri != null) return true;
            else return false;
        }

        private bool CanBrowseToTrackDataURL(object o)
        {
            if(this.SelectedTrackData != null) return true;
            else return false;
        }


        private bool CanSearchTraktorTrackDataSource(object o)
        {
            if(SelectedTraktorTrack != null && SelectedTraktorTrack.DataSourceUri != null) return true;
            else return false;
        }


        #endregion

        #region ICommands

        public ICommand TagSelectedCommand { get; private set; }
        public ICommand LoadMoreResultsCommand { get; private set; }
        public ICommand SaveCollectionCommand { get; private set; }
        public ICommand OpenCollectionCommand { get; private set; }
        public ICommand SearchTrackDataCommand { get; private set; }
        public ICommand CopyTrackDataUrlCommand { get; private set; }
        public ICommand CopyTraktorTrackUrlCommand { get; private set; }
        public ICommand ClearTraktorTrackDataSourceTagCommand { get; private set; }
        public ICommand DonateCommand { get; private set; }
        public ICommand OpenHelpCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }

        public ICommand SearchTraktorTrackDataSourceCommand { get; private set; }
        public ICommand BrowseToTraktorTrackUrlCommand { get; private set; }
        public ICommand BrowseToTrackDataURLCommand { get; private set; }



        #endregion


        #region INotifyProperty Changed
        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if(handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}


