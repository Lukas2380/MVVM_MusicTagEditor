using Common.Command;
using Data;
using MahApps.Metro.Controls;
using Microsoft.Practices.Prism.Events;
using MVVM_MusicTagEditor.Events;
using Services.BitMapImageHelperMethods;
using Services.FetchMetadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MVVM_MusicTagEditor.ViewModels
{
    public class EditWindowViewModel : ViewModelBase
    {
        #region ------------------------- Fields, Constants, Delegates, Events ------------------------------
        // Private helper variables.
        private string menuVisibility;
        private List<Song> selectedItems;
        private List<Song> currentSelectedItems;
        private BackgroundWorker backgroundWorker;

        private int selectedSongNr;

        // Private song variables.
        private string title;
        private string artists;
        private int? year;
        private string albumName;
        private string lyrics;
        private BitmapImage albumCover;
        private List<string> websites;
        #endregion


        #region ------------------------- Properties, Indexers ----------------------------------------------
        /// <summary>
        /// Gets or sets the MenuVisibility.
        /// </summary>
        public string MenuVisibility
        {
            get { return this.menuVisibility; }
            set
            {
                if (this.menuVisibility != value)
                {
                    this.menuVisibility = value;
                    OnPropertyChanged(nameof(this.menuVisibility));
                }
            }
        }

        /// <summary>
        /// Gets or sets the Selected items as a <see cref="List{T}"/> of <see cref="Song"/>.
        /// </summary>
        public List<Song> SelectedItems
        {
            get
            {
                return this.selectedItems;
            }
            set
            {
                if (this.selectedItems != value)
                {
                    this.selectedItems = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the currently selected song.
        /// </summary>
        public Song CurrentSong { get; set; }

        /// <summary>
        /// Gets or sets the fetched song from the internet.
        /// </summary>
        public Song FetchedSong { get; set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the EditWindowViewModel class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator used for communication between view models.</param>
        /// <param name="selectedItems">The list of selected songs.</param>
        public EditWindowViewModel(IEventAggregator eventAggregator, List<Song> selectedItems) : base(eventAggregator)
        {
            // Hookup commands to associated methods
            this.ToggleMenuCommand = new ActionCommand(this.ToggleMenuCommandExecute, this.ToggleMenuCommandCanExecute);
            this.FetchMetadataCommand = new ActionCommand(this.FetchMetadataCommandExecute, this.FetchMetadataCommandCanExecute);
            this.CopyTagCommand = new ActionCommand(this.CopyTagCommandExecute, this.CopyTagCommandCanExecute);
            this.OpenWebsitesCommand = new ActionCommand(this.OpenWebsitesCommandExecuteAsync, this.OpenWebsitesCommandCanExecute);
            this.PreviousSongCommand = new ActionCommand(this.PreviousSongCommandExecute, this.PreviousSongCommandCanExecute);
            this.NextSongCommand = new ActionCommand(this.NextSongCommandExecute, this.NextSongCommandCanExecute);

            this.MenuVisibility = "Collapsed";

            SelectedItems = selectedItems;
            currentSelectedItems = new List<Song>(SelectedItems);

            this.selectedSongNr = 0;
            CurrentSong = currentSelectedItems[this.selectedSongNr];
            FetchedSong = new Song();

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;

            // Try to start the background worker to execute the fetch 
            try
            {
                backgroundWorker.RunWorkerAsync();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("The program is already fetching metadata");
            }
        }

        #region ------------------------- Properties, Indexers ----------------------------------------------
        public ICommand ToggleMenuCommand { get; private set; }
        public ICommand FetchMetadataCommand { get; private set; }
        public ICommand CopyTagCommand { get; private set; }
        public ICommand OpenWebsitesCommand { get; private set; }

        public ICommand PreviousSongCommand { get; private set; }
        public ICommand NextSongCommand { get; private set; }
        #endregion

        #region ------------------------- Commands ----------------------------------------------------------

        #region ToggleMenuCommand
        private bool ToggleMenuCommandCanExecute(object arg)
        {
            return true;
        }

        private void ToggleMenuCommandExecute(object obj)
        {
            this.MenuVisibility = this.MenuVisibility == "Visible" ? "Collapsed" : "Visible";
        }
        #endregion

        #region FetchMetadataCommand
        private bool FetchMetadataCommandCanExecute(object arg)
        {
            return true;
        }

        private async void FetchMetadataCommandExecute(object obj)
        {
            if (backgroundWorker.IsBusy)
            {
                // The initial metadata fetch is still in progress, so we wait until it completes
                await Task.Run(() =>
                {
                    while (backgroundWorker.IsBusy)
                    {
                        // wait till the worker is not buisy anymore
                    }
                });
            }

            // Update the FetchedSong properties with the fetched values
            FetchedSong.Title = this.title;
            FetchedSong.Artists = this.artists;
            FetchedSong.Year = this.year;
            FetchedSong.AlbumName = this.albumName;
            FetchedSong.Lyrics = this.lyrics;
            FetchedSong.AlbumCover = this.albumCover;

            OnPropertyChanged(nameof(FetchedSong));
        }


        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Fetch the metadata and assign the values to FetchedSong properties
            FetchMetadata fetcher = new FetchMetadata(CurrentSong.Artists.ToString(), CurrentSong.Title);

            this.title = fetcher.Song;
            this.artists = fetcher.Artist;
            this.year = fetcher.ReleaseYear;
            this.albumName = fetcher.Album;
            this.lyrics = fetcher.Lyrics;
            this.albumCover = BitMapImageHelper.CreateBitmapImageFromUrl(fetcher.CoverUrl).Result;
            this.websites = fetcher.Websites;

            if (this.backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Update the UI or handle any exceptions in the RunWorkerCompleted event
            if (e.Error != null)
            {
                // Handle the exception and display an error message
                MessageBox.Show("An error occurred while fetching metadata: " + e.Error.Message);
            } else if (e.Cancelled)
            {
                this.FetchedSong = new Song();
                this.OnPropertyChanged(nameof(this.FetchedSong));
            }
        }

        #endregion

        #region CopyTagCommand
        private bool CopyTagCommandCanExecute(object arg)
        {
            return true;
        }

        private void CopyTagCommandExecute(object arg)
        {
            string tag = arg as string;

            switch (tag)
            {
                case "cover":
                    CurrentSong.AlbumCover = FetchedSong.AlbumCover;
                    break;

                case "title":
                    CurrentSong.Title = FetchedSong.Title;
                    break;

                case "album":
                    CurrentSong.AlbumName = FetchedSong.AlbumName;
                    break;

                case "artist":
                    CurrentSong.Artists = FetchedSong.Artists;
                    break;

                case "year":
                    CurrentSong.Year = FetchedSong.Year;
                    break;

                case "genre":
                    CurrentSong.Genre = FetchedSong.Genre;
                    break;

                case "lyricist":
                    CurrentSong.Lyricist = FetchedSong.Lyricist;
                    break;

                case "lyrics":
                    CurrentSong.Lyrics = FetchedSong.Lyrics;
                    break;
            }
        }
        #endregion

        #region PreviousSongCommand
        private bool PreviousSongCommandCanExecute(object arg)
        {
            if (this.selectedSongNr == 0)
                return false;
            return true;
        }

        private void PreviousSongCommandExecute(object obj)
        {
            this.selectedSongNr--;
            this.CurrentSong = currentSelectedItems[this.selectedSongNr];
            this.OnPropertyChanged(nameof(CurrentSong));

            SongToEditChanged();
        }
        #endregion

        #region OpenWebsitesCommand
        private bool OpenWebsitesCommandCanExecute(object arg)
        {
            return true;
        }

        private async void OpenWebsitesCommandExecuteAsync(object obj)
        {
            if (backgroundWorker.IsBusy)
            {
                // The initial metadata fetch is still in progress, so we wait until it completes
                await Task.Run(() =>
                {
                    while (backgroundWorker.IsBusy)
                    {
                        // wait till the worker is not buisy anymore
                    }
                });
            }

            foreach (string website in this.websites)
            {
                System.Diagnostics.Process.Start(website);
            }
        }
        #endregion


        #region NextSongCommand
        private bool NextSongCommandCanExecute(object arg)
        {
            if (this.selectedSongNr >=  currentSelectedItems.Count - 1)
                return false;
            return true;
        }

        private void NextSongCommandExecute(object obj)
        {
            this.selectedSongNr++;
            this.CurrentSong = currentSelectedItems[selectedSongNr];
            this.OnPropertyChanged(nameof(this.CurrentSong));

            SongToEditChanged();
        }
        #endregion
        #endregion

        #region ------------------------- Private helper ----------------------------------------------------
        private void SongToEditChanged()
        {
            //this.backgroundWorker.CancelAsync();
            this.backgroundWorker = null;
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;

            this.FetchedSong = new Song();
            this.OnPropertyChanged(nameof(this.FetchedSong));

            // Try to start the background worker to execute the fetch functions
            try
            {
                backgroundWorker.RunWorkerAsync();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show("The program is already fetching metadata");
            }
        }
        #endregion
    }
}