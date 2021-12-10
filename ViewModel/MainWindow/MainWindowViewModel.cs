using System;
using System.Windows.Input;
using Inventory_Management.Utils.Command;
using Inventory_Management.View.Product;
using Inventory_Management.View.Record;

namespace Inventory_Management.ViewModel.MainWindow
{
    public interface IMainWindowViewModel { }
    
    public class MainWindowViewModel : BaseViewModel, IMainWindowViewModel
    {
        
        public MainWindowViewModel(IServiceProvider serviceProvider) : base(serviceProvider) { }
        
        public ICommand OpenProductList => CommandHelper.CreateCommand(OpenOrActivate<ProductList>);
        public ICommand OpenRecordList => CommandHelper.CreateCommand(OpenOrActivate<RecordList>);
    }
}