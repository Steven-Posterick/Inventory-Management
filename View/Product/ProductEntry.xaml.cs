using System.Windows;
using Inventory_Management.Utils.Events.Product;
using Inventory_Management.ViewModel.Product;
using Prism.Events;

namespace Inventory_Management.View.Product
{
    public partial class ProductEntry : Window
    {
        public ProductEntry(IProductEntryViewModel viewModel, IEventAggregator eventAggregator)
        {
            InitializeComponent();
            DataContext = viewModel;
            eventAggregator.GetEvent<CloseProductEntryEvent>().Subscribe(Close);
        }
    }
}