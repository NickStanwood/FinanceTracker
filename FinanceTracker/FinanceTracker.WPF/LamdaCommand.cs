using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FinanceTracker.WPF
{
    internal class LamdaCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        Func<object?, bool> CanExecuteFn;
        Action<object?> ExecuteFn;
        public LamdaCommand(Func<object?, bool> canExecute, Action<object?> execute)
        {
            CanExecuteFn = canExecute;
            ExecuteFn = execute;
        }

        public bool CanExecute(object? parameter)
        {
            return CanExecuteFn(parameter);
        }

        public void Execute(object? parameter)
        {
            ExecuteFn(parameter);
        }
    }
}
