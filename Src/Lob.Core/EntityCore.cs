using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Lob.Core
{
    /// <summary>
    /// Classe de base générique des POCO
    /// </summary>
    /// <typeparam name="T">Type de l'entité</typeparam>
    [DataContract]
    public class EntityCore<T> : NotifyDataErrorObject, IObjectWithState, IEditableObject, IRevertibleChangeTracking, IChangeTrackingExtended, ICloneable
    {
        #region IEditableObject

        [NonSerialized]
        private Dictionary<PropertyInfo, object> _storedProperties;

        [NonSerialized]
        bool _isEditing; // Only to manage many times calls

        [NonSerialized]
        private bool _isCancelEdit;

        /// <summary>
        /// Démarre le tracking des changements de l'entité
        /// </summary>
        public void BeginEdit()
        {
            if (!_isEditing)
            {
                var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead && p.CanWrite);

                foreach (var property in propertyInfos)
                {
                    var copyValue = property.GetValue(this, null);
                    _storedProperties[property] = copyValue;
                }

                _isEditing = true;
            }
        }

        /// <summary>
        /// Annule les changements de l'entité
        /// </summary>
        public void CancelEdit()
        {
            if (_isEditing)
            {
                _isCancelEdit = true;
                foreach (var pair in _storedProperties)
                {
                    pair.Key.SetValue(this, pair.Value, null);
                }
                _isCancelEdit = false;
                _isEditing = false;
            }
        }

        /// <summary>
        /// Termine le tracking des changements de l'entité
        /// </summary>
        public void EndEdit()
        {
            if (_isEditing)
            {
                // WPF controls (DataGrid,...) call EndEdit, and we dont want to execute EndEdit in EditableListViewModel
                // So EndEdit is executed only if the caller method is AcceptChanges 
                var sf = new System.Diagnostics.StackFrame(1);
                if (sf.GetMethod().Name == "AcceptChanges")
                {
                    _storedProperties.Clear();
                    _isEditing = false;
                }
            }
        }

        #endregion

        #region IRevertibleChangeTracking

        /// <summary>
        /// Rejecte tous les changements de l'entité
        /// </summary>
        public void RejectChanges()
        {
            ClearErrors();
            CancelEdit();
            BeginEdit();
            IsChanged = false;
        }

        #endregion

        #region IChangeTrackingExtended

        [NonSerialized]
        private bool _isChanged;

        /// <summary>
        /// Evénement déclenché lorsque l'entité change
        /// </summary>
        [field: NonSerialized]
        public event EventHandler Changed;

        /// <summary>
        /// Soulève l'événement <see cref="Changed"/>
        /// </summary>
        protected virtual void OnChanged()
        {
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }

        /// <summary>
        /// Accepte tous les changements de l'entité
        /// </summary>
        public void AcceptChanges()
        {
            EndEdit();
            BeginEdit();
            IsChanged = false;
        }

        /// <summary>
        /// Obtient ou définit l'état changé ou pas de l'entité
        /// </summary>
        [NotMapped]
        public bool IsChanged
        {
            get
            {
                return _isChanged;
            }
            protected set
            {
                if (value != _isChanged)
                {
                    _isChanged = value;
                    OnChanged();
                }
            }
        }

        #endregion

        #region IObjectWithState

        /// <summary>
        /// Id du contexte d'appel
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [DataMember]
        [NotMapped]
        public Guid ContextId
        {
            get { return _contextId; }
            set { SetProperty(ref _contextId, value); }
        }
        private Guid _contextId = Guid.NewGuid();

        /// <summary>
        /// Etat de l'entité <see cref="ObjectState"/>
        /// </summary>
        [DataMember]
        [NotMapped]
        public ObjectState ObjectState
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }
        private ObjectState _state;

        #endregion

        #region Cloneable

        /// <summary>
        /// Creates a sallow copy of the object
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public EntityCore()
        {
            _storedProperties = new Dictionary<PropertyInfo, object>();
            _state = ObjectState.Added;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Valide l'entité
        /// </summary>
        public override void Validate()
        {
            if (_isCancelEdit)
                return;

            base.Validate();
        }

        /// <summary>
        /// Notifie que la propriété a changé
        /// </summary>
        /// <param name="propertyName"></param>
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (IsChanged == false)
                IsChanged = true;

            base.OnPropertyChanged(propertyName);
        }

        #endregion

        //protected static void RegisterMetaData<TEntity, TMetadata>()
        //    where TEntity : class
        //    where TMetadata : class
        //{
        //    TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(TEntity), typeof(TMetadata)), typeof(TEntity));
        //}

        [OnDeserializing]
        private void OnDeserializingMethod(StreamingContext context)
        {
            _storedProperties = new Dictionary<PropertyInfo, object>();
        }
    }
}
