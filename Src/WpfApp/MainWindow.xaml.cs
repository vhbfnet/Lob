using Lob.Mvvm;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ISaveable saveable;
            ICancelable cancelable;
            IRefreshable refreshable;
            ISearchable searchable;
            IDetail detail;
            IDeletable deletable;

            if (e.RemovedItems.Count > 0)
            {
                if (e.RemovedItems[0] is TabItem fe)
                {
                    if (!(fe.Content is ContentControl cc))
                        return;

                    var viewModel = cc.DataContext;

                    saveable = viewModel as ISaveable;
                    if (saveable != null)
                    {
                        UnregisterSingleCommand(GlobalCommands.SaveCommand, saveable.SaveCommand);
                        UnregisterSingleCommand(GlobalCommands.SaveAndCloseCommand, saveable.SaveAndCloseCommand);
                    }

                    cancelable = viewModel as ICancelable;
                    if (cancelable != null)
                        UnregisterSingleCommand(GlobalCommands.CancelCommand, cancelable.CancelCommand);

                    refreshable = viewModel as IRefreshable;
                    if (refreshable != null)
                        UnregisterSingleCommand(GlobalCommands.RefreshCommand, refreshable.RefreshCommand);

                    searchable = viewModel as ISearchable;
                    if (searchable != null)
                    {
                        UnregisterSingleCommand(GlobalCommands.SearchCommand, searchable.SearchCommand);
                        UnregisterSingleCommand(GlobalCommands.ClearSearchCommand, searchable.ClearSearchCommand);
                    }

                    detail = viewModel as IDetail;
                    if (detail != null)
                        UnregisterSingleCommand(GlobalCommands.DetailCommand, detail.DetailCommand);

                    deletable = viewModel as IDeletable;
                    if (deletable != null)
                        UnregisterSingleCommand(GlobalCommands.DeleteCommand, deletable.DeleteCommand);
                }
            }

            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0] is TabItem fe)
                {
                    if (!(fe.Content is ContentControl cc))
                        return;

                    var viewModel = cc.DataContext;

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
            }
        }

        private void RegisterSingleCommand(CompositeCommand compositeCommand, ICommand command)
        {
            if (compositeCommand.RegisteredCommands.Count > 0)
                compositeCommand.UnregisterCommand(compositeCommand.RegisteredCommands[0]);
            compositeCommand.RegisterCommand(command);
        }

        private void UnregisterSingleCommand(CompositeCommand compositeCommand, ICommand command)
        {
            if (compositeCommand.RegisteredCommands.Contains(command))
                compositeCommand.UnregisterCommand(command);
        }
    }
}
