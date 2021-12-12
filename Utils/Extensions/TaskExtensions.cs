using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Inventory_Management.Utils.Extensions
{
    public static class TaskExtensions
    {
        public static async void FireAndForget(this Task task)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                await task;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {

                Mouse.OverrideCursor = null;
            }
        }
    }
}