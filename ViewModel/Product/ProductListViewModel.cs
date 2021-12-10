using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Inventory_Management.Context;
using Inventory_Management.Utils;
using Inventory_Management.Utils.Command;
using Inventory_Management.Utils.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory_Management.ViewModel.Product
{
    public interface IProductListViewModel
    {
        
    }
    
    public class ProductListViewModel : BaseViewModel, IProductListViewModel
    {
        public ProductListViewModel(IServiceProvider serviceProvider) : base(serviceProvider) { }

        private string _idText;
        public string IdText
        {
            get => _idText;
            set => SetProperty(ref _idText, value);
        }

        private string _description;
        private ObservableCollection<Model.Product> _productEntries;

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

        private async Task RefreshListTask()
        {
            using var scope = ServiceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<InventoryManagementContext>();
        
            IQueryable<Model.Product> productQuery;
            if (int.TryParse(IdText, out var id))
                productQuery = dbContext.Products.Where(x => x.Id == id);
            else if (!Description.IsNullOrEmpty())
                productQuery = dbContext.Products.Where(x => x.Description.ToLower().Contains(Description.ToLower()));
            else
                productQuery = dbContext.Products.AsQueryable();

            ProductEntries = (await productQuery.ToListAsync()).ToObservable();
        }
        
        public ICommand RefreshList => CommandHelper.CreateCommandAsync(RefreshListTask);
    }
}