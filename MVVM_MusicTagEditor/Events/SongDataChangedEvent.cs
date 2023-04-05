using Data;
using Microsoft.Practices.Prism.Events;


namespace MVVM_MusicTagEditor.Events
{
    /// <summary>
    /// Event signalizes to add new Song data
    /// </summary>
    public class SongDataChangedEvent : CompositePresentationEvent<Song>
    {

    }
}
