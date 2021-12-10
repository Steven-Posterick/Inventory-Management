using System;
using System.Threading.Tasks;

namespace Inventory_Management.ViewModel.Product
{
    public interface IProductEntryViewModel : IOpenWithId<int> { }
    
    public class ProductEntryViewModel : BaseViewModel, IProductEntryViewModel
    {
        public ProductEntryViewModel(IServiceProvider serviceProvider) : base(serviceProvider) { }
        
        // TODO: Implement
        public Task OpenWithId(int id) => throw new NotImplementedException();
    }
}