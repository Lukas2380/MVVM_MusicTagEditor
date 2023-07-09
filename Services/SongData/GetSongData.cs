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
using ATL;

namespace Services.SongData
{
    public class GetSongData
    {
        /// <summary>
        /// Gets the tag for a mp3 file and checks if there is an album cover.
        /// Returns an empty string if there is no album cover found.
        /// </summary>
        /// <returns>Returns the album cover as a BitmapImage. </returns>
        //public static BitmapImage GetAlbumCover(string songPath)
        //{
        //    var coverData = tag.Pictures.FirstOrDefault();
        //    BitmapImage coverImage = null;

        //    if (coverData != null)
        //    {
        //        try
        //        {
        //            using (var ms = new MemoryStream(coverData.PictureData))
        //            {
        //                coverImage = BitMapImageHelper.ToBitmapImage(new Bitmap(ms));
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //    else
        //    {
        //        Bitmap bitmap = BitMapImageHelper.ConvertToBitmap("..\\..\\..\\img\\mp3.png");
        //        coverImage = BitMapImageHelper.ToBitmapImage(bitmap);
        //    }
        //    return coverImage;
        //}
    }
}
