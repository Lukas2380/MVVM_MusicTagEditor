using Common.NotifyPropertyChanged;
using Microsoft.Practices.Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM_MusicTagEditor.ViewModels
{
    /// <summary>
    /// Base class for all view models
    /// </summary>
    public class ViewModelBase : NotifyPropertyChanged
    {
        #region ------------------------- Constructors, Destructors, Dispose, Clone ---------------------------------------
        public ViewModelBase(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }
        #endregion

        #region ------------------------- Properties, Indexers ------------------------------------------------------------
        protected IEventAggregator EventAggregator { get; set; }
        #endregion
    }
}
