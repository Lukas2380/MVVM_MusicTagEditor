using ATL;
using ATL.AudioData;
using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Services.SongData
{
    public class SaveSongData
    {
        public static List<Song> EditedSongs { get; set; } = new List<Song>();

        public static void SaveChanges()
        {
            BackgroundWorker saveWorker = new BackgroundWorker();
            saveWorker.DoWork += SaveWorker_DoWork;
            saveWorker.RunWorkerCompleted += SaveWorker_RunWorkerCompleted;

            // Start the background worker to save the changes
            saveWorker.RunWorkerAsync();
        }

        private static void SaveWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int totalSongs = EditedSongs.Count;
            int processedSongs = 0;

            foreach (var song in EditedSongs)
            {
                Track mp3 = null;
                try
                {
                    mp3 = new Track(song.FileLocation);
                    mp3.Remove(MetaDataIOFactory.TagType.ID3V1);
                    mp3.Remove(MetaDataIOFactory.TagType.ID3V2);

                    mp3.Title = song.Title;
                    mp3.Artist = song.Artists;
                    mp3.Album = song.AlbumName;
                    mp3.Year = song.Year;
                    mp3.Genre = song.Genre;
                    mp3.Lyrics.UnsynchronizedLyrics = song.Lyrics;
                    mp3.AdditionalFields.Add("text", song.Lyricist);

                    // Clear existing pictures
                    mp3.EmbeddedPictures.Clear();

                    // Add album cover picture
                    if (song.AlbumCover != null)
                    {
                        var pictureInfo = ATL.PictureInfo.fromBinaryData(ConvertBitmapImageToByteArray(song.AlbumCover));
                        mp3.EmbeddedPictures.Add(pictureInfo);
                    }

                    mp3.Save();
                }
                finally
                {
                    //mp3?.Dispose();
                }

                processedSongs++;

                // Report progress if needed
                // double progressPercentage = (double)processedSongs / totalSongs * 100;
                // (sender as BackgroundWorker)?.ReportProgress((int)progressPercentage);
            }
        }

        private static void SaveWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Perform any necessary actions upon completion
        }

        public static byte[] ConvertBitmapImageToByteArray(BitmapImage bitmapImage)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(stream);
                return stream.ToArray();
            }
        }
    }

}
