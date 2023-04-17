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
        private int year;
        private BitmapImage albumCover;
        private BitmapSource albumCoversource;
        private string lyrics;

        #endregion

        #region ------------------------- Constructors, Destructors, Dispose, Clone ---------------------------------------

        public Song(string title, string[] artists, string albumName, int year, BitmapImage albumCover, string lyrics)
        {
            this.title = title;
            this.artists = artists;
            this.albumName = albumName;
            this.year = year;
            this.albumCover = albumCover;
            this.lyrics = lyrics;
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
        
        public string Year
        {
            get
            {
                return this.year.ToString();
            }
            set
            {
                int input = 0;
                try
                {
                    input = Convert.ToInt32(value);
                    if (this.year != input)
                    {
                        this.albumName = value;
                        OnPropertyChanged(nameof(this.albumName));
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Input string is not a sequence of digits.");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("The number cannot fit in an Int32.");
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
