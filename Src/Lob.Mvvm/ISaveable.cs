using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lob.Mvvm
{
    /// <summary>
    /// Interface de la commande de sauvegarde d'une vue
    /// </summary>
    public interface ISaveable
    {
        /// <summary>
        /// Commande permettant de sauvegarder la vue
        /// </summary>
        DelegateCommand SaveCommand { get; }

        /// <summary>
        /// Command to save a view and to close it if no errors
        /// </summary>
        DelegateCommand SaveAndCloseCommand { get; }

        /// <summary>
        /// Indique que la commande <see cref="SaveCommand"/> est en cours
        /// </summary>
        bool IsSaving { get; set; }

        /// <summary>
        /// Evénement déclenché avant le Save
        /// </summary>
        event EventHandler Saving;

        /// <summary>
        /// Evénement déclenché après le Save
        /// </summary>
        event EventHandler<OperationEventArgs> Saved;
    }
}
