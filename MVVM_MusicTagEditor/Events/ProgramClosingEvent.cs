using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_MusicTagEditor.Events
{
    /// <summary>
    /// Event signalizes that the the program is closing.
    /// </summary>
    public class ProgramClosingEvent : CompositePresentationEvent<string>
    {

    }
}
