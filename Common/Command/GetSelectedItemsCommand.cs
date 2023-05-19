using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Common.Command
{
    /// <summary>
    /// This is a custom ICommand for getting the Selected Items, in this case Songs, from the Datagrid
    /// https://stackoverflow.com/questions/9880589/bind-to-selecteditems-from-datagrid-or-listbox-in-mvvm
    /// ("Binding directly do view model, little tricky version:")
    /// </summary>
    public class GetSelectedItemsCommand : ICommand
    {
        public GetSelectedItemsCommand(Action<object> action)
        {
            _action = action;
        }

        private readonly Action<object> _action;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}
