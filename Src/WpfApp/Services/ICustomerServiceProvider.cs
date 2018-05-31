using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Crm.Business.Entities;

namespace WpfApp.Services
{
    public interface ICustomerServiceProvider
    {
        Task DeleteCustomer(Customer Customer);
        Task DeleteCustomerByKey(int id);
        Task<ObservableCollection<Customer>> GetAllCustomerAsync();
        Task<Customer> GetCustomerByKeyAsync(int id);
        Task<Customer> SaveCustomerAsync(Customer customer);
    }
}