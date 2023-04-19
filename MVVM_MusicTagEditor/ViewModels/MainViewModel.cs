using Common.Command;
using Common.NotifyPropertyChanged;
using Microsoft.Practices.Prism.Events;
using MVVM_MusicTagEditor.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MVVM_MusicTagEditor.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region ------------------------- Fields, Constants, Delegates, Events ------------------------------
        /// <summary>
        /// View that is currently bound to the <see cref="ContentControl"/> left.
        /// </summary>
        private UserControl currentViewLeft;
        private UserControl currentViewRight;

        private UserControl songView;
        private UserControl infoView;
        #endregion

        #region ------------------------- Constructors, Destructors, Dispose, Clone -------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            // Hookup commands to associated methods
            this.SongViewCommand = new ActionCommand(this.SongViewCommandExecute, this.SongViewCommandCanExecute);
            this.InfoViewCommand = new ActionCommand(this.InfoViewCommandExecute, this.InfoViewCommandCanExecute);

            // Init songview and songviewmodel
            this.songView = new SongView();
            SongViewModel songViewModel = new SongViewModel(this.EventAggregator);
            songView.DataContext = songViewModel;

            // Init infoview and infoviewmodel
            this.infoView = new InfoView();
            InfoViewModel infoViewModel = new InfoViewModel(this.EventAggregator);
            infoView.DataContext = infoViewModel;
        }
        #endregion

        #region ------------------------- Properties, Indexers ----------------------------------------------
        /// <summary>
        /// Gets the students view button command.
        /// </summary>
        //public ICommand StudentsViewCommand { get; private set; }
        public ICommand SongViewCommand { get; private set; }

        //public ICommand SettingsViewCommand { get; private set; }
        public ICommand InfoViewCommand { get; private set; }

        /// <summary>
        /// Gets and sets the view that is currently bound to the <see cref="ContentControl"/> left.
        /// </summary>
        public UserControl CurrentViewLeft
        {
            get
            {
                return this.currentViewLeft;
            }
            set
            {
                if (this.currentViewLeft != value)
                {
                    this.currentViewLeft = value;
                    this.OnPropertyChanged(nameof(this.currentViewLeft));
                }
            }
        }

        public UserControl CurrentViewRight
        {
            get
            {
                return this.currentViewRight;
            }
            set
            {
                if (this.currentViewRight != value)
                {
                    this.currentViewRight = value;
                    this.OnPropertyChanged(nameof(this.currentViewRight));
                }
            }
        }
        #endregion

        #region ------------------------- Private helper ----------------------------------------------------
        #endregion

        #region ------------------------- Commands ----------------------------------------------------------
        /// <summary>
        /// Determines whether the song view command can be executed.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        /// <returns><c>true</c> if the command can be executed otherwise <c>false</c>.</returns>
        private bool SongViewCommandCanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Occures when the user clicks the student view button.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        private void SongViewCommandExecute(object parameter)
        {
            this.CurrentViewLeft = this.CurrentViewLeft == null ? this.songView : null;
        }

        private bool InfoViewCommandCanExecute(object parameter)
        {
            return true;
        }

        private void InfoViewCommandExecute(object parameter)
        {
            this.CurrentViewRight = this.CurrentViewRight == null ? this.infoView : null;
        }
        #endregion
    }
}
