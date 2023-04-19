using Common.NotifyPropertyChanged;
using System;
using System.Linq;
using System.Windows.Media.Imaging;

namespace Data
{
    public class Song : NotifyPropertyChanged
    {
        #region ------------------------- Fields, Constants, Delegates, Events --------------------------------------------

        private string title;
        private string[] artists;
        private string albumName;
        private int? year;
        private BitmapImage albumCover;
        private BitmapSource albumCoversource;
        private string lyrics;
        private string lyricist;

        #endregion

        #region ------------------------- Constructors, Destructors, Dispose, Clone ---------------------------------------

        public Song(string title, string[] artists, string albumName, int? year, BitmapImage albumCover, string lyrics, string lyricist)
        {
            this.title = title;
            this.artists = artists;
            this.albumName = albumName;
            this.year = year;
            this.albumCover = albumCover;
            this.lyrics = lyrics;
            this.lyricist = lyricist;
        }
        #endregion

        #region ------------------------- Properties, Indexers ------------------------------------------------------------

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                if (this.title != value)
                {
                    this.title = value;
                    OnPropertyChanged(nameof(this.title));
                }
            }
        }
        public string Artists
        {
            get
            {
                return StringArrayToString(this.artists);
            }
            set
            {
                if (StringArrayToString(this.artists) != value)
                {
                    this.artists = value.Split(',');
                    OnPropertyChanged(nameof(this.artists));
                }
            }
        }

        public string SongDisplay
        {
            get
            {
                return this.Title + " - " + StringArrayToString(this.artists);
            }
        }

        public string AlbumName
        {
            get
            {
                return this.albumName;
            }
            set
            {
                if (this.albumName != value)
                {
                    this.albumName = value;
                    OnPropertyChanged(nameof(this.albumName));
                }
            }
        }
        
        public int? Year
        {
            get
            {
                return this.year;
            }
            set
            {
                if (this.year != value)
                {
                    this.year = value;
                    OnPropertyChanged(nameof(this.year));
                }
            }
        }

        public BitmapImage AlbumCover
        {
            get
            {
                return this.albumCover;
            }
            set
            {
                if (this.albumCover != value)
                {
                    this.albumCover = value;
                    OnPropertyChanged(nameof(this.albumCover));
                }
            }
        }

        public BitmapSource AlbumCoversource
        {
            get
            {
                return this.albumCoversource;
            }
            set
            {
                if (this.albumCoversource != value)
                {
                    this.albumCoversource = value;
                    OnPropertyChanged(nameof(this.albumCoversource));
                }
            }
        }

        public string Lyrics
        {
            get => lyrics;
            set
            {
                if (this.lyrics != value)
                {
                    this.lyrics = value;
                    OnPropertyChanged(nameof(this.lyrics));
                }
            }
        }

        public string Lyricist
        {
            get => lyricist;
            set
            {
                if (this.lyricist != value)
                {
                    this.lyricist = value;
                    OnPropertyChanged(nameof(this.lyricist));
                }
            }
        }
        #endregion

        #region ------------------------- Private helper ------------------------------------------------------------------
        //public override string ToString()
        //{
        //    return this.Title;
        //}

        public string StringArrayToString(string[] strArr)
        {
            string str = "";
            foreach (var s in strArr)
            {
                if (s == null)
                    return null;
                str += s;
            }
            return str;
        }
        #endregion

        #region ------------------------- Commands ------------------------------------------------------------------------
        #endregion
    }
}
