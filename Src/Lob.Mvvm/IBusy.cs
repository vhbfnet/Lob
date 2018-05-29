using System;
using System.Collections.Generic;
using System.Text;

namespace Lob.Mvvm
{
    /// <summary>
    /// Busy indicator
    /// </summary>
    public interface IBusy
    {
        /// <summary>
        /// Gets or sets the content of the busy.
        /// </summary>
        /// <value>
        /// The content of the busy.
        /// </value>
        string BusyContent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is busy.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is busy; otherwise, <c>false</c>.
        /// </value>
        bool IsBusy { get; set; }

        /// <summary>
        /// Occurs when [is busy changed].
        /// </summary>
        event EventHandler IsBusyChanged;
    }
}
