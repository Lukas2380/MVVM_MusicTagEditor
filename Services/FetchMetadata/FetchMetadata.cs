using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using Services.BitMapImageHelperMethods;


namespace Services.FetchMetadata
{
    public class FetchMetadata
    {
        private HttpClient client;

        private string song;
        private string artist;
        private int? releaseYear;
        private string album;
        private string lyrics;
        private BitmapImage albumCover = null;
        private string coverUrl;
        private List<string> websites = new List<string>();

        private string songUrl;

        private JObject json;
        private HtmlDocument songWebPage;
        private HttpResponseMessage response;
        private string responseBody;

        private string geniusApiKey = "I5zWaPrUJXX6LVtScITg_wpvZnQ_wZnZ-TSizJge8pEwhZoKvU6bx4ecJOko9jrn";

        public string Song
        {
            get { return this.song; }
            private set { this.song = value; }
        }
        public string Artist
        {
            get { return this.artist; }
            private set { this.artist = value; }
        }
        public int? ReleaseYear
        {
            get { return this.releaseYear; }
            private set { this.releaseYear = value; }
        }
        public string Album
        {
            get { return this.album; }
            private set { this.album = value; }
        }
        public string Lyrics
        {
            get { return this.lyrics; }
            private set { this.lyrics = value; }
        }
        public BitmapImage AlbumCover
        {
            get { return this.albumCover; }
            set { this.albumCover = value; }
        }
        public string CoverUrl
        {
            get
            {
                return this.coverUrl;
            }
            set
            {
                this.coverUrl = value;
            }
        }
        public List<string> Websites
        {
            get
            {
                return this.websites;
            }
            set
            {
                if(this.websites != value)
                    this.websites = value;
            }
        }

        public FetchMetadata(string artist, string song)
        {
            this.artist = artist;
            this.song = song;
            this.songUrl = "https://api.genius.com/search?q=" + artist + song;

            this.Websites.Add("https://www.google.com/search?q=" + song + "+" + artist);
            FetchMetadataInformationAsync();
        }

        private async Task FetchMetadataInformationAsync()
        {
            FetchJsonResponseBody();
            FetchInitialTags();
            FetchSongWebPage();
            FetchReleaseYear();
            FetchAlbum();
            FetchLyrics();
            await FetchAlbumCover();
        }

        private async Task FetchAlbumCover()
        {
            AlbumCover = await BitMapImageHelper.CreateBitmapImageFromUrl(CoverUrl);
        }


        private void FetchSongWebPage()
        {
            response = client.GetAsync(songUrl).Result;
            responseBody = response.Content.ReadAsStringAsync().Result;
            songWebPage = new HtmlDocument();
            songWebPage.LoadHtml(responseBody);
        }

        private void FetchInitialTags()
        {
            try
            {
                Artist = json["response"]["hits"][0]["result"]["artist_names"].ToString();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Cant fetch artist: " + ex.Message);
            }

            try
            {
                Song = json["response"]["hits"][0]["result"]["title"].ToString();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Cant fetch songname: " + ex.Message);
            }

            try
            {
                songUrl = json["response"]["hits"][0]["result"]["url"].ToString();
                this.Websites.Add(songUrl);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Cant fetch url: " + ex.Message);
            }

            try
            {
                CoverUrl = json["response"]["hits"][0]["result"]["header_image_url"].ToString();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Failed to create Bitmap from URL: " + ex.Message);
            }
        }

        private void FetchJsonResponseBody()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", geniusApiKey);
            response = client.GetAsync(songUrl).Result;
            string responseBody = response.Content.ReadAsStringAsync().Result;
            json = JObject.Parse(responseBody);
        }

        private void FetchReleaseYear()
        {
            try
            {
                ReleaseYear = int.Parse(json["response"]["hits"][0]["result"]["release_date_components"]["year"].ToString());
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Cant fetch year: " + ex.Message);
                ReleaseYear = null;
            }
        }

        private void FetchAlbum()
        {
            try
            {
                var albumNode = songWebPage.DocumentNode.SelectSingleNode("//div[@class='HeaderArtistAndTracklistdesktop__Tracklist-sc-4vdeb8-2 glZsJC']/a/text()");
                if (albumNode != null)
                {
                    Album = albumNode.InnerText;
                }
                else
                {
                    // Handle the case when the album node is not found
                    //MessageBox.Show("Album not found");
                }
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void FetchLyrics()
        {
            List<HtmlNode> lyricNodes = songWebPage.DocumentNode.SelectNodes("//div[@class='Lyrics__Container-sc-1ynbvzw-5 Dzxov']").ToList();

            if (lyricNodes != null)
            {
                StringBuilder lyricsBuilder = new StringBuilder();

                foreach (HtmlNode node in lyricNodes)
                {
                    string lyrics = node.InnerHtml.Trim();
                    lyrics = lyrics.Replace("<br>", "\n");
                    lyrics = WebUtility.HtmlDecode(lyrics);
                    lyrics = Regex.Replace(lyrics, "<.*?>", String.Empty);
                    lyrics = lyrics.Replace("[Intro]", "");
                    lyrics = Regex.Replace(lyrics, "\\n{0,1}\\[[^\\]]*\\]", String.Empty);
                    lyricsBuilder.AppendLine(lyrics);
                }

                Lyrics = lyricsBuilder.ToString().Trim();
            }
            else
            {
                MessageBox.Show("No lyrics found");
                Lyrics = "";
            }
        }
    }

}
