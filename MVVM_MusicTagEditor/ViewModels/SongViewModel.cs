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
            BitmapImage coverImage = GetAlbumCover(tag);
            string lyrics = GetLyrics(tag);

            return new Song(tag.Title, tag.Artists.Value.ToArray(), tag.Album, tag.Year.Value, coverImage, lyrics, tag.Lyricists);
        }

        /// <summary>
        /// Gets the tag for a mp3 file and checks if there is an album cover.
        /// Returns an empty string if there is no album cover found.
        /// </summary>
        /// <param name="tag">Id3Tag from an mp3 file. </param>
        /// <returns>Returns the album cover as a BitmapImage. </returns>
        private BitmapImage GetAlbumCover(Id3Tag tag)
        {
            var coverData = tag.Pictures.FirstOrDefault();
            BitmapImage coverImage;

            if (coverData != null)
            {
                using (var ms = new MemoryStream(coverData.PictureData))
                {
                    coverImage = ToBitmapImage(new Bitmap(ms));
                }
            }
            else
            {
                Bitmap bitmap = ConvertToBitmap("..\\..\\..\\img\\mp3.png");
                coverImage = ToBitmapImage(bitmap);
            }
            return coverImage;
        }

        /// <summary>
        /// Gets the tag for a mp3 file and checks if there are lyrics.
        /// Returns an empty string if there are no lyrics found.
        /// </summary>
        /// <param name="tag">Id3Tag from an mp3 file. </param>
        /// <returns>Returns the lyrics as a string. </returns>
        private string GetLyrics(Id3Tag tag)
        {
            var lyricsData = tag.Lyrics.FirstOrDefault();
            string lyrics = "";

            if (lyricsData != null)
            {
                lyrics = lyricsData.Lyrics;
            }
            return lyrics;
        }

        private void OnSongDataChanged(Song song)
        {
            //// Save student data
            //this.Songs.Add(song);
        }

        public Bitmap ConvertToBitmap(string fileName)
        {
            Bitmap bitmap;
            using (Stream bmpStream = System.IO.File.Open(fileName, System.IO.FileMode.Open))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(bmpStream);

                bitmap = new Bitmap(image);

            }
            return bitmap;
        }

        public BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg); // Was .Bmp, but this did not show a transparent background.

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }

    #endregion

    #region ------------------------- Commands ------------------------------------------------------------------------
    #endregion
}
}
