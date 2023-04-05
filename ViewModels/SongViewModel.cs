using Data;
using Microsoft.Practices.Prism.Events;
using MVVM_MusicTagEditor.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
//using TagLib.Id3v2;

namespace MVVM_MusicTagEditor.ViewModels
{
    public class SongViewModel : ViewModelBase
    {
        #region ------------------------- Fields, Constants, Delegates, Events --------------------------------------------
        /// <summary> Selected Song from ComboBox. </summary>
        private Song selectedSong;
        #endregion

        #region ------------------------- Constructors, Destructors, Dispose, Clone ---------------------------------------
        public SongViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            // load song data
            this.LoadSongData();

            // subscribe to event
            this.EventAggregator.GetEvent<SongDataChangedEvent>().Subscribe(this.OnSongDataChanged, ThreadOption.UIThread);
        }
        #endregion

        #region ------------------------- Properties, Indexers ------------------------------------------------------------
        /// <summary> Gets or sets the collection of all songs. </summary>
        public ObservableCollection<Song> Songs { get; set; }

        /// <summary> Gets or sets the selected song from the ComboBox</summary>
        public Song SelectedSong
        {
            get
            {
                return this.selectedSong;
            }
            set
            {
                if (this.selectedSong != value)
                {
                    this.selectedSong = value;
                    this.OnPropertyChanged(nameof(this.selectedSong));

                    this.EventAggregator.GetEvent<SelectedSongChangedEvent>().Publish(value);
                }
            }
        }
        #endregion

        #region ------------------------- Private helper ------------------------------------------------------------------
        private void LoadSongData()
        {
            // Init collection
            ObservableCollection<Song> songs = new ObservableCollection<Song>();

            string[] musicFiles = Directory.GetFiles(@"D:\Music\download", "*.mp3");
            foreach (var musicFile in musicFiles)
            {
                using (var tfile = TagLib.File.Create(musicFile))
                {
                    // Lade das Coverbild als BitmapImage
                    var coverData = tfile.Tag.Pictures.FirstOrDefault();
                    var coverImage = new BitmapImage();
                    if (coverData != null)
                    {
                        using (var ms = new MemoryStream(coverData.Data.Data))
                        {
                            coverImage.BeginInit();
                            coverImage.CacheOption = BitmapCacheOption.OnLoad;
                            coverImage.StreamSource = ms;
                            coverImage.EndInit();
                        }
                    }

                    // Erstelle ein neues Song-Objekt und füge es der ObservableCollection hinzu
                    songs.Add(new Song(tfile.Tag.Title, tfile.Tag.Album, coverImage));
                }
            }


            // Set Students
            this.Songs = songs;
        }

        private void OnSongDataChanged(Song song)
        {
            // Save student data
            this.Songs.Add(song);
        }
        #endregion

        #region ------------------------- Commands ------------------------------------------------------------------------
        #endregion
    }
}
