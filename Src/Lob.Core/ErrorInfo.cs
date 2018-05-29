using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Lob.Core
{
    /// <summary>
    /// Classe de base décrivant le contract des erreurs de validation
    /// </summary>
    [DataContract]
    public class ErrorInfo : NotifyObject
    {
        /// <summary>
        /// Identifiant du message
        /// </summary>
        [DataMember]
        public object Key
        {
            get { return _key; }
            set { SetProperty(ref _key, value); }
        }
        private object _key;

        /// <summary>
        /// Nom de la propriété en erreur
        /// </summary>
        [DataMember]
        public string PropertyName
        {
            get { return _propertyName; }
            set { SetProperty(ref _propertyName, value); }
        }
        private string _propertyName;

        /// <summary>
        /// Liste des erreurs de validation
        /// </summary>
        [DataMember]
        public IEnumerable ErrorMessages
        {
            get { return _errorMessages; }
            set { SetProperty(ref _errorMessages, value); }
        }
        private IEnumerable _errorMessages;
    }
}
