using Crm.Business.Entities;
using Lob.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.ViewModels
{
    public class CustomerListViewModel : ListViewModel<Customer>
    {
        public CustomerListViewModel()
        {
            ViewTitle = "Customer list view";
        }

        protected async override Task Load()
        {
            await GetData();
            await base.Load();
        }

        protected async override Task Refresh()
        {
            await GetData();
            await base.Refresh();
        }

        private async Task GetData()
        {
            await Task.Delay(3000);

            var list = new ObservableCollection<Customer>();
            list.Add(new Customer { FirstName = "Victor", LastName = "BAPTISTA" });
            list.Add(new Customer { FirstName = "Hugo", LastName = "DE FREITAS" });

            Items = list;
        }
    }
}
