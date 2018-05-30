using Lob.Mvvm.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lob.Mvvm.Tests
{
    [TestClass]
    public class ListViewModelTest : ViewModelCoreTest
    {
        [TestMethod]
        public void ViewModelHasErrors()
        {
            EntityListViewModel vm = new EntityListViewModel();
            vm.MyProperty = true;
            vm.Validate();
            Assert.IsTrue(vm.HasErrors);
        }

        [TestMethod]
        public void CanExecuteDetailShouldBeFalse()
        {
            EntityListViewModel vm = new EntityListViewModel();
            vm.MyProperty = true;
            Assert.IsFalse(vm.DetailCommand.CanExecute());
        }

        [TestMethod]
        public void CanExecuteDetailShouldBeTrue()
        {
            EntityListViewModel vm = new EntityListViewModel();
            vm.MyProperty = true;
            vm.SelectedItems.Add(new Entity());
            Assert.IsTrue(vm.DetailCommand.CanExecute());
        }

        [TestMethod]
        public void CanExecuteRefreshShouldBeTrue()
        {
            EntityListViewModel vm = new EntityListViewModel();
            Assert.IsTrue(vm.RefreshCommand.CanExecute());
        }

        [TestMethod]
        public void SelectedItemsShouldBeTrue()
        {
            EntityListViewModel vm = new EntityListViewModel();
            var entity = new Entity();
            vm.CurrentItem = entity;
            Assert.IsTrue(vm.SelectedItems.Count == 1);
            vm.CurrentItem = entity;
            Assert.IsTrue(vm.SelectedItems.Count == 1);
        }

        [TestMethod]
        public void CurrentItemPropertyChanged()
        {
            var itemChanged = false;
            EntityListViewModel vm = new EntityListViewModel();
            vm.Items = new ObservableCollection<Entity>
            {
                new Entity
                {
                    MyDate = DateTime.Now,
                    MyInt = 1972,
                    MyString = "Victor"
                }
            };

            vm.CurrentItem = vm.Items.First();

            vm.CurrentItemPropertyChanged += (s, e) =>
            {
                itemChanged = true;
            };

            vm.CurrentItem.MyString = "Hugo";
            Assert.IsTrue(itemChanged);
        }

        [TestMethod]
        public void DetailCommandCurrentItem()
        {
            EntityListViewModel vm = new EntityListViewModel();
            var entity = new Entity();
            vm.CurrentItem = entity;
            Assert.IsTrue(vm.DetailCommand.CanExecute());
            vm.CurrentItem = null;
            Assert.IsFalse(vm.DetailCommand.CanExecute());
        }
    }
}
