using System.Windows;
using Inventory_Management.ViewModel.Allocation;

namespace Inventory_Management.View.Allocation
{
    public partial class AllocationEntry : Window
    {
        public AllocationEntry(IAllocationEntryViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}