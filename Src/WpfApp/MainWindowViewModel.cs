using Lob.Mvvm;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class MainWindowViewModel : ViewModelBase
    {
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
    }
}
