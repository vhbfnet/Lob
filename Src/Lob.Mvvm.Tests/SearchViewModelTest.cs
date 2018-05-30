using System;
using Lob.Mvvm.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lob.Mvvm.Tests
{
    [TestClass]
    public class SearchViewModelTest : ViewModelCoreTest
    {
        [TestMethod]
        public void HasErrorsShouldBeTrue()
        {
            EntitySearchViewModel vm = new EntitySearchViewModel();
            vm.MyProperty = true;
            vm.Validate();
            Assert.IsTrue(vm.HasErrors);
        }

        [TestMethod]
        public void CanExecuteDetailShouldBeFalse()
        {
            EntitySearchViewModel vm = new EntitySearchViewModel();
            vm.MyProperty = true;
            Assert.IsFalse(vm.DetailCommand.CanExecute());
        }

        [TestMethod]
        public void CanExecuteDetailShouldBeTrue()
        {
            EntitySearchViewModel vm = new EntitySearchViewModel();
            vm.MyProperty = true;
            vm.SelectedItems.Add(new Entity());
            Assert.IsTrue(vm.DetailCommand.CanExecute());
        }

        [TestMethod]
        public void CanExecuteRefreshShouldBeTrue()
        {
            EntitySearchViewModel vm = new EntitySearchViewModel();
            vm.MyRequiredProperty = "Required";
            Assert.IsTrue(vm.IsChanged);
            Assert.IsFalse(vm.HasErrors);
            Assert.IsTrue(vm.RefreshCommand.CanExecute());
        }

        [TestMethod]
        public void ViewModelHasErrorsShouldBeFalse()
        {
            EntitySearchViewModel vm = new EntitySearchViewModel();
            vm.MyRequiredProperty = "Required";
            vm.Criteria.MyRequiredString = "Change";
            Assert.IsFalse(vm.HasErrors);
        }

        [TestMethod]
        public void ViewModelHasErrorsShoulBeTrueOnModelChanged()
        {
            EntitySearchViewModel vm = new EntitySearchViewModel();
            vm.Criteria.MyInt = 1972;
            Assert.IsTrue(vm.HasErrors);
        }

        [TestMethod]
        public void ViewModelHasErrorsShoulBeTrueOnVmChanged()
        {
            EntitySearchViewModel vm = new EntitySearchViewModel();
            vm.MyProperty = true;
            Assert.IsTrue(vm.HasErrors);
        }

        [TestMethod]
        public void ViewModelHasErrorsShoulBeTrueOnCriteriaChanged()
        {
            EntitySearchViewModel vm = new EntitySearchViewModel();
            vm.Criteria.MyInt = 1972;
            Assert.IsTrue(vm.HasErrors);
        }

        [TestMethod]
        public void CanExecuteRefreshShouldBeFalseEvenVmHasErrors()
        {
            EntitySearchViewModel vm = new EntitySearchViewModel();
            vm.MyProperty = true;
            Assert.IsTrue(vm.IsChanged);
            Assert.IsTrue(vm.HasErrors);
            Assert.IsTrue(vm.RefreshCommand.CanExecute());
        }

        [TestMethod]
        public void CanExecuteSearchShouldBeTrueIfVmHasErrors()
        {
            EntitySearchViewModel vm = new EntitySearchViewModel();
            vm.MyProperty = true;
            Assert.IsTrue(vm.IsChanged);
            Assert.IsTrue(vm.HasErrors);
            Assert.IsFalse(vm.SearchCommand.CanExecute());
        }

        [TestMethod]
        public void CanExecuteSearchShouldBeTrueIfCriteriaHasErrors()
        {
            EntitySearchViewModel vm = new EntitySearchViewModel();
            vm.Criteria.MyInt = 1972;
            Assert.IsTrue(vm.IsChanged);
            Assert.IsTrue(vm.Criteria.HasErrors);
            Assert.IsFalse(vm.SearchCommand.CanExecute());
        }

        [TestMethod]
        public void IsChangePropertyShouldBeFalse()
        {
            EntitySearchViewModel vm = new EntitySearchViewModel();
            Assert.IsFalse(vm.IsChanged);
        }

        [TestMethod]
        public void IsChangePropertyShouldBeTrue()
        {
            EntitySearchViewModel vm = new EntitySearchViewModel();
            vm.MyRequiredProperty = "Change";
            Assert.IsTrue(vm.IsChanged);
        }

        [TestMethod]
        public void IsChangePropertyWithIgnoreChange()
        {
            EntitySearchViewModel vm = new EntitySearchViewModel();
            Assert.IsFalse(vm.IsChanged);
            vm.MyPropertyIgnored = true;
            Assert.IsFalse(vm.IsChanged);
        }

        [TestMethod]
        public void ViewModelLoadedShouldHaveVmIsChangedFalse()
        {
            EntitySearchViewModel vm = new EntitySearchViewModel { MyRequiredProperty = "Required" };
            vm.LoadCommand.Execute();
            Assert.IsFalse(vm.IsChanged);
        }

        [TestMethod]
        public void ClearSearchCriteria()
        {
            EntitySearchViewModel vm = new EntitySearchViewModel();
            Assert.IsFalse(vm.IsChanged);
            vm.MyProperty = true;
            Assert.IsTrue(vm.IsChanged);
            vm.ClearSearchCommand.Execute();
            Assert.IsFalse(vm.IsChanged);
        }
    }
}