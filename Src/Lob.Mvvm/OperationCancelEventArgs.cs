using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Lob.Mvvm
{
    /// <summary>
    /// OperationCancelEventArgs
    /// </summary>
    public class OperationCancelEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Gets or sets the value indicating whether the default message should be displayed or not.
        /// True to not display the message
        /// </summary>
        public bool CancelDefaultMessage { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCancelEventArgs" /> class.
        /// </summary>
        public OperationCancelEventArgs()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationCancelEventArgs" /> class.
        /// </summary>
        /// <param name="cancel">True to cancel the event</param>
        public OperationCancelEventArgs(bool cancel) : base(cancel)
        {

        }
    }
}
