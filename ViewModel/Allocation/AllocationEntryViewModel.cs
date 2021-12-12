using System;
using System.Threading.Tasks;
using Inventory_Management.Utils.Events.Allocation;
using Prism.Events;

namespace Inventory_Management.ViewModel.Allocation
{
    public interface IAllocationEntryViewModel { }
    
    public class AllocationEntryViewModel : BaseViewModel, IAllocationEntryViewModel
    {
        public AllocationEntryViewModel(IServiceProvider serviceProvider, IEventAggregator eventAggregator) : base(serviceProvider)
        {
            eventAggregator.GetEvent<OpenAllocationEvent>().Subscribe(x =>
            {
                var (receiptId, receiveId) = x;
                OpenAllocation(receiptId, receiveId);
            });
        }

        private void OpenAllocation(int receiptId, int receivedId)
        {
            
        }
    }
}