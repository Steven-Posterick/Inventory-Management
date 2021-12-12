using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Inventory_Management.Context;
using Inventory_Management.Utils;
using Inventory_Management.Utils.Command;
using Inventory_Management.Utils.Events.Product;
using Inventory_Management.Utils.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;

namespace Inventory_Management.ViewModel.Product
{
    public interface IProductEntryViewModel { }
    
    public class ProductEntryViewModel : BaseViewModel, IProductEntryViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessage _message;
        private int _id;
        private string _description;
        private decimal _price;
        private decimal _cost;
        
        public ProductEntryViewModel(IServiceProvider serviceProvider, IEventAggregator eventAggregator, IMessage message) : base(
            serviceProvider)
        {
            _eventAggregator = eventAggregator;
            _message = message;
            eventAggregator.GetEvent<OpenProductEvent>().Subscribe(OnOpen);
        }

        public int Id
        {
            get => _id;
            set
            {
                SetProperty(ref _id, value);
                OnPropertyChanged(nameof(IsCreated));
            }
        }
        
        public bool IsCreated => Id != 0;

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public decimal Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        public decimal Cost
        {
            get => _cost;
            set => SetProperty(ref _cost, value);
        }

        /// <summary>
        /// Takes in an id (if id = 0, then it is a new entry.)
        /// Then loads them into the screen.
        /// </summary>
        /// <param name="id"></param>
        private void OnOpen(int id)
        {
            if (id == 0) return;
           
            using var scope = ServiceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<InventoryManagementContext>();
            var product = dbContext.Products.FirstOrDefault(x => x.Id == id);
            if (product == null) return;

            Id = id;
            Description = product.Description;
            Price = product.Price ?? 0;
            Cost = product.Cost ?? 0;
        }

        /// <summary>
        /// Pre-save logic, cannot have negative price/cost and the description cannot be blank
        /// </summary>
        /// <returns></returns>
        private bool IsValidForSave()
        {
            if (Price <= 0.0M)
            {
                _message.Notify("Price must be greater than $0.00");
                return false;
            }
            
            if (Cost <= 0.0M)
            {
                _message.Notify("Cost must be greater than $0.00");
                return false;
            }

            if (!Description.IsNullOrEmpty()) return true;
            
            _message.Notify("Fill out description before saving");
            return false;
        }

        /// <summary>
        /// Will insert or update the entry in the database, then refresh the list if it is open.
        /// </summary>
        private async Task OnSaveProduct()
        {
            if (!IsValidForSave()) return;
            
            using var scope = ServiceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<InventoryManagementContext>();

            Model.Product product;
            if (Id != 0)
            {
                product = await dbContext.Products.FirstAsync(x => x.Id == Id);
            }
            else
            {
                product = new Model.Product();
                dbContext.Add(product);
            }

            product.Description = Description;
            product.Cost = Cost;
            product.Price = Price;

            await dbContext.SaveChangesAsync();
            
            // Refresh the product list to update the changes
            _eventAggregator.GetEvent<RefreshProductListEvent>().Publish();
            
            // Refresh the current screen with the new id.
            _eventAggregator.GetEvent<OpenProductEvent>().Publish(product.Id);
        }
        
        
        private async Task OnDeleteProduct()
        {
            if (MessageBox.Show("Are you sure you would like to delete this product?", "Delete Product?", MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
            
            using var scope = ServiceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<InventoryManagementContext>();

            var product = await dbContext.Products.FirstOrDefaultAsync(r => r.Id == Id);
            if (product != null)
            {
                // Remove the product
                dbContext.Products.Remove(product);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                _message.Notify($"Failed to find product with id {Id}");
            }
            
            // Close the current product entry screen.
            _eventAggregator.GetEvent<CloseProductEntryEvent>().Publish();
            
            // Reload the list
            _eventAggregator.GetEvent<RefreshProductListEvent>().Publish();
        }
        
        public ICommand SaveProduct => CommandHelper.CreateCommandAsync(OnSaveProduct);
        public ICommand DeleteProduct => CommandHelper.CreateCommandAsync(OnDeleteProduct);

        /// <summary>
        /// TODO: Create the open receive/sell (will be the same screen, just different parameter)
        /// TODO: Add open Record List will be a Pub/Sub that passes in the product id for the search.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public ICommand OpenReceive => CommandHelper.CreateCommand(()=> throw new NotImplementedException());
        public ICommand OpenSell => CommandHelper.CreateCommand(()=> throw new NotImplementedException());
        public ICommand OpenRecordList => CommandHelper.CreateCommand(()=> throw new NotImplementedException());
    }
}