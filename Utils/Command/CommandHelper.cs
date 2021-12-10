using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Inventory_Management.Utils.Command
{
    public static class CommandHelper
    {
        public static ICommand CreateCommand(Action action) => new Utils.Command.Command(action);
        public static ICommand CreateCommand<T>(Action<T> action) => new Command<T>(action);
        public static ICommand CreateCommandAsync(Func<Task> task) => new Utils.Command.Command(()=> SafeAwait(task.Invoke()));

        private static async void SafeAwait(Task task)
        {
            try
            {
                await task;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}