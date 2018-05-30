using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lob.Mvvm
{
    /// <summary>
    /// Interface de la commande d'ouverture d'une fiche en détail
    /// </summary>
    public interface IDetail
    {
        /// <summary>
        /// Commande permettant d'ouvrir une fiche en détail
        /// </summary>
        DelegateCommand DetailCommand { get; }
    }
}
