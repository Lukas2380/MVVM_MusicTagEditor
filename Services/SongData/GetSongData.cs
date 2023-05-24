using Id3;
using Services.BitMapImageHelperMethods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Services.SongData
{
    public class GetSongData
    {
        /// <summary>
        /// The GetGenre method calls the Genres.ToString method to retrieve the string for the genre number in the mp3 file.
        /// </summary>
        /// <param name="tag">Id3Tag from an mp3 file. </param>
        /// <returns>Returns the genre as a string. </returns>
        public static string GetGenre(Id3Tag tag)
        {
            return Genres.ToString(tag);
        }

        /// <summary>
        /// Gets the tag for a mp3 file and checks if there are lyrics.
        /// Returns an empty string if there are no lyrics found.
        /// </summary>
        /// <param name="tag">Id3Tag from an mp3 file. </param>
        /// <returns>Returns the lyrics as a string. </returns>
        public static string GetLyrics(Id3Tag tag)
        {
            var lyricsData = tag.Lyrics.FirstOrDefault();
            string lyrics = "";

            if (lyricsData != null)
            {
                lyrics = lyricsData.Lyrics;
            }
            return lyrics;
        }


        /// <summary>
        /// Gets the tag for a mp3 file and checks if there is an album cover.
        /// Returns an empty string if there is no album cover found.
        /// </summary>
        /// <param name="tag">Id3Tag from an mp3 file. </param>
        /// <returns>Returns the album cover as a BitmapImage. </returns>
        public static BitmapImage GetAlbumCover(Id3Tag tag)
        {
            var coverData = tag.Pictures.FirstOrDefault();
            BitmapImage coverImage = null;

            if (coverData != null)
            {
                try
                {
                    using (var ms = new MemoryStream(coverData.PictureData))
                    { 
                        coverImage = BitMapImageHelper.ToBitmapImage(new Bitmap(ms));
                    }
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                Bitmap bitmap = BitMapImageHelper.ConvertToBitmap("..\\..\\..\\img\\mp3.png");
                coverImage = BitMapImageHelper.ToBitmapImage(bitmap);
            }
            return coverImage;
        }
    }
}
