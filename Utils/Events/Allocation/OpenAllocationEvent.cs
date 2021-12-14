using Inventory_Management.Model;
using Prism.Events;

namespace Inventory_Management.Utils.Events.Allocation
{
    public class OpenAllocationEvent : PubSubEvent<AllocationReference> { }
}