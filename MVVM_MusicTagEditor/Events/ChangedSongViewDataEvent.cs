using Data;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_MusicTagEditor.Events
{
    /// <summary>
    /// Event signalizes that the data in SongView has changed.
    /// </summary>
    public class ChangedSongViewDataEvent : CompositePresentationEvent<string>
    {
        
    }
}
