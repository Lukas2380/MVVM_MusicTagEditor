using System.ComponentModel;

namespace Common.NotifyPropertyChanged
{
    /// <summary>
    /// Class for property change notifications.
    /// Derives from <see cref="INotifyPropertyChanged"/>
    /// </summary>
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        /// <summary>
        /// Multicast event for property change notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// Name of the property used to notify listeners.  
        /// </summary>
        /// <param name="property">Name of the property. This value is optional and 
        /// can be provided automatically when invoked from compilers.
        /// </param>
        public void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }

}
