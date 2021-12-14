using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Inventory_Management.Context;
using Inventory_Management.Utils.Command;
using Inventory_Management.Utils.Events.Product;
using Inventory_Management.Utils.Extensions;
using Inventory_Management.View.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;

namespace Inventory_Management.ViewModel.Product
{
    public interface IProductListViewModel { }
    
    public class ProductListViewModel : BaseViewModel, IProductListViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private string _id;
        private string _description;
        private ObservableCollection<Model.Product> _productEntries = new ObservableCollection<Model.Product>();
        public ProductListViewModel(IServiceProvider serviceProvider, IEventAggregator eventAggregator) : base(serviceProvider)
        {
            _eventAggregator = eventAggregator;
            eventAggregator.GetEvent<RefreshProductListEvent>().Subscribe(()=> OnRefreshListTask().FireAndForget());
        }
        
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public ObservableCollection<Model.Product> ProductEntries
        {
            get => _productEntries;
            private set => SetProperty(ref _productEntries, value);
        }


        /// <summary>
        /// Refreshes the list based on the parameters on the current screen.
        /// </summary>
        private async Task OnRefreshListTask()
        {
            ProductEntries.Clear();
            
            using var scope = ServiceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<InventoryManagementContext>();

            IQueryable<Model.Product> productQuery;
            if (int.TryParse(Id, out var id))
                productQuery = dbContext.Products.Where(x => x.Id == id);
            else if (!Description.IsNullOrEmpty())
                productQuery = dbContext.Products.Where(x => x.Description.ToLower().Contains(Description.ToLower()));
            else
                productQuery = dbContext.Products.AsQueryable();

            ProductEntries = (await productQuery.OrderBy(x=> x.Id).ToListAsync()).ToObservable();
        }
        
        /// <summary>
        /// Creating a product will open product entry screen without an id.
        /// </summary>
        private void OnCreateProduct() => OpenProductEntry();
        
        /// <summary>
        /// Opening a product simply has an id of not 0.
        /// </summary>
        /// <param name="id"></param>
        private void OnOpenProduct(int id) => OpenProductEntry(id);

        /// <summary>
        /// Opens or activates the current screen, then publishes the open event message.
        /// </summary>
        /// <param name="id"></param>
        private void OpenProductEntry(int id = 0)
        {
            OpenOrActivate<ProductEntry>(() =>
            {
                _eventAggregator.GetEvent<OpenProductEvent>().Publish(id);
            });
        }
        
        public ICommand RefreshList => CommandHelper.CreateCommandAsync(OnRefreshListTask);
        public ICommand CreateProduct => CommandHelper.CreateCommand(OnCreateProduct);
        public ICommand OpenProduct => CommandHelper.CreateCommand<int>(OnOpenProduct);
    }
}