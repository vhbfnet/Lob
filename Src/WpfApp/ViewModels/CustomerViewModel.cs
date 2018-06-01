using Crm.Business.Entities;
using Lob.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Services;

namespace WpfApp.ViewModels
{
    public class CustomerViewModel : EditableViewModel<Customer>
    {
        private readonly ICustomerServiceProvider _customerServiceProvider;

        [Required]
        public string VmProperty1
        {
            get { return _vmProperty1; }
            set { SetProperty(ref _vmProperty1, value); }
        }
        private string _vmProperty1;

        [Required]
        public string VmProperty2
        {
            get { return _vmProperty2; }
            set { SetProperty(ref _vmProperty2, value); }
        }
        private string _vmProperty2;

        public CustomerViewModel()
        {
            ViewTitle = "Customer";
            _customerServiceProvider = Singleton<CustomerServiceProvider>.Instance; // TODO : Inject this by DI
        }

        protected async override Task Load()
        {
            await Refresh();
        }

        protected async override Task Refresh()
        {
            await GetCustomer();
            await base.Refresh();
        }

        protected async override Task Save()
        {
            Model = await _customerServiceProvider.SaveCustomerAsync(Model);
            await base.Save();
        }

        private async Task GetCustomer()
        {
            VmProperty1 = string.Empty;
            VmProperty2 = string.Empty;
            Model = await _customerServiceProvider.GetCustomerByKeyAsync(1);
        }
    }
}
