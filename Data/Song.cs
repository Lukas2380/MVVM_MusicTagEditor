using Common.NotifyPropertyChanged;
using System.Windows.Media.Imaging;

namespace Data
{
    public class Song : NotifyPropertyChanged
    {
        #region Fields

        private string title;
        private string albumName;
        private BitmapImage albumCover;

        #endregion

        #region Constructor

        public Song(string title, string albumName, BitmapImage albumCover)
        {
            this.title = title;
            this.albumName = albumName;
            this.albumCover = albumCover;
        }

        #endregion

        #region Properties

        public string Title
        {
            get => title;
            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged(nameof(this.title));
                }
            }
        }

        public string AlbumName
        {
            get => albumName;
            set
            {
                if (albumName != value)
                {
                    albumName = value;
                    OnPropertyChanged(nameof(this.albumName));
                }
            }
        }

        public BitmapImage AlbumCover
        {
            get => albumCover;
            set
            {
                if (albumCover != value)
                {
                    albumCover = value;
                    OnPropertyChanged(nameof(this.albumCover));
                }
            }
        }


        #endregion

        #region ------------------------- Private helper ------------------------------------------------------------------
        //public override string ToString()
        //{
        //    return this.Title;
        //}
        #endregion

        #region ------------------------- Commands ------------------------------------------------------------------------
        #endregion
    }
}
