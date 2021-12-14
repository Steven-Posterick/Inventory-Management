using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Inventory_Management.Context;
using Inventory_Management.Utils.Command;
using Inventory_Management.Utils.Events.Allocation;
using Inventory_Management.Utils.Events.Record;
using Inventory_Management.View.Record;
using Microsoft.Extensions.DependencyInjection;
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
            var scope = ServiceProvider.CreateScope();
            var context = scope.ServiceProvider.GetService<InventoryManagementContext>();
            var allocation = context.Allocations.FirstOrDefault(a => a.ReceiptId == receiptId && a.ReceivedId == receivedId);

            AllocatedQuantity = allocation?.AllocatedQuantity ?? 0;
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
            });
        });

        // OpenReceipt
        public ICommand OpenReceipt => CommandHelper.CreateCommand(() =>
        {
            OpenOrActivate<RecordEntry>(() =>
            {
                _eventAggregator.GetEvent<OpenRecordEvent>().Publish(ReceiptId);
            });
        });

        // connect to database to show allocated quantity on allocation screen
        private int _allocatedQuantity;
        public int AllocatedQuantity
        {
            get => _allocatedQuantity;
            set => SetProperty(ref _allocatedQuantity, value);
        }

        
    }
}