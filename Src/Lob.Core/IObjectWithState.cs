using System;
using System.Collections.Generic;
using System.Text;

namespace Lob.Core
{
    /// <summary>
    /// Interface définissant l'état d'une entité.
    /// </summary>
    public interface IObjectWithState
    {
        /// <summary>
        /// Indentifiant du contexte d'un appel de service.
        /// </summary>
        Guid ContextId { get; }

        /// <summary>
        /// Obtient ou définit l'état de l'entité.
        /// </summary>
        /// <seealso cref="Lob.Core.ObjectState"/>
        ObjectState ObjectState { get; set; }
    }
}
