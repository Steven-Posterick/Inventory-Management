using System.Windows;

namespace Inventory_Management.Utils
{
    public interface IMessage
    {
        void Notify(string message);
    }
    public class Message : IMessage
    {
        /// <summary>
        /// Simple notification, can be changed to something else later on if desired.
        /// </summary>
        /// <param name="message"></param>
        public void Notify(string message) => MessageBox.Show(message);
    }
}