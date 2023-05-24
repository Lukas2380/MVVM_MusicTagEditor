using System.ComponentModel;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace Common.NotifyPropertyChanged
{
    /// <summary>
    /// Class for property change notifications.
    /// Derives from <see cref="INotifyPropertyChanged"/>
    /// </summary>
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        private readonly SynchronizationContext synchronizationContext;

        public NotifyPropertyChanged()
        {
            synchronizationContext = SynchronizationContext.Current;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
