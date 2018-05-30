using System;
using System.Collections.Generic;
using System.Text;

namespace Lob.Mvvm
{
    /// <summary>
    /// Argument de l'événement CurrentItemChanged
    /// </summary>
    public class CurrentItemChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Valeur précendente
        /// </summary>
        public object PreviousValue { get; set; }

        /// <summary>
        /// Contructeur
        /// </summary>
        /// <param name="previousValue">Valeur précendente</param>
        public CurrentItemChangedEventArgs(object previousValue)
        {
            PreviousValue = previousValue;
        }
    }
}
