using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lob.Mvvm
{
    /// <summary>
    /// Interface de la commande de suppression d'une vue
    /// </summary>
    public interface IDeletable
    {
        /// <summary>
        /// Commande permettant de supprimer la vue
        /// </summary>
        DelegateCommand DeleteCommand { get; }

        /// <summary>
        /// Indique que la commande <see cref="DeleteCommand"/> est en cours
        /// </summary>
        bool IsDeleting { get; set; }

        /// <summary>
        /// Evénement déclenché avant le Delete
        /// </summary>
        event EventHandler<OperationCancelEventArgs> Deleting;

        /// <summary>
        /// Evénement déclenché après le Delete
        /// </summary>
        event EventHandler<OperationEventArgs> Deleted;
    }
}
