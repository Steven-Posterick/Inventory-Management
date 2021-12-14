using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Inventory_Management.Utils.Command;
using Inventory_Management.Utils.Events.Allocation;
using Inventory_Management.Utils.Events.Record;
using Inventory_Management.View.Record;
using Prism.Events;

namespace Inventory_Management.ViewModel.Allocation
{
    public interface IAllocationEntryViewModel { }
    
    public class AllocationEntryViewModel : BaseViewModel, IAllocationEntryViewModel
    {
        private int _receiptId;
        private int _receivedId;

        public int ReceiptId
        {
            get => _receiptId;
            set => SetProperty(ref _receiptId, value);
        }

        public int ReceivedId
        {
            get => _receivedId;
            set => SetProperty(ref _receivedId, value);
        }

        private void OpenAllocation(int receiptId, int receivedId)
        {
            ReceiptId = receiptId;
            ReceivedId = receivedId;
        }

        private readonly IEventAggregator _eventAggregator;

        public AllocationEntryViewModel(IServiceProvider serviceProvider, IEventAggregator eventAggregator) : base(serviceProvider)
        {
            _eventAggregator = eventAggregator;
            
            eventAggregator.GetEvent<OpenAllocationEvent>().Subscribe(x =>
            {
                OpenAllocation(x.ReceiptId, x.ReceivedId);
            });
        }

        // OpenReceived
        public ICommand OpenReceived => CommandHelper.CreateCommand(() =>
        {
            OpenOrActivate<RecordEntry>(() =>
            {
                _eventAggregator.GetEvent<OpenRecordEvent>().Publish(ReceivedId);
                //_eventAggregator.GetEvent<OpenRecordEvent>().Publish(ReceiptId);
            });
        });

        // OpenReceipt

    }
}