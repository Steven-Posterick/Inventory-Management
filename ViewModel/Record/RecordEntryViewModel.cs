using System;
using System.Threading.Tasks;

namespace Inventory_Management.ViewModel.Record
{
    public interface IRecordEntryViewModel : IOpenWithId<int> { }
    
    public class RecordEntryViewModel : BaseViewModel, IRecordEntryViewModel
    {
        public RecordEntryViewModel(IServiceProvider serviceProvider) : base(serviceProvider) { }
        
        // TODO: Implement
        public Task OpenWithId(int id) => throw new NotImplementedException();
    }
}