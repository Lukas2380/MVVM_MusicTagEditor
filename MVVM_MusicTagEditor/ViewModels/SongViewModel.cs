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

            string[] musicFiles = Directory.GetFiles(@"C:\Users\lukas\Music\download", "*.mp3");
            foreach (var musicFile in musicFiles)
            {
                using (var tfile = TagLib.File.Create(musicFile))
                {
                    // Load the cover image as BitmapImage
                    var coverData = tfile.Tag.Pictures.FirstOrDefault();
                    if (coverData != null)
                    {
                        using (var ms = new MemoryStream(coverData.Data.Data))
                        {
                            var coverImage = ToBitmapImage(new Bitmap(ms));
                            songs.Add(new Song(tfile.Tag.Title, tfile.Tag.Artists, tfile.Tag.Album, int.Parse(tfile.Tag.Year.ToString()), coverImage, tfile.Tag.Lyrics));
                        }
                    }
                    else
                    {
                        Bitmap bitmap = ConvertToBitmap("..\\..\\..\\img\\mp3.png");
                        BitmapSource sauce = BitmapToBitmapSource(bitmap);
                        var coverImage = ToBitmapImage(bitmap);

                        songs.Add(new Song(tfile.Tag.Title, tfile.Tag.Artists, tfile.Tag.Album, int.Parse(tfile.Tag.Year.ToString()), coverImage, tfile.Tag.Lyrics));
                    }
                }
            }

            // Set Songs
            this.Songs = songs;
        }

        private void OnSongDataChanged(Song song)
        {
            //// Save student data
            //this.Songs.Add(song);
        }

        public static BitmapSource BitmapToBitmapSource(Bitmap source)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                          source.GetHbitmap(),
                          IntPtr.Zero,
                          Int32Rect.Empty,
                          BitmapSizeOptions.FromEmptyOptions());
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
