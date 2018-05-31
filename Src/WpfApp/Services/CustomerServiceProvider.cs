using Crm.Business.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Services
{
    /// <summary>
    /// Customer Service Provider
    /// </summary>
    public class CustomerServiceProvider : ICustomerServiceProvider
    {
        ObservableCollection<Customer> _customers;

        public CustomerServiceProvider()
        {
            _customers = new ObservableCollection<Customer>
            {
                new Customer { Id = 1, FirstName = "Victor", LastName = "BAPTISTA" },
                new Customer { Id = 2, FirstName = "Hugo", LastName = "DE FREITAS" }
            };
        }

        public async Task<ObservableCollection<Customer>> GetAllCustomerAsync()
        {
            await Task.Delay(3000); // Simulate database access          
            return await Task.Run(() => _customers);
        }

        public async Task<Customer> GetCustomerByKeyAsync(int id)
        {
            await Task.Delay(2000); // Simulate database access          
            return  await Task.Run(() => _customers.FirstOrDefault(c => c.Id == id));
        }

        public async Task<Customer> SaveCustomerAsync(Customer customer)
        {
            var exist = _customers.FirstOrDefault(c => c.Id == customer.Id);

            if (exist == null)
                _customers.Add(customer);
            else
                _customers[_customers.IndexOf(exist)] = customer;

            await Task.Delay(2000); // Simulate database access          
            return await Task.Run(() => _customers.FirstOrDefault(c => c.Id == customer.Id));
        }

        public async Task DeleteCustomer(Customer Customer)
        {
            await DeleteCustomerByKey(Customer.Id);
        }

        public async Task DeleteCustomerByKey(int id)
        {
            var customer = await GetCustomerByKeyAsync(id);

            await Task.Delay(2000); // Simulate database access          
            await Task.Run(() => { _customers.Remove(customer); });
        }
    }
}
