using Inventory_Management.ViewModel.Record;
using Prism.Events;

namespace Inventory_Management.Utils.Events.Record
{
    public class CreateRecordEvent : PubSubEvent<(RecordType recordType, int productId)> { }
}