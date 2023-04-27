using Id3;
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
                        coverImage = ToBitmapImage(new Bitmap(ms));
                    }
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                Bitmap bitmap = ConvertToBitmap("..\\..\\..\\img\\mp3.png");
                coverImage = ToBitmapImage(bitmap);
            }
            return coverImage;
        }

        /// <summary>
        /// The ConvertToBitmap method takes a filename from an image and creates a Bitmap for it.
        /// </summary>
        /// <param name="fileName">The path to the image.</param>
        /// <returns>Returns the converted Bitmap</returns>
        private static Bitmap ConvertToBitmap(string fileName)
        {
            Bitmap bitmap;
            // here some error
            using (Stream bmpStream = System.IO.File.Open(fileName, System.IO.FileMode.Open))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(bmpStream);
                bitmap = new Bitmap(image);
            }
            return bitmap;
        }

        /// <summary>
        /// The ToBitmapImage method converts a Bitmap to a BitmapImage. (This is a helper method from the internet)
        /// </summary>
        /// <param name="bitmap">The bitmap that is to be converted to a BitmapImage</param>
        /// <returns>Returns the converted BitmapImage</returns>
        private static BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg); // Was .Bmp, but this did not show a transparent background.
                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }
    }
}
