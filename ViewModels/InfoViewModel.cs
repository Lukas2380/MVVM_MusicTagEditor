using Common.Command;
using Data;
using Microsoft.Practices.Prism.Events;
using MVVM_MusicTagEditor.Events;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public string AlbumName { get; set; }
        public BitmapImage AlbumCover { get; set; }
        public ICommand SendDataCommand { get; set; }
        #endregion

        #region ------------------------- Private helper ------------------------------------------------------------------
        #endregion

        #region ------------------------- Commands ------------------------------------------------------------------------
        private bool SendDataCommandCanExecute(object parameter)
        {
            return true;
        }

        private void SendDataCommandExecute(object parameter)
        {
            // Create new song
            Song song = new Song(this.Title, this.AlbumName, this.AlbumCover);

            // Send data to SongViewModel
            this.EventAggregator.GetEvent<SongDataChangedEvent>().Publish(song);
        }

        private void OnSelectedSongChanged(Song song)
        {
            // Write to UI
            Title = song.Title;
            AlbumName = song.AlbumName;
            this.OnPropertyChanged(nameof(Title));
            this.OnPropertyChanged(nameof(AlbumName));
        }
        #endregion
    }
}
