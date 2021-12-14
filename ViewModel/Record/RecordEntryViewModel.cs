﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Inventory_Management.Context;
using Inventory_Management.Model;
using Inventory_Management.Utils;
using Inventory_Management.Utils.Command;
using Inventory_Management.Utils.Events.Allocation;
using Inventory_Management.Utils.Events.Record;
using Inventory_Management.Utils.Extensions;
using Inventory_Management.View.Allocation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;

namespace Inventory_Management.ViewModel.Record
{
    public interface IRecordEntryViewModel { }
    
    public class RecordEntryViewModel : BaseViewModel, IRecordEntryViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessage _message;
        private int _id;
        private int _productId;
        private int _quantity;
        private DateTime _date;
        private RecordType _recordType;
        private bool _canRelease;
        private bool _canDelete;
        private bool _canFill;
        private string _valueDisplay;
        private decimal _value;
        private ObservableCollection<Model.Allocation> _allocations = new ObservableCollection<Model.Allocation>();

        public RecordEntryViewModel(IServiceProvider serviceProvider, IEventAggregator eventAggregator, IMessage message) : base(serviceProvider)
        {
            _eventAggregator = eventAggregator;
            _message = message;
            eventAggregator.GetEvent<OpenRecordEvent>().Subscribe(x=> OpenRecordEntry(x).FireAndForget());
            eventAggregator.GetEvent<CreateRecordEvent>()
                .Subscribe(x => CreateRecordEntry(x.recordType, x.productId).FireAndForget());
        }

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public int ProductId
        {
            get => _productId;
            set => SetProperty(ref _productId, value);
        }

        public int Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value);
        }

        public decimal Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public RecordType RecordType
        {
            get => _recordType;
            set
            {
                SetProperty(ref _recordType, value);

                ValueDisplay = value switch
                {
                    RecordType.Receipt => "Cost: ",
                    RecordType.Received => "Price: ",
                    _ => null
                };
                
            }
        }

        public ObservableCollection<Model.Allocation> Allocations
        {
            get => _allocations;
            private set
            {
                SetProperty(ref _allocations, value);
                CanDelete = value == null || value.Count == 0;
            }
        }

        public bool CanRelease
        {
            get => _canRelease;
            set => SetProperty(ref _canRelease, value);
        }

        public bool CanDelete
        {
            get => _canDelete;
            set => SetProperty(ref _canDelete, value);
        }
        
        public bool CanSetQuantity
        {
            get => _canDelete;
            set => SetProperty(ref _canDelete, value);
        }
        
        public bool CanFill
        {
            get => _canFill;
            set => SetProperty(ref _canFill, value);
        }

        
        public string ValueDisplay
        {
            get => _valueDisplay;
            set => SetProperty(ref _valueDisplay, value);
        }

        private async Task CreateRecordEntry(RecordType recordType, int productId)
        {
            Allocations = new ObservableCollection<Model.Allocation>();
            CanRelease = false;
            CanSetQuantity = true;
            CanFill = false;
            ProductId = productId;
            
            using var scope = ServiceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<InventoryManagementContext>();
            var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == productId);
            
            RecordType = recordType;
            Value = (recordType == RecordType.Receipt ? product.Price : product.Cost) ?? 0;
        }

        private async Task OpenRecordEntry(int id)
        {
            using var scope = ServiceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<InventoryManagementContext>();

            Model.Record record;
            if (await dbContext.ReceiptRecords.AnyAsync(x=> x.Id == id))
            {
                var receiptRecord = await dbContext.ReceiptRecords.Include(x=> x.Allocations).Include(x=> x.IdNavigation).FirstAsync(x => x.Id == id);
                record = receiptRecord.IdNavigation;
                RecordType = RecordType.Receipt;
                Allocations = receiptRecord.Allocations.ToObservable();
                CanRelease = !CanDelete;
                Value = receiptRecord.Price ?? 0;
                CanFill = false;
            }
            else if (await dbContext.ReceivedRecords.AnyAsync(x=> x.Id == id))
            {
                var receivedRecord = await dbContext.ReceivedRecords.Include(x=> x.Allocations).Include(x=> x.IdNavigation).FirstAsync(x => x.Id == id);
                record = receivedRecord.IdNavigation;
                RecordType = RecordType.Received;
                Allocations = receivedRecord.Allocations.ToObservable();
                CanRelease = false;
                Value = receivedRecord.Cost ?? 0;
                CanFill = true;
            }
            else
            {
                _message.Notify("Invalid Record");
                _eventAggregator.GetEvent<CloseRecordEntry>().Publish();
                return;
            }
            
            CanSetQuantity = false;

            Id = record.Id;
            ProductId = record.ProductId.GetValueOrDefault();
            Quantity = record.Quantity.GetValueOrDefault();
            Date = record.Date.GetValueOrDefault();
        }

        private async Task OnFillRecord()
        {
            if (Allocations.Sum(x => x.AllocatedQuantity) >= Quantity)
            {
                _message.Notify("Already fully allocated.");
                return;
            }

            using var scope = ServiceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<InventoryManagementContext>();

            var validRecords = dbContext.ReceiptRecords.Where(x =>
                x.IdNavigation.ProductId == ProductId &&
                x.IdNavigation.Quantity < x.Allocations.Sum(x => x.AllocatedQuantity));
            
            var count = 0;
            await foreach (var record in validRecords.AsAsyncEnumerable())
            {
                count++;
                // This will call a parameterized SQL query.
                await dbContext.Database.ExecuteSqlRawAsync("CALL public.allocate_records({0},{1})", Id, record.Id);

                // Stop when the Received record is fully allocated.
                if (!await dbContext.ReceivedRecords.AnyAsync(x=> x.Id == Id && x.Allocations.Sum(a=> a.AllocatedQuantity) < x.IdNavigation.Quantity))
                    break;
            }

            if (count == 0)
            {
                _message.Notify("Failed to find any records to allocate to.");
            }
            else
            {
                // Refresh the UI.
                _eventAggregator.GetEvent<OpenRecordEvent>().Publish(Id);
            }
        }

        private async Task OnReleaseRecord()
        {
            throw new NotImplementedException();
        }

        private async Task OnDeleteRecord()
        {
            throw new NotImplementedException();
        }
        
        private void OnOpenAllocation(AllocationReference reference)
        {
            OpenOrActivate<AllocationEntry>(() =>
            {
                _eventAggregator.GetEvent<OpenAllocationEvent>().Publish(reference);
            });
        }
        
        public ICommand FillRecord => CommandHelper.CreateCommandAsync(OnFillRecord);
        public ICommand ReleaseRecord => CommandHelper.CreateCommandAsync(OnReleaseRecord);
        public ICommand DeleteRecord => CommandHelper.CreateCommandAsync(OnDeleteRecord);
        public ICommand OpenAllocation => CommandHelper.CreateCommand<AllocationReference>(OnOpenAllocation);
    }

    public enum RecordType
    {
        Receipt, 
        Received
    }
}