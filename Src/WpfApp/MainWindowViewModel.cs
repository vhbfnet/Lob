using Lob.Mvvm;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    /// <summary>
    /// Main view model
    /// </summary>
    /// <seealso cref="Lob.Mvvm.ViewModelBase" />
    public class MainWindowViewModel : ViewModelBase
    {
        ViewModelBase _previousViewModel;
        
        #region SelectionChangedCommand

        public DelegateCommand<ViewModelBase> SelectionChangedCommand
        {
            get { return _selectionChangedCommand ?? (_selectionChangedCommand = new DelegateCommand<ViewModelBase>(ExecuteSelectionChangedCommand, CanExecuteSelectionChangedCommand)); }
        }
        private DelegateCommand<ViewModelBase> _selectionChangedCommand;

        public virtual void ExecuteSelectionChangedCommand(ViewModelBase viewModel)
        {
            SetActionControl(viewModel);
        }

        public virtual bool CanExecuteSelectionChangedCommand(ViewModelBase viewModel)
        {
            return true;
        }

        #endregion

        #region Composites Commands

        public CompositeCommand SaveAllCommand
        {
            get
            {
                return _saveAllCommand ?? (_saveAllCommand = GlobalCommands.SaveAllCommand);
            }
        }
        private CompositeCommand _saveAllCommand;

        public CompositeCommand CloseAllCommand
        {
            get
            {
                return _closeAllCommand ?? (_closeAllCommand = GlobalCommands.CloseAllCommand);
            }
        }
        private CompositeCommand _closeAllCommand;

        public CompositeCommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = GlobalCommands.RefreshCommand);
            }
        }
        private CompositeCommand _refreshCommand;

        public CompositeCommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = GlobalCommands.SearchCommand);
            }
        }
        private CompositeCommand _searchCommand;

        public CompositeCommand ClearSearchCommand
        {
            get
            {
                return _clearSearchCommand ?? (_clearSearchCommand = GlobalCommands.ClearSearchCommand);
            }
        }
        private CompositeCommand _clearSearchCommand;

        public CompositeCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = GlobalCommands.SaveCommand);
            }
        }
        private CompositeCommand _saveCommand;

        public CompositeCommand SaveAndCloseCommand
        {
            get
            {
                return _saveAndCloseCommand ?? (_saveAndCloseCommand = GlobalCommands.SaveAndCloseCommand);
            }
        }
        private CompositeCommand _saveAndCloseCommand;

        public CompositeCommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = GlobalCommands.CancelCommand);
            }
        }
        private CompositeCommand _cancelCommand;

        public CompositeCommand DeleteCommand
        {
            get
            {
                return _deleteCommand ?? (_deleteCommand = GlobalCommands.DeleteCommand);
            }
        }
        private CompositeCommand _deleteCommand;

        public CompositeCommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = GlobalCommands.CloseCommand);
            }
        }
        private CompositeCommand _closeCommand;

        public CompositeCommand DetailCommand
        {
            get
            {
                return _detailCommand ?? (_detailCommand = GlobalCommands.DetailCommand);
            }
        }
        private CompositeCommand _detailCommand;

        #endregion

        private void SetActionControl(ViewModelBase viewModel)
        {
            ISaveable saveable;
            ICancelable cancelable;
            IRefreshable refreshable;
            ISearchable searchable;
            IDetail detail;
            IDeletable deletable;

            if (_previousViewModel != null)
            {
                saveable = _previousViewModel as ISaveable;
                if (saveable != null)
                {
                    UnregisterSingleCommand(GlobalCommands.SaveCommand, saveable.SaveCommand);
                    UnregisterSingleCommand(GlobalCommands.SaveAndCloseCommand, saveable.SaveAndCloseCommand);
                }

                cancelable = _previousViewModel as ICancelable;
                if (cancelable != null)
                    UnregisterSingleCommand(GlobalCommands.CancelCommand, cancelable.CancelCommand);

                refreshable = _previousViewModel as IRefreshable;
                if (refreshable != null)
                    UnregisterSingleCommand(GlobalCommands.RefreshCommand, refreshable.RefreshCommand);

                searchable = _previousViewModel as ISearchable;
                if (searchable != null)
                {
                    UnregisterSingleCommand(GlobalCommands.SearchCommand, searchable.SearchCommand);
                    UnregisterSingleCommand(GlobalCommands.ClearSearchCommand, searchable.ClearSearchCommand);
                }

                detail = _previousViewModel as IDetail;
                if (detail != null)
                    UnregisterSingleCommand(GlobalCommands.DetailCommand, detail.DetailCommand);

                deletable = _previousViewModel as IDeletable;
                if (deletable != null)
                    UnregisterSingleCommand(GlobalCommands.DeleteCommand, deletable.DeleteCommand);
            }


            _previousViewModel = viewModel;
            saveable = viewModel as ISaveable;
            if (saveable != null)
            {
                RegisterSingleCommand(GlobalCommands.SaveCommand, saveable.SaveCommand);
                RegisterSingleCommand(GlobalCommands.SaveAndCloseCommand, saveable.SaveAndCloseCommand);
            }

            cancelable = viewModel as ICancelable;
            if (cancelable != null)
                RegisterSingleCommand(GlobalCommands.CancelCommand, cancelable.CancelCommand);

            refreshable = viewModel as IRefreshable;
            if (refreshable != null)
                RegisterSingleCommand(GlobalCommands.RefreshCommand, refreshable.RefreshCommand);

            searchable = viewModel as ISearchable;
            if (searchable != null)
            {
                RegisterSingleCommand(GlobalCommands.SearchCommand, searchable.SearchCommand);
                RegisterSingleCommand(GlobalCommands.ClearSearchCommand, searchable.ClearSearchCommand);
            }

            detail = viewModel as IDetail;
            if (detail != null)
                RegisterSingleCommand(GlobalCommands.DetailCommand, detail.DetailCommand);

            deletable = viewModel as IDeletable;
            if (deletable != null)
                RegisterSingleCommand(GlobalCommands.DeleteCommand, deletable.DeleteCommand);

        }

        private void RegisterSingleCommand(CompositeCommand compositeCommand, DelegateCommand command)
        {
            if (compositeCommand.RegisteredCommands.Count > 0)
                compositeCommand.UnregisterCommand(compositeCommand.RegisteredCommands[0]);
            compositeCommand.RegisterCommand(command);
        }

        private void UnregisterSingleCommand(CompositeCommand compositeCommand, DelegateCommand command)
        {
            if (compositeCommand.RegisteredCommands.Contains(command))
                compositeCommand.UnregisterCommand(command);
        }
    }
}
