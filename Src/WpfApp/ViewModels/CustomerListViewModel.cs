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
    /// Customer list view model
    /// </summary>
    /// <seealso cref="Lob.Mvvm.ListViewModel{Crm.Business.Entities.Customer}" />
    public class CustomerListViewModel : ListViewModel<Customer>
    {
        private readonly ICustomerServiceProvider _customerServiceProvider;

        public CustomerListViewModel()
        {
            ViewTitle = "Customer list view";
            _customerServiceProvider = Singleton<CustomerServiceProvider>.Instance; // TODO : Inject this by DI
        }

        protected async override Task Load()
        {
            // Load here referentials data
            await Refresh();
        }

        protected async override Task Refresh()
        {
            await GetCustomers();
            await base.Refresh();
        }

        private async Task GetCustomers()
        {
            Items = await _customerServiceProvider.GetAllCustomerAsync();
        }
    }
}
