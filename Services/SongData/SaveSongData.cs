using Data;
using Id3;
using Id3.Frames;
using Id3.InfoFx;
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
        public static void SaveChanges(List<Song> songs)
        {
            BackgroundWorker saveWorker = new BackgroundWorker();
            saveWorker.DoWork += SaveWorker_DoWork;
            saveWorker.RunWorkerCompleted += SaveWorker_RunWorkerCompleted;

            // Start the background worker to save the changes
            saveWorker.RunWorkerAsync(songs);
        }

        private static void SaveWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<Song> songs = e.Argument as List<Song>;

            int totalSongs = songs.Count();
            int processedSongs = 0;

            foreach (var song in songs)
            {
                using (var mp3 = new Mp3(song.FileLocation, Mp3Permissions.ReadWrite))
                {
                    Id3Tag tag = mp3.GetTag(Id3TagFamily.Version2X);

                    tag.Title = song.Title;

                    tag.Artists.Value.Clear();
                    tag.Artists.Value.Add(song.Artists);

                    tag.Album = song.AlbumName;
                    tag.Year = song.Year;

                    //// Create a new picture frame with the album cover
                    //var pictureFrame = new PictureFrame
                    //{
                    //    PictureData = ConvertBitmapImageToByteArray(song.AlbumCover),
                    //    MimeType = "image/jpeg", // Modify the MIME type if necessary
                    //    PictureType = PictureType.FrontCover // Modify the picture type if necessary
                    //};

                    //tag.Pictures.Clear();
                    //tag.Pictures.Add(pictureFrame);

                    tag.Genre = song.Genre;

                    tag.Lyrics.Clear();
                    tag.Lyrics.Add(song.Lyrics);

                    tag.Lyricists.Value.Clear();
                    tag.Lyricists.Value.Add(song.Lyricist);

                    // Neither of them work with V2, nothing I can do

                    mp3.WriteTag(tag, Id3Version.V1X, WriteConflictAction.Replace);









                    //mp3.UpdateTag(tag);
                }

                processedSongs++;

                // Report progress if needed
                //double progressPercentage = (double)processedSongs / totalSongs * 100;
                //(sender as BackgroundWorker)?.ReportProgress((int)progressPercentage);
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
