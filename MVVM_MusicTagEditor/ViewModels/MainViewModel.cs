using Common.Command;
using Common.NotifyPropertyChanged;
using ControlzEx.Theming;
using Data;
using Microsoft.Practices.Prism.Events;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using MVVM_MusicTagEditor.Events;
using MVVM_MusicTagEditor.Views;
using Services.Dialog;
using Services.SongData;
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
        private UserControl currentViewLeft;
        private UserControl currentViewRight;

        private UserControl songView;
        private UserControl songViewTemplate;
        private UserControl infoView;

        private SongViewModel songViewModel;

        private EditWindowView editWindowView;
        private EditWindowViewModel editWindowViewModel;

        private bool isDarkTheme;
        private string menuVisibility;
        private bool editSelectionCanExecute = false;
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
            this.ToggleMenuCommand = new ActionCommand(this.ToggleMenuCommandExecute, this.ToggleMenuCommandCanExecute);
            this.EditSelectionCommand = new ActionCommand(this.EditSelectionCommandExecute, this.EditSelectionCommandCanExecute);
            this.SaveChangesCommand = new ActionCommand(this.SaveChangesCommandExecute, this.SaveChangesCommandCanExecute);

            // Init infoview and infoviewmodel
            this.infoView = new InfoView();
            InfoViewModel infoViewModel = new InfoViewModel(this.EventAggregator);
            infoView.DataContext = infoViewModel;

            // Init songViewTemplate and model
            this.songViewTemplate = new SongViewTemplate();

            // Init theme
            this.IsDarkTheme = Application.Current.Resources.Source.ToString().Contains("dark") ? false : true;
            this.MenuVisibility = "Collapsed";

            // Set starting views
            this.CurrentViewLeft = this.songViewTemplate;
            this.CurrentViewRight = this.infoView;

            // subscribe to event
            this.EventAggregator.GetEvent<ChangedSongViewDataEvent>().Subscribe(this.CreateNewSongView, ThreadOption.UIThread);

            Application.Current.MainWindow.Closing += new CancelEventHandler(this.ProgramClosing);
        }
        #endregion

        #region ------------------------- Properties, Indexers ----------------------------------------------

        public ICommand SongViewCommand { get; private set; }
        public ICommand InfoViewCommand { get; private set; }
        public ICommand ToggleThemeCommand { get; private set; }
        public ICommand ChooseDirectoryCommand { get; private set; }
        public ICommand ToggleMenuCommand { get; private set; }
        public ICommand EditSelectionCommand { get; private set; }
        public ICommand SaveChangesCommand { get; private set; }

        /// <summary>
        /// Gets or sets the view that is currently bound to the <see cref="ContentControl"/> left.
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

        /// <summary>
        /// Gets or sets the view that is currently bound to the <see cref="ContentControl"/> right.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the boolean value for the darktheme.
        /// </summary>
        public bool IsDarkTheme
        {
            get { return this.isDarkTheme; }
            set
            {
                if (this.isDarkTheme != value)
                { this.isDarkTheme = value; }
            }
        }

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
        /// Gets or sets the boolean for if the editselection button can be executed.
        /// </summary>
        public bool EditSelectionCanExecute
        {
            get
            {
                return this.editSelectionCanExecute;
            }
            set
            {
                if (this.editSelectionCanExecute != value)
                {
                    this.editSelectionCanExecute = value;
                    OnPropertyChanged(nameof(this.editSelectionCanExecute));
                }
            }
        }

        #endregion

        #region ------------------------- Private helper ----------------------------------------------------
        /// <summary>
        /// Creates a new instance of the song view and sets up the associated view model.
        /// </summary>
        /// <param name="dir">The directory for the songs.</param>
        private void CreateNewSongView(string dir)
        {
            // Initialize the song view and song view model
            this.songView = new SongView();
            this.songViewModel = new SongViewModel(this.EventAggregator, dir);
            songView.DataContext = songViewModel;

            this.CurrentViewLeft = this.songView;
            this.editSelectionCanExecute = true;
        }

        /// <summary>
        /// Changes the application theme.
        /// </summary>
        /// <param name="theme">The theme to apply. Can be "light" or "dark".</param>
        private void ChangeTheme(string theme)
        {
            theme = "ResourceDictionaries/" + theme + ".xaml";
            Application.Current.Resources.Source = new Uri(theme, UriKind.RelativeOrAbsolute);
            this.OnPropertyChanged(nameof(Application.Current.Resources.Source));
        }

        /// <summary>
        /// The EditWindow_Closing method sets the EditSelectionCanExecute of the EditSelection button to true so that the button is active when the EditSelection window is closed.
        /// </summary>
        private void EditWindow_Closing(object sender, CancelEventArgs e)
        {
            this.EditSelectionCanExecute = true;

            //set the mainwindow to be in the foreground
            var mainWindow = Application.Current.MainWindow;
            mainWindow.WindowState = WindowState.Normal;
            mainWindow.Show();
            mainWindow.Activate();
        }

        /// <summary>
        /// The ProgramClosing method handels what happens when the program is being closed from the main window.
        /// </summary>
        void ProgramClosing(object sender, CancelEventArgs e)
        {
            // Add saving the songs to the database

            // This closes the entire program
            Environment.Exit(0);
        }
        #endregion

        #region ------------------------- Commands ----------------------------------------------------------
        #region SongViewCommand
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
        /// Occures when the user clicks the song view button.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        private void SongViewCommandExecute(object parameter)
        {
            this.CurrentViewLeft = this.CurrentViewLeft == null ? this.songView : null;
        }
        #endregion

        #region InfoViewCommand
        /// <summary>
        /// Determines whether the InfoViewCommand can be executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns>True if the command can be executed, false otherwise.</returns>
        private bool InfoViewCommandCanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Executes the InfoViewCommand.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void InfoViewCommandExecute(object parameter)
        {
            this.CurrentViewRight = this.CurrentViewRight == null ? this.infoView : null;
        }
        #endregion

        #region ToggleThemeCommand
        /// <summary>
        /// Determines whether the ToggleThemeCommand can be executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns>True if the command can be executed, false otherwise.</returns>
        private bool ToggleThemeCommandCanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Executes the ToggleThemeCommand.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
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
        #endregion

        #region ChooseDirectoryCommand
        /// <summary>
        /// Determines whether the ChooseDirectoryCommand can be executed.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns>True if the command can be executed, false otherwise.</returns>
        private bool ChooseDirectoryCommandCanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Executes the ChooseDirectoryCommand.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        private void ChooseDirectoryCommandExecute(object parameter)
        {
            if (ChangeDirectoryService.ChangeDirectory(out string directory))
            {
                this.EventAggregator.GetEvent<ChangedSongViewDataEvent>().Publish(directory);
            }
        }
        #endregion

        #region ToggleMenuCommand
        /// <summary>
        /// Determines whether the ToggleMenuCommand can be executed.
        /// </summary>
        /// <param name="arg">The command parameter.</param>
        /// <returns>True if the command can be executed, false otherwise.</returns>
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

        #region EditSelectionCommand
        /// <summary>
        /// Determines whether the EditSelectionCommand can be executed.
        /// </summary>
        /// <param name="arg">The command parameter.</param>
        /// <returns>True if the command can be executed, false otherwise.</returns>
        private bool EditSelectionCommandCanExecute(object arg)
        {
            if (this.editSelectionCanExecute)
                return true;

            return false;
        }

        /// <summary>
        /// Executes the EditSelectionCommand.
        /// </summary>
        /// <param name="obj">The command parameter.</param>
        private void EditSelectionCommandExecute(object obj)
        {
            if (this.songViewModel.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select something first!");
            }
            else
            {
                this.editWindowView = new EditWindowView();
                this.editWindowViewModel = new EditWindowViewModel(this.EventAggregator, this.songViewModel.SelectedItems);
                this.editWindowView.DataContext = editWindowViewModel;
                this.editWindowView.Show();

                this.editSelectionCanExecute = false;

                // Subscribe to the other window closing event
                var editWindow = Application.Current.Windows.OfType<EditWindowView>().FirstOrDefault();

                if (editWindow != null)
                    editWindow.Closing += EditWindow_Closing;
            }
        }
        #endregion

        #region SaveChangesCommand
        /// <summary>
        /// Determines whether the SaveChangesCommand can be executed.
        /// </summary>
        /// <param name="arg">The command parameter.</param>
        /// <returns>True.</returns>
        private bool SaveChangesCommandCanExecute(object arg)
        {
            return true;
        }

        /// <summary>
        /// Executes the SaveChangesCommand.
        /// </summary>
        /// <param name="obj">The command parameter.</param>
        private void SaveChangesCommandExecute(object obj)
        {
            SaveSongData.SaveChanges(this.songViewModel.Songs.ToList());
        }
        #endregion

        #endregion
    }
}