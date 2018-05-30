using System;
using System.Collections.Generic;
using System.Text;

namespace Lob.Mvvm
{
    /// <summary>
    /// Argument de l'événement ItemsInitialized
    /// </summary>
    public class ItemsInitializedEventArgs : EventArgs
    {
        /// <summary>
        /// Valeur précendente
        /// </summary>
        public object PreviousValue { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="previousValue">Valeur précendente</param>
        public ItemsInitializedEventArgs(object previousValue)
        {
            PreviousValue = previousValue;
        }
    }
}
