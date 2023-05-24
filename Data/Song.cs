using Common.NotifyPropertyChanged;
using System;
using System.Linq;
using System.Security.Policy;
using System.Windows;
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
        private string genre;
        private string lyrics;
        private string lyricist;
        private string fileLocation;

        #endregion

        #region ------------------------- Constructors, Destructors, Dispose, Clone ---------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Song"/> class.
        /// </summary>
        /// <param name="title">Title of the song.</param>
        /// <param name="artists">Array of artists involved in the song.</param>
        /// <param name="albumName">Name of the album the song belongs to.</param>
        /// <param name="year">Year when the song was released. Can be null.</param>
        /// <param name="albumCover">Image of the album cover.</param>
        /// <param name="genre">Genre of the song.</param>
        /// <param name="lyrics">Lyrics of the song.</param>
        /// <param name="lyricist">Lyricist of the song.</param>
        public Song(string title, string[] artists, string albumName, int? year, BitmapImage albumCover, string genre, string lyrics, string lyricist, string fileLocation)
        {
            this.title = title;
            this.artists = artists;
            this.albumName = albumName;
            this.year = year;
            this.albumCover = albumCover;
            this.genre = genre;
            this.lyrics = lyrics;
            this.lyricist = lyricist;
            this.fileLocation = fileLocation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Song"/> class with default values.
        /// </summary>
        public Song()
        {
            this.title = "";
            this.artists = null;
            this.albumName = "";
            this.year = null;
            this.albumCover = null;
            this.genre = "";
            this.lyrics = "";
            this.lyricist = "";
        }
        #endregion

        #region ------------------------- Properties, Indexers ------------------------------------------------------------

        /// <summary>
        /// Gets or sets the title of the song.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the artists of a song.
        /// </summary>
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

        //public string SongDisplay
        //{
        //    get
        //    {
        //        return this.Title + " - " + StringArrayToString(this.artists);
        //    }
        //}

        /// <summary>
        /// Gets or sets the albumname of a song.
        /// </summary>
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
        
        /// <summary>
        /// Gets or sets the year of a song. 
        /// The int? means that the integer can be zero.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the albumcover as a <see cref="BitmapImage"/> of a song.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the genre of a song.
        /// </summary>
        public string Genre
        {
            get => this.genre;
            set
            {
                if (this.genre != value)
                {
                    this.genre = value;
                    OnPropertyChanged(nameof(this.genre));
                }
            }
        }

        /// <summary>
        /// Gets or sets the lyrics of a song.
        /// </summary>
        public string Lyrics
        {
            get => this.lyrics;
            set
            {
                if (this.lyrics != value)
                {
                    this.lyrics = value;
                    OnPropertyChanged(nameof(this.lyrics));
                }
            }
        }

        /// <summary>
        /// Gets or sets the lyricist of a song.
        /// </summary>
        public string Lyricist
        {
            get => this.lyricist;
            set
            {
                if (this.lyricist != value)
                {
                    this.lyricist = value;
                    OnPropertyChanged(nameof(this.lyricist));
                }
            }
        }

        public string FileLocation
        {
            get => this.fileLocation;
            set
            {
                if (this.fileLocation != value)
                {
                    this.fileLocation = value;
                    OnPropertyChanged(nameof(this.fileLocation));
                }
            }
        }
        #endregion

        #region ------------------------- Private helper ------------------------------------------------------------------
        /// <summary>
        /// Converts an array of strings to a single string.
        /// </summary>
        /// <param name="strArr">The array of strings to be converted.</param>
        /// <returns>A string of the array, or null if the array is null.</returns>
        public string StringArrayToString(string[] strArr)
        {
            string str = "";

            if (strArr != null)
            {
                foreach (var s in strArr)
                {
                    if (s == null)
                        return null;
                    str += s;
                }
            }

            return str;
        }
        #endregion
    }
}
