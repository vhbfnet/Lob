using System;
using System.Collections.Generic;
using System.Text;

namespace Lob.Mvvm
{
    /// <summary>
    /// Indique les différents modes d'une fiche éditable
    /// </summary>
    public enum Mode
    {
        /// <summary>
        /// Valeur par défaut
        /// </summary>
        None,
        /// <summary>
        /// Mode création
        /// </summary>
        New,
        /// <summary>
        /// Mode modification
        /// </summary>
        Edit,
        /// <summary>
        /// Mode suppression
        /// </summary>
        Delete
    }
}
