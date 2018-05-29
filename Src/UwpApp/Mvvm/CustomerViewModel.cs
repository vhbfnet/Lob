using Crm.Business.Entities;
using Lob.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UwpApp.Mvvm
{
    public class CustomerViewModel : ViewModelBase
    {
        public Customer Customer
        {
            get { return _customer; }
            set { SetProperty(ref _customer, value); }
        }
        private Customer _customer;

        public CustomerViewModel()
        {
            Customer = new Customer { FirstName = "Victor", LastName = "Hugo", Email = "vhbf@hotmail.fr" };
        }
    }
}
