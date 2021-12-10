using System.Windows;
using Inventory_Management.ViewModel.Record;

namespace Inventory_Management.View.Record
{
    public partial class RecordEntry : Window
    {
        public RecordEntry(IRecordEntryViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}