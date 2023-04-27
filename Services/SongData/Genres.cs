using Id3;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Services.SongData
{
    public class Genres
    {
        private static string[] genre = new string[]
        {
            "Blues",
            "Classic Rock",
            "Country",
            "Dance",
            "Disco",
            "Funk",
            "Grunge",
            "Hip-Hop",
            "Jazz",
            "Metal",
            "New Age",
            "Oldies",
            "Other",
            "Pop",
            "R&B",
            "Rap",
            "Reggae",
            "Rock",
            "Techno",
            "Industrial",
            "Alternative",
            "Ska",
            "Death Metal",
            "Pranks",
            "Soundtrack",
            "Euro-Techno",
            "Ambient",
            "Trip-Hop",
            "Vocal",
            "Jazz+Funk",
            "Fusion",
            "Trance",
            "Classical",
            "Instrumental",
            "Acid",
            "House",
            "Game",
            "Sound Clip",
            "Gospel",
            "Noise",
            "Alt. Rock",
            "Bass",
            "Soul",
            "Punk",
            "Space",
            "Meditative",
            "Instrumental Pop",
            "Instrumental Rock",
            "Ethnic",
            "Gothic",
            "Darkwave",
            "Techno-Industrial",
            "Electronic",
            "Pop-Folk",
            "Eurodance",
            "Dream",
            "Southern Rock",
            "Comedy",
            "Cult",
            "Gangsta Rap",
            "Top 40",
            "Christian Rap",
            "Pop/Funk",
            "Jungle",
            "Native American",
            "Cabaret",
            "New Wave",
            "Psychedelic",
            "Rave",
            "Showtunes",
            "Trailer",
            "Lo-Fi",
            "Tribal",
            "Acid Punk",
            "Acid Jazz",
            "Polka",
            "Retro",
            "Musical",
            "Rock & Roll",
            "Hard Rock",
            "Folk",
            "Folk-Rock",
            "National Folk",
            "Swing",
            "Fast-Fusion",
            "Bebop",
            "Latin",
            "Revival",
            "Celtic",
            "Bluegrass",
            "Avantgarde",
            "Gothic Rock",
            "Progressive Rock",
            "Psychedelic Rock",
            "Symphonic Rock",
            "Slow Rock",
            "Big Band",
            "Chorus",
            "Easy Listening",
            "Acoustic",
            "Humour",
            "Speech",
            "Chanson",
            "Opera",
            "Chamber Music",
            "Sonata",
            "Symphony",
            "Booty Bass",
            "Primus",
            "Porn Groove",
            "Satire",
            "Slow Jam",
            "Club",
            "Tango",
            "Samba",
            "Folklore",
            "Ballad",
            "Power Ballad",
            "Rhythmic Soul",
            "Freestyle",
            "Duet",
            "Punk Rock",
            "Drum Solo",
            "A Cappella",
            "Euro-House",
            "Dance Hall",
            "Goa",
            "Drum & Bass",
            "Club-House",
            "Hardcore",
            "Terror",
            "Indie",
            "BritPop",
            "Afro-Punk",
            "Polsk Punk",
            "Beat",
            "Christian Gangsta Rap",
            "Heavy Metal",
            "Black Metal",
            "Crossover",
            "Contemporary Christian",
            "Christian Rock",
            "Merengue",
            "Salsa",
            "Thrash Metal",
            "Anime",
            "JPop",
            "Synthpop",
            "Abstract",
            "Art Rock",
            "Baroque",
            "Bhangra",
            "Big Beat",
            "Breakbeat",
            "Chillout",
            "Downtempo",
            "Dub",
            "EBM",
            "Eclectic",
            "Electro",
            "Electroclash",
            "Emo",
            "Experimental",
            "Garage",
            "Global",
            "IDM",
            "Illbient",
            "Industro-Goth",
            "Jam Band",
            "Krautrock",
            "Leftfield",
            "Lounge",
            "Math Rock",
            "New Romantic",
            "Nu-Breakz",
            "Post-Punk",
            "Post-Rock",
            "Psytrance",
            "Shoegaze",
            "Space Rock",
            "Trop Rock",
            "World Music",
            "Neoclassical",
            "Audiobook",
            "Audio Theatre",
            "Neue Deutsche Welle",
            "Podcast",
            "Indie Rock",
            "G-Funk",
            "Dubstep",
            "Garage Rock",
            "Psybient",
            "None"
        };

        /// <summary>
        /// This ToString method gets the tags of one mp3 file and returns the correlating string for the genre.
        /// This is necessary because the genre is saved as a number in the file. Hence the array of all the genres.
        /// </summary>
        /// <param name="tag">The Id3Tag from a mp3 file.</param>
        /// <returns>Returns the string of the genre.</returns>
        public static string ToString(Id3Tag tag)
        {
            string genreName = tag.Genre.ToString();
            genreName = GetActualString(genreName);
            return genreName;
        }

        /// <summary>
        /// The GetActualString method is for converting the genre from the mp3 file to the coorilating strign for it.
        /// Some mp3 genres are saved like this: (66).
        /// It can be converted to the string via getting the value of the genre array on the position of the genre "66".
        /// </summary>
        /// <param name="genreName"></param>
        /// <returns></returns>
        private static string GetActualString(string genreName)
        {
            Regex rx = new Regex(@"[(]-?\d+[)]");
            if (!rx.IsMatch(genreName))
                return genreName;

            genreName = genreName.Replace("(", "").Replace(")", "");
            int nr = int.Parse(genreName);
            return genre[nr];
        }
    }
}
