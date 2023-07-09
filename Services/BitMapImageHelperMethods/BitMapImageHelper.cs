using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Services.BitMapImageHelperMethods
{
    public class BitMapImageHelper
    {
        public static BitmapImage ConvertImageToBitmapImage(byte[] imageData)
        {
            //using (MemoryStream memoryStream = new MemoryStream(imageData))
            //{
            //    using (Image image = Image.FromStream(memoryStream))
            //    {
            //        Bitmap bitmap = new Bitmap(image);
            //        BitmapImage bitmapImage = new BitmapImage();

            //        using (MemoryStream bitmapStream = new MemoryStream())
            //        {
            //            bitmap.Save(bitmapStream, ImageFormat.Png);
            //            bitmapStream.Position = 0;

            //            bitmapImage.BeginInit();
            //            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            //            bitmapImage.StreamSource = bitmapStream;
            //            bitmapImage.EndInit();
            //            bitmapImage.Freeze();
            //        }

            //        return bitmapImage;
            //    }
            //}
            BitmapSource bitmapSource = Clipboard.GetImage();

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            MemoryStream memoryStream = new MemoryStream();
            BitmapImage bImg = new BitmapImage();

            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            encoder.Save(memoryStream);

            memoryStream.Position = 0;
            bImg.BeginInit();
            bImg.StreamSource = memoryStream;
            bImg.EndInit();

            memoryStream.Close();
            return bImg;
        }


        /// <summary>
        /// The ConvertToBitmap method takes a filename from an image and creates a Bitmap for it.
        /// </summary>
        /// <param name="fileName">The path to the image.</param>
        /// <returns>Returns the converted Bitmap</returns>
        public static Bitmap ConvertToBitmap(string fileName)
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
        public static BitmapImage ToBitmapImage(Bitmap bitmap)
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

        /// <summary>
        /// The CreateBitmapImageFromUrl method creates a bitmapimage from a url.
        /// </summary>
        /// <param name="url">The image url.</param>
        /// <returns>A bitmap image.</returns>
        public static BitmapImage CreateBitmapImageFromUrl(string url)
        {
            try
            {
                using (var webClient = new WebClient())
                {
                    byte[] imageData = webClient.DownloadData(url);
                    using (var stream = new MemoryStream(imageData))
                    {
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.StreamSource = stream;
                        bitmapImage.EndInit();
                        bitmapImage.Freeze();

                        // Use the bitmapImage as needed
                        return bitmapImage;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception
                Console.WriteLine("Error downloading image data: " + ex.Message);
                return null;
            }
        }

    }
}
