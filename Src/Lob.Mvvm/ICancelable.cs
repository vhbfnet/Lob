using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lob.Mvvm
{
    /// <summary>
    /// Interface de la commande d'annulation des modifications d'une vue
    /// </summary>
    public interface ICancelable
    {
        /// <summary>
        /// Commande permettant d'annuler les modifications de la vue
        /// </summary>
        DelegateCommand CancelCommand { get; }

        /// <summary>
        /// Indique que la commande <see cref="CancelCommand"/> est en cours
        /// </summary>
        bool IsCanceling { get; set; }
    }
}
