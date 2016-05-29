using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AllJoynSampleApp.ViewModels
{
    internal class GenericCommand : ICommand
    {
        Action<object> _execute;
        Func<object,bool> _canExecute;
        public GenericCommand(Action execute) : this((object o)=> { execute(); }, null) { }
        public GenericCommand(Action<object> execute, Func<object,bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            if(_canExecute != null)
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null) return true;
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
