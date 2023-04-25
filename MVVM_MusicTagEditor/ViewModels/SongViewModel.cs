using Data;
using Microsoft.Practices.Prism.Events;
using MVVM_MusicTagEditor.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using Id3;
using Services.SongData;

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

            string[] musicFiles = Directory.GetFiles(@"C:\Users\lukas\Music\download");
            foreach (var musicFile in musicFiles)
            {
                // Error if used by another process!!
                using (var mp3 = new Mp3(musicFile))
                {
                    Id3Tag tag = mp3.GetTag(Id3TagFamily.Version2X);

                    songs.Add(CreateSong(tag));
                }
            }

            // Set Songs
            this.Songs = songs;
        }

        private Song CreateSong(Id3Tag tag)
        {
            BitmapImage coverImage = GetSongData.GetAlbumCover(tag);
            string lyrics = GetSongData.GetLyrics(tag);
            string genre = GetSongData.GetGenre(tag);

            return new Song(tag.Title, tag.Artists.Value.ToArray(), tag.Album, tag.Year.Value, coverImage, genre, lyrics, tag.Lyricists);
        }

        private void OnSongDataChanged(Song song)
        {
            //// Save student data
            //this.Songs.Add(song);
        }
        #endregion

        #region ------------------------- Commands ------------------------------------------------------------------------
    #endregion
    }
}
