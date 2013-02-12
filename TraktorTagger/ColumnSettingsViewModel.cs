using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TraktorTagger
{
    public class ColumnSettingsViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        #region Track Columns


        private bool _ShowTrackTitleColumn;
        public bool ShowTrackTitleColumn
        {
            get
            {
                return _ShowTrackTitleColumn;
            }
            set
            {
                if(_ShowTrackTitleColumn != value)
                {
                    _ShowTrackTitleColumn = value;
                    RaisePropertyChanged("ShowTrackTitleColumn");
                }
            }
        }


        private bool _ShowTrackMixColumn;
        public bool ShowTrackMixColumn
        {
            get
            {
                return _ShowTrackMixColumn;
            }
            set
            {
                if(_ShowTrackMixColumn != value)
                {
                    _ShowTrackMixColumn = value;
                    RaisePropertyChanged("ShowTrackMixColumn");
                }
            }
        }



        private bool _ShowTrackArtistColumn;
        public bool ShowTrackArtistColumn
        {
            get
            {
                return _ShowTrackArtistColumn;
            }
            set
            {
                if(_ShowTrackArtistColumn != value)
                {
                    _ShowTrackArtistColumn = value;
                    RaisePropertyChanged("ShowTrackArtistColumn");
                }
            }
        }


        private bool _ShowTrackRemixerColumn;
        public bool ShowTrackRemixerColumn
        {
            get
            {
                return _ShowTrackRemixerColumn;
            }
            set
            {
                if(_ShowTrackRemixerColumn != value)
                {
                    _ShowTrackRemixerColumn = value;
                    RaisePropertyChanged("ShowTrackRemixerColumn");
                }
            }
        }


        private bool _ShowTrackProducerColumn;
        public bool ShowTrackProducerColumn
        {
            get
            {
                return _ShowTrackProducerColumn;
            }
            set
            {
                if(_ShowTrackProducerColumn != value)
                {
                    _ShowTrackProducerColumn = value;
                    RaisePropertyChanged("ShowTrackProducerColumn");
                }
            }
        }


        private bool _ShowTrackReleaseColumn;
        public bool ShowTrackReleaseColumn
        {
            get
            {
                return _ShowTrackReleaseColumn;
            }
            set
            {
                if(_ShowTrackReleaseColumn != value)
                {
                    _ShowTrackReleaseColumn = value;
                    RaisePropertyChanged("ShowTrackReleaseColumn");
                }
            }
        }


        private bool _ShowTrackReleasedColumn;
        public bool ShowTrackReleasedColumn
        {
            get
            {
                return _ShowTrackReleasedColumn;
            }
            set
            {
                if(_ShowTrackReleasedColumn != value)
                {
                    _ShowTrackReleasedColumn = value;
                    RaisePropertyChanged("ShowTrackReleasedColumn");
                }
            }
        }


        private bool _ShowTrackLabelColumn;
        public bool ShowTrackLabelColumn
        {
            get
            {
                return _ShowTrackLabelColumn;
            }
            set
            {
                if(_ShowTrackLabelColumn != value)
                {
                    _ShowTrackLabelColumn = value;
                    RaisePropertyChanged("ShowTrackLabelColumn");
                }
            }
        }


        private bool _ShowTrackCatalogNoColumn;
        public bool ShowTrackCatalogNoColumn
        {
            get
            {
                return _ShowTrackCatalogNoColumn;
            }
            set
            {
                if(_ShowTrackCatalogNoColumn != value)
                {
                    _ShowTrackCatalogNoColumn = value;
                    RaisePropertyChanged("ShowTrackCatalogNoColumn");
                }
            }
        }


        private bool _ShowTrackGenreColumn;
        public bool ShowTrackGenreColumn
        {
            get
            {
                return _ShowTrackGenreColumn;
            }
            set
            {
                if(_ShowTrackGenreColumn != value)
                {
                    _ShowTrackGenreColumn = value;
                    RaisePropertyChanged("ShowTrackGenreColumn");
                }
            }
        }


        private bool _ShowTrackKeyColumn;
        public bool ShowTrackKeyColumn
        {
            get
            {
                return _ShowTrackKeyColumn;
            }
            set
            {
                if(_ShowTrackKeyColumn != value)
                {
                    _ShowTrackKeyColumn = value;
                    RaisePropertyChanged("ShowTrackKeyColumn");
                }
            }
        }


        private bool _ShowTrackBPMColumn;
        public bool ShowTrackBPMColumn
        {
            get
            {
                return _ShowTrackBPMColumn;
            }
            set
            {
                if(_ShowTrackBPMColumn != value)
                {
                    _ShowTrackBPMColumn = value;
                    RaisePropertyChanged("ShowTrackBPMColumn");
                }
            }
        }



        private bool _ShowTrackRatingColumn;
        public bool ShowTrackRatingColumn
        {
            get
            {
                return _ShowTrackRatingColumn;
            }
            set
            {
                if(_ShowTrackRatingColumn != value)
                {
                    _ShowTrackRatingColumn = value;
                    RaisePropertyChanged("ShowTrackRatingColumn");
                }
            }
        }



        private bool _ShowTrackImportedColumn;
        public bool ShowTrackImportedColumn
        {
            get
            {
                return _ShowTrackImportedColumn;
            }
            set
            {
                if(_ShowTrackImportedColumn != value)
                {
                    _ShowTrackImportedColumn = value;
                    RaisePropertyChanged("ShowTrackImportedColumn");
                }
            }
        }


        private bool _ShowTrackModifiedColumn;
        public bool ShowTrackModifiedColumn
        {
            get
            {
                return _ShowTrackModifiedColumn;
            }
            set
            {
                if(_ShowTrackModifiedColumn != value)
                {
                    _ShowTrackModifiedColumn = value;
                    RaisePropertyChanged("ShowTrackModifiedColumn");
                }
            }
        }


        private bool _ShowTrackLastPlayedColumn;
        public bool ShowTrackLastPlayedColumn
        {
            get
            {
                return _ShowTrackLastPlayedColumn;
            }
            set
            {
                if(_ShowTrackLastPlayedColumn != value)
                {
                    _ShowTrackLastPlayedColumn = value;
                    RaisePropertyChanged("ShowTrackLastPlayedColumn");
                }
            }
        }


        private bool _ShowTrackPlayCountColumn;
        public bool ShowTrackPlayCountColumn
        {
            get
            {
                return _ShowTrackPlayCountColumn;
            }
            set
            {
                if(_ShowTrackPlayCountColumn != value)
                {
                    _ShowTrackPlayCountColumn = value;
                    RaisePropertyChanged("ShowTrackPlayCountColumn");
                }
            }
        }


        private bool _ShowTrackPlayTimeColumn;
        public bool ShowTrackPlayTimeColumn
        {
            get
            {
                return _ShowTrackPlayTimeColumn;
            }
            set
            {
                if(_ShowTrackPlayTimeColumn != value)
                {
                    _ShowTrackPlayTimeColumn = value;
                    RaisePropertyChanged("ShowTrackPlayTimeColumn");
                }
            }
        }



        private bool _ShowTrackFileNameColumn;
        public bool ShowTrackFileNameColumn
        {
            get
            {
                return _ShowTrackFileNameColumn;
            }
            set
            {
                if(_ShowTrackFileNameColumn != value)
                {
                    _ShowTrackFileNameColumn = value;
                    RaisePropertyChanged("ShowTrackFileNameColumn");
                }
            }
        }


        private bool _ShowTrackFilePathColumn;
        public bool ShowTrackFilePathColumn
        {
            get
            {
                return _ShowTrackFilePathColumn;
            }
            set
            {
                if(_ShowTrackFilePathColumn != value)
                {
                    _ShowTrackFilePathColumn = value;
                    RaisePropertyChanged("ShowTrackFilePathColumn");
                }
            }
        }


        private bool _ShowTrackFileSizeColumn;
        public bool ShowTrackFileSizeColumn
        {
            get
            {
                return _ShowTrackFileSizeColumn;
            }
            set
            {
                if(_ShowTrackFileSizeColumn != value)
                {
                    _ShowTrackFileSizeColumn = value;
                    RaisePropertyChanged("ShowTrackFileSizeColumn");
                }
            }
        }


        private bool _ShowTrackBitRateColumn;
        public bool ShowTrackBitRateColumn
        {
            get
            {
                return _ShowTrackBitRateColumn;
            }
            set
            {
                if(_ShowTrackBitRateColumn != value)
                {
                    _ShowTrackBitRateColumn = value;
                    RaisePropertyChanged("ShowTrackBitRateColumn");
                }
            }
        }


        private bool _ShowTrackComments1Column;
        public bool ShowTrackComment1Column
        {
            get
            {
                return _ShowTrackComments1Column;
            }
            set
            {
                if(_ShowTrackComments1Column != value)
                {
                    _ShowTrackComments1Column = value;
                    RaisePropertyChanged("ShowTrackComment1Column");
                }
            }
        }



        private bool _ShowTrackComments2Column;
        public bool ShowTrackComment2Column
        {
            get
            {
                return _ShowTrackComments2Column;
            }
            set
            {
                if(_ShowTrackComments2Column != value)
                {
                    _ShowTrackComments2Column = value;
                    RaisePropertyChanged("ShowTrackComment2Column");
                }
            }
        }


        private bool _ShowTrackLyricsColumn;
        public bool ShowTrackLyricsColumn
        {
            get
            {
                return _ShowTrackLyricsColumn;
            }
            set
            {
                if(_ShowTrackLyricsColumn != value)
                {
                    _ShowTrackLyricsColumn = value;
                    RaisePropertyChanged("ShowTrackLyricsColumn");
                }
            }
        }



        #endregion


        #region Track Data Columns

        private bool _ShowTrackDataTitleColumn;
        public bool ShowTrackDataTitleColumn
        {
            get
            {
                return _ShowTrackDataTitleColumn;
            }
            set
            {
                if(_ShowTrackDataTitleColumn != value)
                {
                    _ShowTrackDataTitleColumn = value;
                    RaisePropertyChanged("ShowTrackDataTitleColumn");
                }
            }
        }


        private bool _ShowTrackDataMixColumn;
        public bool ShowTrackDataMixColumn
        {
            get
            {
                return _ShowTrackDataMixColumn;
            }
            set
            {
                if(_ShowTrackDataMixColumn != value)
                {
                    _ShowTrackDataMixColumn = value;
                    RaisePropertyChanged("ShowTrackDataMixColumn");
                }
            }
        }


        private bool _ShowTrackDataArtistColumn;
        public bool ShowTrackDataArtistColumn
        {
            get
            {
                return _ShowTrackDataArtistColumn;
            }
            set
            {
                if(_ShowTrackDataArtistColumn != value)
                {
                    _ShowTrackDataArtistColumn = value;
                    RaisePropertyChanged("ShowTrackDataArtistColumn");
                }
            }
        }


        private bool _ShowTrackDataRemixerColumn;
        public bool ShowTrackDataRemixerColumn
        {
            get
            {
                return _ShowTrackDataRemixerColumn;
            }
            set
            {
                if(_ShowTrackDataRemixerColumn != value)
                {
                    _ShowTrackDataRemixerColumn = value;
                    RaisePropertyChanged("ShowTrackDataRemixerColumn");
                }
            }
        }


        private bool _ShowTrackDataProducerColumn;
        public bool ShowTrackDataProducerColumn
        {
            get
            {
                return _ShowTrackDataProducerColumn;
            }
            set
            {
                if(_ShowTrackDataProducerColumn != value)
                {
                    _ShowTrackDataProducerColumn = value;
                    RaisePropertyChanged("ShowTrackDataProducerColumn");
                }
            }
        }

        private bool _ShowTrackDataReleaseColumn;
        public bool ShowTrackDataReleaseColumn
        {
            get
            {
                return _ShowTrackDataReleaseColumn;
            }
            set
            {
                if(_ShowTrackDataReleaseColumn != value)
                {
                    _ShowTrackDataReleaseColumn = value;
                    RaisePropertyChanged("ShowTrackDataReleaseColumn");
                }
            }
        }

        private bool _ShowTrackDataReleasedColumn;
        public bool ShowTrackDataReleasedColumn
        {
            get
            {
                return _ShowTrackDataReleasedColumn;
            }
            set
            {
                if(_ShowTrackDataReleasedColumn != value)
                {
                    _ShowTrackDataReleasedColumn = value;
                    RaisePropertyChanged("ShowTrackDataReleasedColumn");
                }
            }
        }


        private bool _ShowTrackDataLabelColumn;
        public bool ShowTrackDataLabelColumn
        {
            get
            {
                return _ShowTrackDataLabelColumn;
            }
            set
            {
                if(_ShowTrackDataLabelColumn != value)
                {
                    _ShowTrackDataLabelColumn = value;
                    RaisePropertyChanged("ShowTrackDataLabelColumn");
                }
            }
        }



        private bool _ShowTrackDataCatalogNoColumn;
        public bool ShowTrackDataCatalogNoColumn
        {
            get
            {
                return _ShowTrackDataCatalogNoColumn;
            }
            set
            {
                if(_ShowTrackDataCatalogNoColumn != value)
                {
                    _ShowTrackDataCatalogNoColumn = value;
                    RaisePropertyChanged("ShowTrackDataCatalogNoColumn");
                }
            }
        }


        private bool _ShowTrackDataGenreColumn;
        public bool ShowTrackDataGenreColumn
        {
            get
            {
                return _ShowTrackDataGenreColumn;
            }
            set
            {
                if(_ShowTrackDataGenreColumn != value)
                {
                    _ShowTrackDataGenreColumn = value;
                    RaisePropertyChanged("ShowTrackDataGenreColumn");
                }
            }
        }


        private bool _ShowTrackDataKeyColumn;
        public bool ShowTrackDataKeyColumn
        {
            get
            {
                return _ShowTrackDataKeyColumn;
            }
            set
            {
                if(_ShowTrackDataKeyColumn != value)
                {
                    _ShowTrackDataKeyColumn = value;
                    RaisePropertyChanged("ShowTrackDataKeyColumn");
                }
            }
        }



        #endregion


        public ICommand ResetTrackColumnsCommand { get; private set; }
        public ICommand ResetTrackDataColumnsCommand { get; private set; }



        public ColumnSettingsViewModel()
        {
            ResetTrackDataColumns(null);
            ResetTrackColumns(null);

            this.ResetTrackColumnsCommand = new RelayCommand(new Action<object>(this.ResetTrackColumns));
            this.ResetTrackDataColumnsCommand = new RelayCommand(new Action<object>(this.ResetTrackDataColumns));
        }





        #region INotifyProperty Changed
        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if(handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void ResetTrackColumns(object o)
        {
            ShowTrackTitleColumn = true;
            ShowTrackMixColumn = true;
            ShowTrackArtistColumn = true;
            ShowTrackRemixerColumn = true;
            ShowTrackProducerColumn = true;
            ShowTrackReleaseColumn = true;
            ShowTrackReleasedColumn = true;
            ShowTrackLabelColumn = true;
            ShowTrackCatalogNoColumn = true;
            ShowTrackGenreColumn = true;
            ShowTrackKeyColumn = true;

            ShowTrackBPMColumn = false;
            ShowTrackRatingColumn = false;
            ShowTrackImportedColumn = false;
            ShowTrackModifiedColumn = false;
            ShowTrackLastPlayedColumn = false;
            ShowTrackPlayCountColumn = false;
            ShowTrackPlayTimeColumn = false;

            ShowTrackFileNameColumn = false;
            ShowTrackFilePathColumn = false;
            ShowTrackFileSizeColumn = false;
            ShowTrackBitRateColumn = false;

            ShowTrackComment1Column = false;
            ShowTrackComment2Column = false;
            ShowTrackLyricsColumn = false;
        }

        public void ResetTrackDataColumns(object o)
        {
            ShowTrackDataTitleColumn = true;
            ShowTrackDataMixColumn = true;
            ShowTrackDataArtistColumn = true;
            ShowTrackDataRemixerColumn = true;
            ShowTrackDataProducerColumn = true;
            ShowTrackDataReleaseColumn = true;
            ShowTrackDataReleasedColumn = true;
            ShowTrackDataLabelColumn = true;
            ShowTrackDataCatalogNoColumn = true;
            ShowTrackDataGenreColumn = true;
            ShowTrackDataKeyColumn = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}

