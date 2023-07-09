using ATL;
using Common.Command;
using Data;
using Microsoft.Practices.Prism.Events;
using Services.BitMapImageHelperMethods;
using Services.FetchMetadata;
using Services.SongData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MVVM_MusicTagEditor.ViewModels
{
    public class EditWindowViewModel : ViewModelBase
    {
        #region ------------------------- Constructors, Destructors, Dispose, Clone ---------------------------------------
        
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
            this.PasteAlbumCoverCommand = new ActionCommand(this.PasteAlbumCoverExecute, this.PasteAlbumCoverCanExecute);

            this.MenuVisibility = "Collapsed";

            SelectedItems = selectedItems;
            currentSelectedItems = new List<Song>(SelectedItems);

            SaveSongData.EditedSongs.AddRange(currentSelectedItems);

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

        #endregion

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

        /// <summary>
        /// Gets the command for toggling the menu visibility.
        /// </summary>
        public ICommand ToggleMenuCommand { get; private set; }

        /// <summary>
        /// Gets the command for fetching metadata.
        /// </summary>
        public ICommand FetchMetadataCommand { get; private set; }

        /// <summary>
        /// Gets the command for copying tags from the fetched song to the current song.
        /// </summary>
        public ICommand CopyTagCommand { get; private set; }

        /// <summary>
        /// Gets the command for opening the websites for the song.
        /// </summary>
        public ICommand OpenWebsitesCommand { get; private set; }

        /// <summary>
        /// Get the command for the previous song.
        /// </summary>
        public ICommand PreviousSongCommand { get; private set; }

        /// <summary>
        /// Gets the command for the next song.
        /// </summary>
        public ICommand NextSongCommand { get; private set; }

        /// <summary>
        /// Gets the command for the paste action.
        /// </summary>
        public ICommand PasteAlbumCoverCommand { get; private set; }

        #endregion

        #region ------------------------- Commands ----------------------------------------------------------

        #region ToggleMenuCommand

        /// <summary>
        /// Determines whether the ToggleMenuCommand can be executed.
        /// </summary>
        /// <param name="arg">The command parameter.</param>
        /// <returns>True, indicating that the command can always be executed.</returns>
        private bool ToggleMenuCommandCanExecute(object arg)
        {
            return true;
        }

        /// <summary>
        /// Executes the ToggleMenuCommand.
        /// </summary>
        /// <param name="obj">The command parameter.</param>
        private void ToggleMenuCommandExecute(object obj)
        {
            this.MenuVisibility = this.MenuVisibility == "Visible" ? "Collapsed" : "Visible";
        }

        #endregion

        #region FetchMetadataCommand
        /// <summary>
        /// Determines whether the FetchMetadataCommand can be executed.
        /// </summary>
        /// <param name="arg">The command parameter.</param>
        /// <returns>True, indicating that the command can always be executed.</returns>
        private bool FetchMetadataCommandCanExecute(object arg)
        {
            return true;
        }

        /// <summary>
        /// Executes the FetchMetadataCommand.
        /// </summary>
        /// <param name="obj">The command parameter.</param>
        private async void FetchMetadataCommandExecute(object obj)
        {
            if (backgroundWorker.IsBusy)
            {
                // The initial metadata fetch is still in progress, so we wait until it completes
                await Task.Run(() =>
                {
                    while (backgroundWorker.IsBusy)
                    {
                        // wait till the worker is not busy anymore
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
        #endregion

        #region CopyTagCommand

        /// <summary>
        /// Determines whether the CopyTagCommand can be executed.
        /// </summary>
        /// <param name="arg">The command parameter.</param>
        /// <returns>True, indicating that the command can always be executed.</returns>
        private bool CopyTagCommandCanExecute(object arg)
        {
            return true;
        }

        /// <summary>
        /// Executes the CopyTagCommand.
        /// </summary>
        /// <param name="arg">The command parameter.</param>
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

        /// <summary>
        /// Determines whether the PreviousSongCommand can be executed.
        /// </summary>
        /// <param name="arg">The command parameter.</param>
        /// <returns>True, if the selectedSongNr is greater than 0; otherwise, false.</returns>
        private bool PreviousSongCommandCanExecute(object arg)
        {
            if (this.selectedSongNr == 0)
                return false;
            return true;
        }

        /// <summary>
        /// Executes the PreviousSongCommand.
        /// </summary>
        /// <param name="obj">The command parameter.</param>
        private void PreviousSongCommandExecute(object obj)
        {
            this.selectedSongNr--;
            this.CurrentSong = currentSelectedItems[this.selectedSongNr];
            this.OnPropertyChanged(nameof(CurrentSong));
            SongToEditChanged();
        }

        #endregion

        #region OpenWebsitesCommand

        /// <summary>
        /// Determines whether the OpenWebsitesCommand can be executed.
        /// </summary>
        /// <param name="arg">The command parameter.</param>
        /// <returns>True, indicating that the command can always be executed.</returns>
        private bool OpenWebsitesCommandCanExecute(object arg)
        {
            return true;
        }

        /// <summary>
        /// Executes the OpenWebsitesCommand async.
        /// </summary>
        /// <param name="obj">The command parameter.</param>
        private async void OpenWebsitesCommandExecuteAsync(object obj)
        {
            if (backgroundWorker.IsBusy)
            {
                // The initial metadata fetch is still in progress, so we wait until it completes
                await Task.Run(() =>
                {
                    while (backgroundWorker.IsBusy)
                    {
                        // wait till the worker is not busy anymore
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

        /// <summary>
        /// Determines whether the NextSongCommand can be executed.
        /// </summary>
        /// <param name="arg">The command parameter.</param>
        /// <returns>True, if it is not the last song; otherwise, false.</returns>
        private bool NextSongCommandCanExecute(object arg)
        {
            if (this.selectedSongNr >= currentSelectedItems.Count - 1)
                return false;
            return true;
        }

        /// <summary>
        /// Executes the NextSongCommand.
        /// </summary>
        /// <param name="obj">The command parameter.</param>
        private void NextSongCommandExecute(object obj)
        {
            this.selectedSongNr++;
            this.CurrentSong = currentSelectedItems[selectedSongNr];
            this.OnPropertyChanged(nameof(this.CurrentSong));
            SongToEditChanged();
        }

        #endregion

        #region PasteAlbumCoverCommand

        /// <summary>
        /// Determines whether the PasteAlbumCoverCommand can be executed.
        /// </summary>
        /// <param name="arg">The command parameter.</param>
        /// <returns>True</returns>
        private bool PasteAlbumCoverCanExecute(object arg)
        {
            return true;
        }

        /// <summary>
        /// Executes the PasteAlbumCoverCommand.
        /// </summary>
        /// <param name="obj">The command parameter.</param>
        private void PasteAlbumCoverExecute(object obj)
        {
            if (Clipboard.ContainsImage())
            {
                FetchedSong.AlbumCover = BitMapImageHelper.GetClipBoardImage();
                OnPropertyChanged(nameof(FetchedSong));
            }
        }

        #endregion
        
        #endregion

        #region ------------------------- Private helper ----------------------------------------------------

        /// <summary>
        /// Executes the background work to fetch metadata and assign the values to FetchedSong properties.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Fetch the metadata
            FetchMetadata fetcher = new FetchMetadata(CurrentSong.Artists.ToString(), CurrentSong.Title);

            // Assign values
            this.title = fetcher.Song;
            this.artists = fetcher.Artist;
            this.year = fetcher.ReleaseYear;
            this.albumName = fetcher.Album;
            this.lyrics = fetcher.Lyrics;
            this.albumCover = fetcher.AlbumCover;
            this.websites = fetcher.Websites;

            if (this.backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
        }

        /// <summary>
        /// Handles the completion of the background worker.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Update the UI or handle any exceptions in the RunWorkerCompleted event
            if (e.Error != null)
            {
                // Handle the exception and display an error message
                MessageBox.Show("An error occurred while fetching metadata: " + e.Error.Message);
            }
            else if (e.Cancelled)
            {
                this.FetchedSong = new Song();
                this.OnPropertyChanged(nameof(this.FetchedSong));
            }
        }

        /// <summary>
        /// Handles the change of the song to be edited.
        /// </summary>
        private void SongToEditChanged()
        {
            // Create a new backgroundworker
            this.backgroundWorker = null;
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;

            // Clear the fetchedsong
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