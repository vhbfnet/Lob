using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lob.Mvvm
{
    /// <summary>
    /// Interface de la commande de recherche des données
    /// </summary>
    public interface ISearchable
    {
        /// <summary>
        /// This command starts a search.
        /// </summary>
        DelegateCommand SearchCommand { get; }

        /// <summary>
        /// This command starts a search.
        /// </summary>
        DelegateCommand ClearSearchCommand { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the command <see cref="SearchCommand"/> is running.
        /// </summary>
        bool IsSearching { get; set; }

        /// <summary>
        /// Event raised before Search 
        /// </summary>
        event EventHandler Searching;

        /// <summary>
        /// Event raised after Search
        /// </summary>
        event EventHandler<OperationEventArgs> Searched;
    }
}
