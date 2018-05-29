using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Lob.Core
{
    /// <summary>
    /// Base calss thats implement <seealso cref="System.ComponentModel.INotifyDataErrorInfo"/>
    /// </summary>
    [DataContract]
    public abstract class NotifyDataErrorObject : NotifyObject, INotifyDataErrorInfo, IValidatable
    {
        [NonSerialized]
        public static bool ClientSideValidation = true;

        public ObservableCollection<ErrorInfo> Errors { get; private set; }

        #region INotifyDataErrorInfo

        [NonSerialized]
        private object _lock;

        [NonSerialized]
        private ConcurrentDictionary<string, List<string>> _errors;

        /// <summary>
        /// Evénement levé lorsque qu'une propriété de l'objet contient des erreurs de validation
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Déclenche l'événement <see cref="ErrorsChanged"/>
        /// </summary>        
        /// <param name="sender">Source de l'événement</param>
        /// <param name="propertyName">Propriété contenant l'erreur</param>
        protected virtual void OnErrorsChanged(object sender, string propertyName)
        {
            if (sender is INotifyDataErrorInfo notifyDataErrorInfo)
                AddOrUpdateError(sender.GetHashCode(), propertyName, notifyDataErrorInfo.GetErrors(propertyName));

            ErrorsChanged?.Invoke(sender, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Renvoie les erreurs de validation de l'objet
        /// </summary>
        /// <param name="propertyName">Nom de la propriété</param>
        /// <returns>Liste d'erreurs</returns>
        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return _errors.Select(s => s.Value);

            List<string> errorsForName;
            _errors.TryGetValue(propertyName, out errorsForName);

            return errorsForName;
        }

        /// <summary>
        /// Indique si l'objet contient des erreurs
        /// </summary>
        public virtual bool HasErrors
        {
            get
            {
                return (_errors != null) && _errors.Any(kv => kv.Value != null && kv.Value.Count > 0);
            }
        }

        /// <summary>
        /// Valide l'entité
        /// </summary>
        public virtual void Validate()
        {
            if (!ClientSideValidation)
                return;

            lock (_lock)
            {
                var validationContext = new ValidationContext(this, null, null);
                var validationResults = new List<ValidationResult>();
                Validator.TryValidateObject(this, validationContext, validationResults, true);

                foreach (var prop in _errors.ToList())
                {
                    if (validationResults.All(r => r.MemberNames.All(m => m != prop.Key)))
                    {
                        List<string> outList;
                        _errors.TryRemove(prop.Key, out outList);
                        OnErrorsChanged(this, prop.Key);
                    }
                }

                var query = from r in validationResults
                            from m in r.MemberNames
                            group r by m into g
                            select g;

                foreach (var prop in query)
                {
                    var messages = prop.Select(r => r.ErrorMessage).ToList();
                    if (_errors.ContainsKey(prop.Key))
                    {
                        List<string> outList;
                        _errors.TryRemove(prop.Key, out outList);
                    }

                    _errors.TryAdd(prop.Key, messages);
                    OnErrorsChanged(this, prop.Key);
                }
            }
        }

        /// <summary>
        /// Efface les erreurs de validation
        /// </summary>
        public virtual void ClearErrors()
        {
            var errorsList = _errors.ToList();
            for (int i = errorsList.Count - 1; i >= 0; i--)
            {
                List<string> outList;
                _errors.TryRemove(errorsList[i].Key, out outList);
                OnErrorsChanged(this, errorsList[i].Key);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Contructeur par défaut
        /// </summary>
        protected NotifyDataErrorObject()
        {
            _lock = new object();
            _errors = new ConcurrentDictionary<string, List<string>>();
            Errors = new ObservableCollection<ErrorInfo>();
        }

        #endregion

        #region Public

        public void AddError(string propertyName, List<string> messages)
        {
            _errors.TryAdd(propertyName, messages);
            OnErrorsChanged(this, propertyName);
        }

        #endregion

        #region Overrides

        //protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    base.OnPropertyChanged(propertyName);
        //    Validate();
        //}

        protected void AddOrUpdateError(int key, string propertyName, IEnumerable errorMessages)
        {
            var errorInfo = Errors.FirstOrDefault(er => er.Key.Equals(key) && er.PropertyName == propertyName);
            if (errorInfo == null)
            {
                errorInfo = new ErrorInfo { Key = key, PropertyName = propertyName, ErrorMessages = errorMessages };
                Errors.Add(errorInfo);
            }
            else
            {
                if (errorMessages == null)
                    Errors.Remove(errorInfo);
                else
                    errorInfo.ErrorMessages = errorMessages;
            }
        }

        #endregion

        #region Private

        [OnDeserializing]
        private void OnDeserializingMethod(StreamingContext context)
        {
            _lock = new object();
            _errors = new ConcurrentDictionary<string, List<string>>();
        }

        #endregion
    }
}
