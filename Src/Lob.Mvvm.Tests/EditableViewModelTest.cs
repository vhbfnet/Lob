using System;
using Lob.Mvvm.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lob.Mvvm.Tests
{
    [TestClass]
    public class EditableViewModelTest : ViewModelCoreTest
    {
        [TestMethod]
        public void CanExecuteRefreshShouldBeTrue()
        {
            EntityEditableViewModel vm = new EntityEditableViewModel();
            Assert.IsTrue(vm.RefreshCommand.CanExecute());
        }

        [TestMethod]
        public void CanExecuteRefreshShouldBeFalse()
        {
            EntityEditableViewModel vm = new EntityEditableViewModel();
            vm.MyRequiredProperty = "Something";
            Assert.IsTrue(vm.IsChanged);
            Assert.IsFalse(vm.RefreshCommand.CanExecute());
        }

        [TestMethod]
        public void IsChangePropertyShouldBeFalse()
        {
            EntityEditableViewModel vm = new EntityEditableViewModel();
            Assert.IsFalse(vm.IsChanged);
        }

        [TestMethod]
        public void IsChangePropertyShouldBeTrue()
        {
            EntityEditableViewModel vm = new EntityEditableViewModel();
            vm.Model = new Entity();
            vm.Model.MyRequiredString = "Change";
            Assert.IsTrue(vm.IsChanged);
        }

        [TestMethod]
        public void IsChangePropertyWithIgnoreChange()
        {
            EntityEditableViewModel vm = new EntityEditableViewModel();
            vm.MyPropertyIgnored = true;
            Assert.IsFalse(vm.IsChanged);
        }


        [TestMethod]
        public void ViewModelHasErrorsShouldBeFalse()
        {
            EntityEditableViewModel vm = new EntityEditableViewModel();
            vm.Model = new Entity();
            vm.MyRequiredProperty = "Required";
            vm.Model.MyRequiredString = "Change";
            Assert.IsFalse(vm.HasErrors);
        }

        [TestMethod]
        public void ViewModelHasErrorsShoulBeTrueOnModelChanged()
        {
            EntityEditableViewModel vm = new EntityEditableViewModel();
            vm.Model = new Entity();
            vm.Model.MyInt = 1972;
            Assert.IsTrue(vm.HasErrors);
        }

        [TestMethod]
        public void ViewModelHasErrorsShoulBeTrueOnVmChanged()
        {
            EntityEditableViewModel vm = new EntityEditableViewModel();
            vm.MyProperty = true;
            Assert.IsTrue(vm.HasErrors);
        }

        [TestMethod]
        public void ExecuteCancel()
        {
            EntityEditableViewModel vm = new EntityEditableViewModel();
            vm.MyRequiredProperty = "Something";
            vm.Model = new Entity();
            Assert.IsTrue(vm.IsChanged);
            Assert.IsFalse(vm.HasErrors);
            vm.CancelCommand.Execute();
            Assert.IsFalse(vm.IsChanged);
            Assert.IsFalse(vm.HasErrors);
        }

        [TestMethod]
        public void ExecuteCancelWithErrorsInVm()
        {
            EntityEditableViewModel vm = new EntityEditableViewModel();
            vm.MyProperty = true;
            Assert.IsTrue(vm.IsChanged);
            Assert.IsTrue(vm.HasErrors);
            vm.CancelCommand.Execute();
            Assert.IsFalse(vm.IsChanged);
            Assert.IsFalse(vm.HasErrors);
        }

        [TestMethod]
        public void ExecuteCancelWithErrorsInModel()
        {
            EntityEditableViewModel vm = new EntityEditableViewModel();
            vm.Model = new Entity();
            vm.MyProperty = true;
            Assert.IsTrue(vm.IsChanged);
            Assert.IsTrue(vm.HasErrors);
            Assert.IsTrue(vm.Model.HasErrors);
            vm.CancelCommand.Execute();
            Assert.IsFalse(vm.IsChanged);
            Assert.IsFalse(vm.HasErrors);
            Assert.IsFalse(vm.Model.HasErrors);
        }

        [TestMethod]
        public void CanExecuteCancelShoulBeFalse()
        {
            EntityEditableViewModel vm = new EntityEditableViewModel();
            vm.Model = new Entity();
            Assert.IsFalse(vm.IsChanged);
            Assert.IsFalse(vm.CancelCommand.CanExecute());
        }

        [TestMethod]
        public void CanExecuteCancelShoulBeTrue()
        {
            EntityEditableViewModel vm = new EntityEditableViewModel();
            vm.MyRequiredProperty = "Something";
            vm.Model = new Entity();
            vm.Model.MyRequiredString = "Required";
            Assert.IsTrue(vm.IsChanged);
            Assert.IsFalse(vm.HasErrors);
            Assert.IsFalse(vm.Model.HasErrors);
            Assert.IsTrue(vm.CancelCommand.CanExecute());
        }

        [TestMethod]
        public void CanExecuteSaveShoudBeFalseIfVmHasErrors()
        {
            EntityEditableViewModel vm = new EntityEditableViewModel();
            vm.MyProperty = true;
            Assert.IsFalse(vm.SaveCommand.CanExecute());
        }

        [TestMethod]
        public void CanExecuteSaveShoudBeFalseIfModelHasErrors()
        {
            EntityEditableViewModel vm = new EntityEditableViewModel();
            vm.Model = new Entity();
            vm.Model.MyInt = 1972;
            Assert.IsFalse(vm.SaveCommand.CanExecute());
        }

        [TestMethod]
        public void CanExecuteSaveShoudBeTrue()
        {
            EntityEditableViewModel vm = new EntityEditableViewModel();
            vm.Model = new Entity();
            vm.MyRequiredProperty = "Something";
            vm.Model.MyRequiredString = "My string";
            Assert.IsTrue(vm.SaveCommand.CanExecute());
        }

        [TestMethod]
        public void ExecuteSave()
        {
            EntityEditableViewModel vm = new EntityEditableViewModel();
            vm.Model = new Entity();
            vm.MyRequiredProperty = "Something";
            vm.Model.MyRequiredString = "My string";
            Assert.IsTrue(vm.IsChanged);
            Assert.IsFalse(vm.HasErrors);
            Assert.IsFalse(vm.Model.HasErrors);
            Assert.IsTrue(vm.SaveCommand.CanExecute());
            vm.SaveCommand.Execute();
            Assert.IsFalse(vm.IsChanged);
        }

        [TestMethod]
        public void ExecuteDeleteIfModeDelete()
        {
            EntityEditableViewModel vm = new EntityEditableViewModel();
            vm.Mode = Mode.Delete;
            Assert.IsFalse(vm.IsChanged);
            Assert.IsFalse(vm.HasErrors);
            Assert.IsTrue(vm.DeleteCommand.CanExecute());
            vm.DeleteCommand.Execute();
            Assert.IsFalse(vm.IsChanged);
            //Assert.IsFalse(vm.IsClosing);
        }

        [TestMethod]
        public void ExecuteDeleteIfModeEdit()
        {
            EntityEditableViewModel vm = new EntityEditableViewModel();
            vm.Mode = Mode.Edit;
            Assert.IsFalse(vm.IsChanged);
            Assert.IsFalse(vm.HasErrors);
            Assert.IsTrue(vm.DeleteCommand.CanExecute());
            vm.DeleteCommand.Execute();
            Assert.IsFalse(vm.IsChanged);
            //Assert.IsFalse(vm.IsClosing);
        }

        [TestMethod]
        public void ModelCancelEdit()
        {
            Entity model = new Entity { MyRequiredString = "Required" };
            model.AcceptChanges();
            Assert.IsFalse(model.IsChanged);
            Assert.IsTrue(model.MyRequiredString == "Required");
            model.MyRequiredString = "Required changed";
            Assert.IsTrue(model.IsChanged);
            model.CancelEdit();
            Assert.IsTrue(model.MyRequiredString == "Required");
        }

        [TestMethod]
        public void ViewModelCancelEdit()
        {
            EntityEditableViewModel vm = new EntityEditableViewModel { MyRequiredProperty = "Required" };
            vm.AcceptChanges();
            Assert.IsFalse(vm.IsChanged);
            Assert.IsTrue(vm.MyRequiredProperty == "Required");
            vm.MyRequiredProperty = "Required changed";
            Assert.IsTrue(vm.IsChanged);
            vm.CancelEdit();
            Assert.IsTrue(vm.MyRequiredProperty == "Required");
        }

        [TestMethod]
        public void ViewModelLoadedShouldHaveVmIsChangedFalse()
        {
            EntityEditableViewModel vm = new EntityEditableViewModel { MyRequiredProperty = "Required" };
            vm.LoadCommand.Execute();
            Assert.IsFalse(vm.IsChanged);
        }

        [TestMethod]
        public void ViewModelLoadedShouldHaveModelIsChangedFalse()
        {
            EntityEditableViewModel vm = new EntityEditableViewModel { MyRequiredProperty = "Required" };
            vm.Model = new Entity { MyRequiredString = "Required" };
            vm.LoadCommand.Execute();
            Assert.IsFalse(vm.IsChanged);
            Assert.IsFalse(vm.Model.IsChanged);
        }
    }
}