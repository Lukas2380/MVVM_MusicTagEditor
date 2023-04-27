using Common.Command;
using Common.NotifyPropertyChanged;
using Microsoft.Practices.Prism.Events;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using MVVM_MusicTagEditor.Events;
using MVVM_MusicTagEditor.Views;
using Services.Dialog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;

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
        private UserControl songViewTemplate;
        private UserControl infoView;

        private bool isDarkTheme;
        #endregion

        #region ------------------------- Constructors, Destructors, Dispose, Clone -------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            // Hookup commands to associated methods
            this.ToggleThemeCommand = new ActionCommand(this.ToggleThemeCommandExecute, this.ToggleThemeCommandCanExecute);
            this.ChooseDirectoryCommand = new ActionCommand(this.ChooseDirectoryCommandExecute, this.ChooseDirectoryCommandCanExecute);

            // Init infoview and infoviewmodel
            this.infoView = new InfoView();
            InfoViewModel infoViewModel = new InfoViewModel(this.EventAggregator);
            infoView.DataContext = infoViewModel;

            // Init songViewTemplate and model
            this.songViewTemplate = new SongViewTemplate();
            
            // Init theme
            this.isDarkTheme = Application.Current.Resources.Source.ToString().Contains("dark") ? false : true;

            // Set starting views
            this.CurrentViewLeft = this.songViewTemplate;
            this.CurrentViewRight = this.infoView;

            // subscribe to event
            this.EventAggregator.GetEvent<ChangedSongViewDataEvent>().Subscribe(this.CreateNewSongView, ThreadOption.UIThread);
        }
        #endregion

        #region ------------------------- Properties, Indexers ----------------------------------------------
        /// <summary>
        /// Gets the students view button command.
        /// </summary>
        public ICommand SongViewCommand { get; private set; }
        public ICommand InfoViewCommand { get; private set; }
        public ICommand ToggleThemeCommand { get; private set; }
        public ICommand ChooseDirectoryCommand { get; private set; }

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

        public bool IsDarkTheme
        {
            get { return this.isDarkTheme; }
            set 
            { 
                if (this.isDarkTheme != value)
                { this.isDarkTheme = value;}
            }
        }
        #endregion

        #region ------------------------- Private helper ----------------------------------------------------
        private void CreateNewSongView(string dir)
        {
            // Init songview and songviewmodel
            this.songView = new SongView();
            SongViewModel songViewModel = new SongViewModel(this.EventAggregator, dir);
            songView.DataContext = songViewModel;

            this.CurrentViewLeft = this.songView;
        }
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

        private bool ToggleThemeCommandCanExecute(object parameter)
        {
            return true;
        }

        private void ToggleThemeCommandExecute(object parameter)
        {
            if (isDarkTheme)
            {
                ChangeTheme("LightTheme");
                isDarkTheme = false;
            }
            else
            {
                ChangeTheme("DarkTheme");
                isDarkTheme = true;
            }
        }

        private void ChangeTheme(string theme)
        {
            theme = "ResourceDictionaries/" + theme + ".xaml";
            Application.Current.Resources.Source = new Uri(theme, UriKind.RelativeOrAbsolute);
            this.OnPropertyChanged(nameof(Application.Current.Resources.Source));
        }




        private bool ChooseDirectoryCommandCanExecute(object parameter)
        {
            return true;
        }

        private void ChooseDirectoryCommandExecute(object parameter)
        {
            if (ChangeDirectoryService.ChangeDirectory(out string filename))
                this.EventAggregator.GetEvent<ChangedSongViewDataEvent>().Publish(filename);
        }
        #endregion
    }
}
