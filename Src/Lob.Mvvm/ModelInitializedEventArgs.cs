using System;
using System.Collections.Generic;
using System.Text;

namespace Lob.Mvvm
{
    /// <summary>
    /// Argument de l'événement ModelInitialized 
    /// </summary>
    public class ModelInitializedEventArgs : EventArgs
    {
        /// <summary>
        /// Valeur précendente
        /// </summary>
        public object PreviousValue { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="previousValue">Ancienne valeur</param>
        public ModelInitializedEventArgs(object previousValue)
        {
            PreviousValue = previousValue;
        }
    }
}
