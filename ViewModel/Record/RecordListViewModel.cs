using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Inventory_Management.Context;
using Inventory_Management.Utils.Command;
using Inventory_Management.Utils.Events.Product;
using Inventory_Management.Utils.Events.Record;
using Inventory_Management.Utils.Extensions;
using Inventory_Management.View.Product;
using Inventory_Management.View.Record;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;
using System.Linq;

namespace Inventory_Management.ViewModel.Record
{
    public interface IRecordListViewModel { }
    public class RecordListViewModel : BaseViewModel, IRecordListViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private ObservableCollection<Model.Record> _recordEntries = new ObservableCollection<Model.Record>();
        private string _id;
        private string _productId;
        
        // Note: This list will only query, additions will be done via the InventoryEntry screen.
        public RecordListViewModel(IServiceProvider serviceProvider, IEventAggregator eventAggregator) : base(serviceProvider)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<RefreshRecordList>().Subscribe(() => OnRefreshList().FireAndForget());
            _eventAggregator.GetEvent<SetProductIdForRecordList>().Subscribe(OnSetProductId);
        }

        /// <summary>
        /// Sets all other fields to null, then sets ProductId to corresponding value.
        /// </summary>
        /// <param name="productId"></param>
        private void OnSetProductId(string productId)
        {
            Id = null;
            ProductId = productId;
        }

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public ObservableCollection<Model.Record> RecordEntries
        {
            get => _recordEntries;
            private set => SetProperty(ref _recordEntries, value);
        }

        public string ProductId
        {
            get => _productId;
            set => SetProperty(ref _productId, value);
        }


        public FilterType Filter { get; set; }
        public ICommand AllFilter => CommandHelper.CreateCommand(() =>
        {
            Filter = FilterType.All;
        });

        public ICommand ReceiptFilter => CommandHelper.CreateCommand(() =>
        {
            Filter = FilterType.Receipt;
        });

        public ICommand ReceivedFilter => CommandHelper.CreateCommand(() =>
        {
            Filter = FilterType.Received;
        });


        public enum FilterType
        {
            All,
            Receipt,
            Received
        }


        private async Task OnRefreshList()
        {
            using var scope = ServiceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<InventoryManagementContext>();

            IQueryable<Model.Record> queryable = dbContext.Records
                .Include(x => x.ReceivedRecord)
                .Include(x => x.ReceiptRecord);

            queryable = Filter switch
            {
                FilterType.Receipt => queryable.Where(x => x.ReceiptRecord != null),
                FilterType.Received => queryable.Where(x => x.ReceivedRecord != null),
                _ => queryable
            };

            // Id, ProductId queries
            if (int.TryParse(Id, out var id))
                queryable = queryable.Where(x => x.Id == id);
            else if (int.TryParse(ProductId, out var productid))
                queryable = queryable.Where(x => x.ProductId == productid);

            RecordEntries = (await queryable.ToListAsync()).ToObservable();
        }

        /// <summary>
        /// Creating a record will open product entry screen without an id.
        /// </summary>
        private void OnCreateRecord() => OpenRecordEntry();

        /// <summary>
        /// Opening a record simply has an id of not 0.
        /// </summary>
        /// <param name="id"></param>
        private void OnOpenRecord(int id) => OpenRecordEntry(id);

        private void OpenRecordEntry(int id = 0)
        {
            OpenOrActivate<RecordEntry>(() =>
            {
                _eventAggregator.GetEvent<OpenRecordEvent>().Publish(id);
            });
        }
        
        public ICommand RefreshList => CommandHelper.CreateCommandAsync(OnRefreshList);
        public ICommand CreateRecord => CommandHelper.CreateCommand(OnCreateRecord);
        public ICommand OpenRecord => CommandHelper.CreateCommand<int>(OpenRecordEntry);
    }
}