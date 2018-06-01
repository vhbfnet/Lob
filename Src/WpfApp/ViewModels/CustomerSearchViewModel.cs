using Crm.Business.Entities;
using Lob.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Services;

namespace WpfApp.ViewModels
{
    /// <summary>
    /// Customer search view model
    /// </summary>
    /// <seealso cref="Lob.Mvvm.SearchViewModel{Crm.Business.Entities.Customer, Crm.Business.Entities.Customer}" />
    public class CustomerSearchViewModel : SearchViewModel<Customer, Customer>
    {
        private readonly ICustomerServiceProvider _customerServiceProvider;

        public CustomerSearchViewModel()
        {
            ViewTitle = "Search customers";
            _customerServiceProvider = Singleton<CustomerServiceProvider>.Instance; // TODO : Inject this by DI
        }

        protected async override Task Load()
        {
            // Load here referentials data
            await Refresh();
        }

        protected async override Task Refresh()
        {
            await Search();
        }

        protected async override Task Search()
        {
            await GetCustomers();
            await base.Search();
        }

        private async Task GetCustomers()
        {
            Items = await _customerServiceProvider.GetAllCustomerAsync();
        }
    }
}
