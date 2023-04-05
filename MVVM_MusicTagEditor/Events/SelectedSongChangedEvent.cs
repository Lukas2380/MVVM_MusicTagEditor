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
    /// Event signalizes to add new Song data
    /// </summary>
    public class SelectedSongChangedEvent : CompositePresentationEvent<Song>
    {

    }
}
