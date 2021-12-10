using System;
using System.Threading.Tasks;

namespace Inventory_Management.ViewModel.Allocation
{
    public interface IAllocationEntryViewModel : IOpenWithId<(int ReceiptId, int ReceiveId)> { }
    
    public class AllocationEntryViewModel : BaseViewModel, IAllocationEntryViewModel
    {
        public AllocationEntryViewModel(IServiceProvider serviceProvider) : base(serviceProvider) { }

        public Task OpenWithId((int ReceiptId, int ReceiveId) id) { throw new NotImplementedException(); }
    }
}