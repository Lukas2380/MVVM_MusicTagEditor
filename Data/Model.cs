using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Data
{
    public class OriginalSong
    {
        public int Id { get; set; } // Primary key

        public string Title { get; set; }
        public string Artists { get; set; }
        public string AlbumName { get; set; }
        public int? Year { get; set; }
        public BitmapImage AlbumCover { get; set; }
        public string Genre { get; set; }
        public string Lyrics { get; set; }
        public string Lyricist { get; set; }
    }

    public class UpdatedSong
    {
        public int Id { get; set; } // Primary key

        public string Title { get; set; }
        public string[] Artists { get; set; }
        public string AlbumName { get; set; }
        public int? Year { get; set; }
        public BitmapImage AlbumCover { get; set; }
        public string Genre { get; set; }
        public string Lyrics { get; set; }
        public string Lyricist { get; set; }
    }
}
