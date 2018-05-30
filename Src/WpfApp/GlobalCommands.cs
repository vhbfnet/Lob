using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    /// <summary>
    /// Global commands
    /// </summary>
    public static class GlobalCommands
    {
        // Composite commands
        public static CompositeCommand SaveAllCommand = new CompositeCommand(true);
        public static CompositeCommand CloseAllCommand = new CompositeCommand();

        // Single composite command
        public static CompositeCommand SaveCommand = new CompositeCommand();
        public static CompositeCommand SaveAndCloseCommand = new CompositeCommand();
        public static CompositeCommand CancelCommand = new CompositeCommand();
        public static CompositeCommand DeleteCommand = new CompositeCommand();
        public static CompositeCommand CloseCommand = new CompositeCommand();
        public static CompositeCommand RefreshCommand = new CompositeCommand();
        public static CompositeCommand SearchCommand = new CompositeCommand();
        public static CompositeCommand ClearSearchCommand = new CompositeCommand();
        public static CompositeCommand DetailCommand = new CompositeCommand();
    }
}
