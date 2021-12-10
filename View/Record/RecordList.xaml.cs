using System.Windows;
using Inventory_Management.ViewModel.Record;

namespace Inventory_Management.View.Record
{
    public partial class RecordList : Window
    {
        public RecordList(IRecordEntryViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}