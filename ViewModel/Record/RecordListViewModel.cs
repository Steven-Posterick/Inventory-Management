using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Inventory_Management.Context;
using Inventory_Management.Utils.Command;
using Inventory_Management.Utils.Events.Product;
using Inventory_Management.Utils.Extensions;
using Inventory_Management.View.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;

namespace Inventory_Management.ViewModel.Record
{
    public interface IRecordListViewModel { }
    public class RecordListViewModel : BaseViewModel, IRecordListViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private string _id;
        private ObservableCollection<Model.Record> _recordEntries = new ObservableCollection<Model.Record>();

        public RecordListViewModel(IServiceProvider serviceProvider) : base(serviceProvider) {}


        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public ObservableCollection<Model.Record> RecordEntries
        {
            get => _recordEntries;
            private set => SetProperty(ref _recordEntries, value);
        }

        

    }
}