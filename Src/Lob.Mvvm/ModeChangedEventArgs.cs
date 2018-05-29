using System;
using System.Collections.Generic;
using System.Text;

namespace Lob.Mvvm
{
    /// <summary>
    /// Argument de l'événement ModeChanged
    /// </summary>
    public class ModeChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Mode précédent
        /// </summary>
        public Mode PreviousMode { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="previousMode">Mode précedent</param>
        public ModeChangedEventArgs(Mode previousMode)
        {
            PreviousMode = previousMode;
        }
    }
}
