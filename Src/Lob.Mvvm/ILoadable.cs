using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lob.Mvvm
{
    /// <summary>
    /// Interface de la commande de chargement des données
    /// </summary>
    public interface ILoadable
    {
        /// <summary>
        /// Commande permettant de charger les données de la vue
        /// </summary>
        DelegateCommand LoadCommand { get; }

        /// <summary>
        /// Indique que la commande <see cref="LoadCommand"/> est en cours
        /// </summary>
        bool IsLoading { get; set; }

        /// <summary>
        /// Evénement déclenché avant le Load
        /// </summary>
        event EventHandler Loading;

        /// <summary>
        /// Evénement déclenché après le Load
        /// </summary>
        event EventHandler<OperationEventArgs> Loaded;
    }
}
