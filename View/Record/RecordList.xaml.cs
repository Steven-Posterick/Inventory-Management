using System.Windows;
using Inventory_Management.ViewModel.Record;

namespace Inventory_Management.View.Record
{
    public partial class RecordList : Window
    {

        public RecordList(IRecordListViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}