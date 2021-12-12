using Prism.Events;

namespace Inventory_Management.Utils.Events.Allocation
{
    public class OpenAllocationEvent : PubSubEvent<(int ReceiptId, int ReceiveId)> { }
}