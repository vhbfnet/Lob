using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lob.Mvvm
{
    /// <summary>
    /// Interface de la commande de raffraîssiment des données
    /// </summary>
    public interface IRefreshable
    {
        /// <summary>
        /// Commande permettant de raffraîchir les données de la vue
        /// </summary>
        DelegateCommand RefreshCommand { get; }

        /// <summary>
        /// Indique que la commande <see cref="RefreshCommand"/> est en cours
        /// </summary>
        bool IsRefreshing { get; set; }

        /// <summary>
        /// Evénement déclenché avant le Refresh
        /// </summary>
        event EventHandler Refreshing;

        /// <summary>
        /// Evénement déclenché après le Refresh
        /// </summary>
        event EventHandler Refreshed;
    }
}
