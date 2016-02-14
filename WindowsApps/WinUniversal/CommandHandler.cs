using System;
using System.Windows.Input;

namespace WinUniversal
{
    /// <summary>
    /// ICommand implementation for the view model
    /// </summary>
    public class CommandHandler : ICommand
    {
        public CommandHandler(Action executeCmd)
        {
            if (executeCmd == null)
            {
                throw new ArgumentNullException(nameof(executeCmd));
            }

            _executeCmd = executeCmd;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _executeCmd();
        }

        private Action _executeCmd;
    }
}
