using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Lob.Core
{
    /// <summary>
    /// <see cref="System.ComponentModel.IChangeTracking"/> extension
    /// </summary>
    public interface IChangeTrackingExtended : IChangeTracking
    {
        /// <summary>
        /// Event raised when the object changes
        /// </summary>
        event EventHandler Changed;
    }
}
