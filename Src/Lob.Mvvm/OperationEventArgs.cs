using System;
using System.Collections.Generic;
using System.Text;

namespace Lob.Mvvm
{
    /// <summary>
    /// OperationEventArgs 
    /// </summary>
    public class OperationEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the error.
        /// </summary>
        public Exception Error { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the operation has succed or not.
        /// </summary>
        public bool Succeed { get { return (Error == null); } }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationEventArgs" /> class.
        /// </summary>
        public OperationEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationEventArgs" /> class.
        /// </summary>
        /// <param name="exception">Exception in the operation.</param>
        public OperationEventArgs(Exception exception)
        {
            Error = exception;
        }
    }
}
