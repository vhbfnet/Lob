using System;
using System.Collections.Generic;
using System.Text;

namespace Lob.Mvvm
{
    /// <summary>
    /// Header info
    /// </summary>
    public interface IHeaderInfo
    {
        /// <summary>
        /// Evénement déclenché lorqu'une des propriétés de l'entête change
        /// </summary>
        event EventHandler HeaderChanged;

        /// <summary>
        /// Obtient ou définit le titre de la vue
        /// </summary>
        string ViewTitle { get; set; }

        /// <summary>
        /// Obtient ou définit icône du titre de la vue
        /// </summary>
        string ViewUriIcon { get; set; }

        /// <summary>
        /// Obtient ou définit la date de création de la vue
        /// </summary>
        DateTime ViewCreated { get; set; }
    }
}
