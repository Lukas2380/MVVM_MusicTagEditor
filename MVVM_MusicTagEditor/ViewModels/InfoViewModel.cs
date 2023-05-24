using Common.Command;
using Data;
using Microsoft.Practices.Prism.Events;
using MVVM_MusicTagEditor.Events;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MVVM_MusicTagEditor.ViewModels
{
    public class InfoViewModel : ViewModelBase
    {
        #region ------------------------- Constructors, Destructors, Dispose, Clone ---------------------------------------
        /// <summary>
        /// Initializes a new instance of the InfoViewModel class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator used for communication between view models.</param>
        public InfoViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            // Hookup commands
            this.SendDataCommand = new ActionCommand(this.SendDataCommandExecute, this.SendDataCommandCanExecute);

            // subscribe to event
            this.EventAggregator.GetEvent<SelectedSongChangedEvent>().Subscribe(this.OnSelectedSongChanged, ThreadOption.UIThread);
        }
        #endregion

        #region ------------------------- Properties, Indexers ------------------------------------------------------------
        
        /// <summary>
        /// Gets or sets the title of the song.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the artists of the song.
        /// </summary>
        public string Artists { get; set; }

        /// <summary>
        /// Gets or sets the album name of the song.
        /// </summary>
        public string AlbumName { get; set; }

        /// <summary>
        /// Gets or sets the release year of the song.
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Gets or sets the album cover image of the song.
        /// </summary>
        public BitmapImage AlbumCover { get; set; }

        /// <summary>
        /// Gets or sets the genre of the song.
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// Gets or sets the lyrics of the song.
        /// </summary>
        public string Lyrics { get; set; }

        /// <summary>
        /// Gets or sets the lyricist of the song.
        /// </summary>
        public string Lyricist { get; set; }

        /// <summary>
        /// Gets or sets the command to send data.
        /// </summary>
        public ICommand SendDataCommand { get; set; }

        #endregion

        #region ------------------------- Commands ------------------------------------------------------------------------
        private bool SendDataCommandCanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Save button
        /// </summary>
        /// <param name="parameter"></param>
        private void SendDataCommandExecute(object parameter)
        {
            // Create new song
            //Song song = new Song(this.Title, this.Artists.Split(','), this.AlbumName, this.Year, this.AlbumCover, this.Lyrics, this.Lyricist);
            // Send data to SongViewModel
            //this.EventAggregator.GetEvent<SongDataChangedEvent>().Publish(song);
        }

        /// <summary>
        /// Handles the event when the selected song changes.
        /// </summary>
        /// <param name="song">The selected song.</param>
        private void OnSelectedSongChanged(Song song)
        {
            // Update the UI
            Title = song.Title;
            Artists = song.Artists.ToString();
            AlbumName = song.AlbumName;
            Year = song.Year;
            AlbumCover = song.AlbumCover;
            Lyrics = song.Lyrics;
            Lyricist = song.Lyricist;
            Genre = song.Genre;

            // Notify the UI
            this.OnPropertyChanged(nameof(Title));
            this.OnPropertyChanged(nameof(Artists));
            this.OnPropertyChanged(nameof(AlbumName));
            this.OnPropertyChanged(nameof(AlbumCover));
            this.OnPropertyChanged(nameof(Lyrics));
            this.OnPropertyChanged(nameof(Lyricist));
            this.OnPropertyChanged(nameof(Year));
            this.OnPropertyChanged(nameof(Genre));
        }

        #endregion
    }
}
