using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Inventory_Management.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory_Management.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected readonly IServiceProvider ServiceProvider;

        protected BaseViewModel(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(ref T reference, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(reference, value)) return;
            
            reference = value;
            OnPropertyChanged(propertyName);
        }

        protected void OpenOrActivate<TWindow>() where TWindow : Window
        { 
            var window = Application.Current.Windows.Cast<Window>().FirstOrDefault(x => x is TWindow);
            if (window == null)
                ServiceProvider.GetService<TWindow>()?.Show();
            else
                window.Activate();
        }
    }
}