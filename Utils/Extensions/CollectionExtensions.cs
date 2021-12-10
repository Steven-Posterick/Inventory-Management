using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Inventory_Management.Utils.Extensions
{
    public static class CollectionExtensions
    {
        public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> enumerable)
        {
            return new ObservableCollection<T>(enumerable);
        }
    }
}