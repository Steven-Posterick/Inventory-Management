using System.Windows;
using Inventory_Management.Utils.Events.Record;
using Inventory_Management.ViewModel.Record;
using Prism.Events;

namespace Inventory_Management.View.Record
{
    public partial class RecordEntry : Window
    {
        public RecordEntry(IRecordEntryViewModel viewModel, IEventAggregator eventAggregator)
        {
            InitializeComponent();
            DataContext = viewModel;
            eventAggregator.GetEvent<CloseRecordEntry>().Subscribe(Close);
        }
    }
}