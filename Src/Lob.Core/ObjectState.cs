using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Lob.Core
{
    /// <summary>
    /// Définit les différents états possibles d'une entité
    /// </summary>
    [DataContract]
    [Flags]
    public enum ObjectState
    {
        /// <summary>
        /// Etat détaché
        /// </summary>
        [EnumMember(Value = "1")]
        Detached = 1,
        /// <summary>
        /// Etat inchangé
        /// </summary>
        [EnumMember(Value = "2")]
        Unchanged = 2,
        /// <summary>
        /// Etat ajouté
        /// </summary>
        [EnumMember(Value = "4")]
        Added = 4,
        /// <summary>
        /// Etat supprimé
        /// </summary>
        [EnumMember(Value = "8")]
        Deleted = 8,
        /// <summary>
        /// Etat modifié
        /// </summary>
        [EnumMember(Value = "16")]
        Modified = 16
    }
}
