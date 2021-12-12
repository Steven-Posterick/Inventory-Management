using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Inventory_Management.Utils.Extensions;

namespace Inventory_Management.Utils.Command
{
    public static class CommandHelper
    {
        public static ICommand CreateCommand(Action action) => new Command(action);
        public static ICommand CreateCommand<T>(Action<T> action) => new Command<T>(action);
        public static ICommand CreateCommandAsync(Func<Task> task) => new Command(()=> task.Invoke().FireAndForget());
    }
}