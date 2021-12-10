using System;
using System.Windows;
using System.Windows.Input;

namespace Inventory_Management.Utils.Command
{
    /// <summary>
    /// Default Command logic, for a command with no can execute func.
    /// </summary>
    public class Command : ICommand
    {
        private readonly Action _execute;

        public Command(Action execute)
        {
            _execute = execute;
        }
        
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            try
            {
                _execute();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
    
    
    /// <summary>
    /// Default Generic Command logic, for a command with no can execute func.
    /// </summary>
    public class Command<T> : ICommand
    {
        private readonly Action<T> _execute;

        public Command(Action<T> execute)
        {
            _execute = execute;
        }
        
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => _execute((T) parameter);
        
        public event EventHandler CanExecuteChanged;
    }
}