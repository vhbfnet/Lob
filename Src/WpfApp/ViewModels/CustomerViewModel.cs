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

namespace WpfApp.ViewModels
{
    public class CustomerViewModel : EditableViewModel<Customer>
    {
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
            ViewTitle = "Customer view";
            LoadCommand.Execute();
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

        protected async override Task Save()
        {
            await GetData();
            await base.Save();
        }

        private async Task GetData()
        {
            await Task.Delay(3000);
            VmProperty1 = string.Empty;
            VmProperty2 = string.Empty;
            Mode = Mode.Edit;
            Model = new Customer();
        }
    }
}
