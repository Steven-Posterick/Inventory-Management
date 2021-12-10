using System.Windows;
using Inventory_Management.ViewModel.Product;

namespace Inventory_Management.View.Product
{
    public partial class ProductEntry : Window
    {
        public ProductEntry(IProductEntryViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}