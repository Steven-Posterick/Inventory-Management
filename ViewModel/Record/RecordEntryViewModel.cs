using System;
using System.Threading.Tasks;
using Inventory_Management.Utils.Events.Record;
using Inventory_Management.Utils.Extensions;
using Prism.Events;

namespace Inventory_Management.ViewModel.Record
{
    public interface IRecordEntryViewModel { }
    
    public class RecordEntryViewModel : BaseViewModel, IRecordEntryViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        public RecordEntryViewModel(IServiceProvider serviceProvider, IEventAggregator eventAggregator) : base(serviceProvider)
        {
            _eventAggregator = eventAggregator;
            eventAggregator.GetEvent<OpenRecordEvent>().Subscribe(x=> OpenRecordEntry(x).FireAndForget());
        }

        private async Task OpenRecordEntry(int id)
        {
            throw new NotImplementedException();
        }
        
    }
}