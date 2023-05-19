using Data;
using MahApps.Metro.Controls;
using Microsoft.Practices.Prism.Events;
using MVVM_MusicTagEditor.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MVVM_MusicTagEditor.ViewModels
{
    public class EditWindowViewModel : ViewModelBase
    {
        private List<Song> selectedItems;

        public List<Song> SelectedItems
        {
            get
            {
                return this.selectedItems;
            }
            set
            {
                if (this.selectedItems != value)
                {
                    this.selectedItems = value;
                }
            }
        }

        public Song CurrentSong { get; set; }

        public EditWindowViewModel(IEventAggregator eventAggregator, List<Song> selectedItems) : base(eventAggregator)
        {
            SelectedItems = selectedItems;

            CurrentSong = SelectedItems.FirstOrDefault();
        }

    }
}
