using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Inventory_Management.Context;
using Inventory_Management.Model;
using Inventory_Management.Utils;
using Inventory_Management.View.Allocation;
using Inventory_Management.View.MainWindow;
using Inventory_Management.View.Product;
using Inventory_Management.View.Record;
using Inventory_Management.ViewModel.Allocation;
using Inventory_Management.ViewModel.MainWindow;
using Inventory_Management.ViewModel.Product;
using Inventory_Management.ViewModel.Record;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Prism.Events;

namespace Inventory_Management
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private readonly IHost _host;
        public new static App Current => (App)Application.Current;
        public IServiceProvider Services { get; }
        public App()
        {
            _host = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    // View models 
                    services.AddTransient<IMainWindowViewModel, MainWindowViewModel>();
                    services.AddTransient<IAllocationEntryViewModel, AllocationEntryViewModel>();
                    services.AddTransient<IRecordListViewModel, RecordListViewModel>();
                    services.AddTransient<IRecordEntryViewModel, RecordEntryViewModel>();
                    services.AddTransient<IProductListViewModel, ProductListViewModel>();
                    services.AddTransient<IProductEntryViewModel, ProductEntryViewModel>();

                    // Views
                    services.AddTransient<MainWindow>();
                    services.AddTransient<AllocationEntry>();
                    services.AddTransient<RecordList>();
                    services.AddTransient<RecordEntry>();
                    services.AddTransient<ProductList>();
                    services.AddTransient<ProductEntry>();
                    
                    // Services
                    services.AddSingleton<IEventAggregator, EventAggregator>();
                    services.AddSingleton<IMessage, Message>();

                    services.AddDbContext<InventoryManagementContext>(options =>
                    {
                        var connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                        options.UseNpgsql(connectionString);
                    });
                    
                }).Build();

            Services = _host.Services;
        }
        
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();
            Services.GetService<MainWindow>()?.Show();
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
            }
        }
    }
}
