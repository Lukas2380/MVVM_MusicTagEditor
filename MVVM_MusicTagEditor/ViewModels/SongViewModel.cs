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
using System.ComponentModel;
using System.Windows.Input;
using Common.Command;
using System.Collections;

namespace MVVM_MusicTagEditor.ViewModels
{
    public class SongViewModel : ViewModelBase
    {
        #region ------------------------- Fields, Constants, Delegates, Events --------------------------------------------
        private Song selectedSong;
        private string songdirectory;
        private int progressBarValue;
        private BackgroundWorker songDataLoader;

        #endregion

        #region ------------------------- Constructors, Destructors, Dispose, Clone ---------------------------------------
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SongViewModel"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator used for communication between view models.</param>
        /// <param name="dir">The directory of the songs.</param>
        public SongViewModel(IEventAggregator eventAggregator, string dir) : base(eventAggregator)
        {
            this.SongDirectory = dir;

            // Initialize the BackgroundWorker
            this.songDataLoader = new BackgroundWorker();
            this.songDataLoader.DoWork += this.LoadSongData;
            this.songDataLoader.RunWorkerCompleted += this.OnSongDataLoaded;

            // Start the BackgroundWorker
            this.songDataLoader.RunWorkerAsync();

            // subscribe to event
            this.EventAggregator.GetEvent<SongDataChangedEvent>().Subscribe(this.OnSongDataChanged, ThreadOption.UIThread);
        }

        #endregion

        #region ------------------------- Properties, Indexers ------------------------------------------------------------

        /// <summary> 
        /// Gets or sets the collection of all songs. 
        /// </summary>
        public ObservableCollection<Song> Songs { get; set; }

        #region SelectedItems
        // This is from this website: https://stackoverflow.com/questions/9880589/bind-to-selecteditems-from-datagrid-or-listbox-in-mvvm 
        // (The answer starts with "Binding directly do view model, little tricky version:")
        public List<Song> SelectedItems { get; set; } = new List<Song>();
        public ICommand SelectedItemsCommand
        {
            get
            {
                return new GetSelectedItemsCommand(list =>
                {
                    SelectedItems.Clear();
                    IList items = (IList)list;
                    IEnumerable<Song> collection = items.Cast<Song>();
                    SelectedItems = collection.ToList();
                });
            }
        }
        #endregion

        /// <summary> 
        /// Gets or sets the selected song for the UI.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the directory of the songs.
        /// </summary>
        public string SongDirectory
        {
            get
            {
                return this.songdirectory;
            }
            set
            {
                if (this.songdirectory != value)
                {
                    this.songdirectory = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the value for the progress bar.
        /// </summary>
        public int ProgressBarValue
        {
            get { return this.progressBarValue; }
            set
            {
                if (this.progressBarValue != value)
                {
                    this.progressBarValue = value;
                    this.OnPropertyChanged(nameof(this.ProgressBarValue));
                }
            }
        }
        #endregion

        #region ------------------------- Private helper ------------------------------------------------------------------

        /// <summary>
        /// The LoadSongData method uses a backgroundworker and loads the data for every song in the <see cref="songdirectory"/>
        /// </summary>
        private void LoadSongData(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            // Init collection
            ObservableCollection<Song> songs = new ObservableCollection<Song>();

            string[] musicFiles = Directory.GetFiles(@songdirectory);
            int totalFiles = musicFiles.Length;
            int filesProcessed = 0;
            foreach (var musicFile in musicFiles)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                // Error if used by another process!!
                Exception ex;

                using (var mp3 = new Mp3(musicFile))
                {
                    Id3Tag tag = mp3.GetTag(Id3TagFamily.Version2X);

                    if (tag != null)
                    {
                        songs.Add(CreateSong(tag, musicFile));
                    }
                }

                filesProcessed++;
                ProgressBarValue = (int)((float)filesProcessed / (float)totalFiles * 100);
            }

            // Set Songs
            e.Result = songs;

            // Load songs to db
            SongDbContext.CreateNewDataBase(songs.ToList());
        }

        /// <summary>
        /// The create song method creates a new song based on the <see cref="Id3Tag"/>.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private Song CreateSong(Id3Tag tag, string location)
        {
            BitmapImage coverImage = GetSongData.GetAlbumCover(tag);
            string lyrics = GetSongData.GetLyrics(tag);
            string genre = GetSongData.GetGenre(tag);

            return new Song(tag.Title, tag.Artists.Value.ToArray(), tag.Album, tag.Year.Value, coverImage, genre, lyrics, tag.Lyricists, location);
        }

        private void OnSongDataChanged(Song song)
        {
            //// Save song data
            //this.Songs.Add(song);
        }

        /// <summary>
        /// The OnSongDataLoaded method handels what happens when loading the song data is finished.
        /// </summary>
        private void OnSongDataLoaded(object sender, RunWorkerCompletedEventArgs e)
        {
            // Set Songs property to the loaded data
            this.Songs = (ObservableCollection<Song>)e.Result;

            this.OnPropertyChanged(nameof(Songs));
            this.ProgressBarValue = 100;
            this.songDataLoader.Dispose();
        }

        #endregion
    }
}
