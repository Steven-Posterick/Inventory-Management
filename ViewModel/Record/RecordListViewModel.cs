using System;

namespace Inventory_Management.ViewModel.Record
{
    public interface IRecordListViewModel { }
    public class RecordListViewModel : BaseViewModel, IRecordListViewModel
    {
        public RecordListViewModel(IServiceProvider serviceProvider) : base(serviceProvider) { }
    }
}