using System.Windows;
using Inventory_Management.ViewModel.Product;

namespace Inventory_Management.View.Product
{
    public partial class ProductList : Window
    {
        public ProductList(IProductListViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}