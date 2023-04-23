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
        public InfoViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            // Hookup commands
            this.SendDataCommand = new ActionCommand(this.SendDataCommandExecute, this.SendDataCommandCanExecute);

            // subscribe to event
            this.EventAggregator.GetEvent<SelectedSongChangedEvent>().Subscribe(this.OnSelectedSongChanged, ThreadOption.UIThread);
        }
        #endregion

        #region ------------------------- Properties, Indexers ------------------------------------------------------------
        public string Title { get; set; }
        public string Artists { get; set; }
        public string AlbumName { get; set; }
        public int? Year { get; set; }
        public BitmapImage AlbumCover { get; set; }
        public string Genre { get; set; }
        public string Lyrics { get; set; }
        public string Lyricist { get; set; }
        public ICommand SendDataCommand { get; set; }
        #endregion

        #region ------------------------- Private helper ------------------------------------------------------------------
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

        private void OnSelectedSongChanged(Song song)
        {
            // Write to UI
            Title = song.Title;
            Artists = song.Artists.ToString();
            AlbumName = song.AlbumName;
            Year = song.Year;
            AlbumCover = song.AlbumCover;
            Lyrics = song.Lyrics;
            Lyricist = song.Lyricist;
            Genre = song.Genre;

            // this shid important
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
