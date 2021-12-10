using System.Threading.Tasks;

namespace Inventory_Management.ViewModel
{
    public interface IOpenWithId<in T>
    {
        Task OpenWithId(T id);
    }
}