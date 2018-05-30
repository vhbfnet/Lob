using Lob.Core;
using Lob.Mvvm.Properties;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lob.Mvvm
{
    /// <summary>
    /// View model pour la gestion d'une fiche éditable (ajout/modification/supression)
    /// </summary>
    /// <typeparam name="T">Type de l'entité</typeparam>
    public abstract class EditableViewModel<T> : WorkspaceViewModel, ISaveable, ICancelable, IDeletable
        where T : INotifyPropertyChanged, INotifyDataErrorInfo, IEditableObject, IChangeTrackingExtended, IRevertibleChangeTracking, IValidatable, IObjectWithState, new()
    {
        #region Properties

        /// <summary>
        /// Elément à éditer
        /// </summary>
        [IgnoreChange]
        public T Model
        {
            get { return _model; }
            set
            {
                if (!ReferenceEquals(value, _model))
                {
                    var olValue = _model;
                    _model = value;
                    OnPropertyChanged();
                    OnModelInitialized(olValue);
                }
            }
        }
        private T _model;

        /// <summary>
        /// Get or set the icon to show in new <see cref="Mode"/> 
        /// </summary>
        [IgnoreChange]
        public string ViewUriIconNew
        {
            get { return _viewUriIconNew; }
            set { SetProperty(ref _viewUriIconNew, value); }
        }
        private string _viewUriIconNew;

        /// <summary>
        /// Get or set the icon to show in edit <see cref="Mode"/>  
        /// </summary>
        [IgnoreChange]
        public string ViewUriIconEdit
        {
            get { return _viewUriIconEdit; }
            set { SetProperty(ref _viewUriIconEdit, value); }
        }
        private string _viewUriIconEdit;

        #endregion

        #region ISaveable

        /// <summary>
        /// Commande permettant de sauvegarder la vue
        /// </summary>
        public DelegateCommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new DelegateCommand(ExecuteSave, CanExecuteSave)); }
        }
        private DelegateCommand _saveCommand;

        /// <summary>
        /// Execute la commande <see cref="SaveCommand"/>. Processus de sauvegarde de la vue
        /// </summary>
        protected virtual async void ExecuteSave()
        {
            Exception exception = null;
            OnSaving();
            BusyContent = Resources.Message_Saving;
            try
            {
                await Save();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                OnSaved(new OperationEventArgs(exception));
            }
        }

        /// <summary>
        /// Determine si la commande <see cref="SaveCommand"/> peut être executée
        /// </summary>
        /// <returns>True ou false</returns>
        protected virtual bool CanExecuteSave()
        {
            var isActive = !IsBusy && !HasErrors && IsChanged && !ViewModelsIsBusy() && (!Equals(Model, default(T)) && !Model.HasErrors);
            SaveCommand.IsActive = isActive;
            return isActive;
        }

        /// <summary>
        /// Command to save a view and to close it if no errors
        /// </summary>
        public DelegateCommand SaveAndCloseCommand
        {
            get { return _saveAndCloseCommand ?? (_saveAndCloseCommand = new DelegateCommand(ExecuteSaveAndCloseCommand, CanExecuteSaveAndCloseCommand)); }
        }
        private DelegateCommand _saveAndCloseCommand;

        /// <summary>
        /// Execute la commande <see cref="SaveCommand"/>. Processus de sauvegarde de la vue
        /// </summary>
        protected virtual void ExecuteSaveAndCloseCommand()
        {
            ExecuteSave();
            // TODO : ExecuteSaveAndCloseCommand
            //Saved += (s, e) => { if (e.Error == null) CloseCommand.Execute(); };
        }

        /// <summary>
        /// Determine si la commande <see cref="SaveAndCloseCommand"/> peut être executée
        /// </summary>
        /// <returns>True ou false</returns>
        protected virtual bool CanExecuteSaveAndCloseCommand()
        {
            var isActive = !IsBusy && !HasErrors && IsChanged && !ViewModelsIsBusy();
            SaveAndCloseCommand.IsActive = isActive;
            return isActive;
        }

        /// <summary>
        /// Indicate that the view is saving 
        /// </summary>
        [IgnoreChange]
        public bool IsSaving
        {
            get { return _isSaving; }
            set { SetProperty(ref _isSaving, value); }
        }
        private bool _isSaving;

        /// <summary>
        /// Evénement déclenché avant le <see cref="Save"/>
        /// </summary>
        public event EventHandler Saving;

        /// <summary>
        /// Déclenche l'événement <see cref="Saving"/>
        /// </summary>
        protected virtual void OnSaving()
        {
            IsBusy = true;
            IsSaving = true;

            if (Saving != null)
                Saving(this, EventArgs.Empty);
        }

        /// <summary>
        /// Evénement déclenché après le <see cref="Save"/>
        /// </summary>
        public event EventHandler<OperationEventArgs> Saved;

        /// <summary>
        /// Déclenche l'événement <see cref="Saved"/>
        /// </summary>
        protected virtual void OnSaved(OperationEventArgs operationEventArgs)
        {
            if (operationEventArgs != null)
            {
                if (operationEventArgs.Succeed)
                {
                    Mode = Mode.Edit;
                    SynchronizeChilds();
                    AcceptChanges();
                    // TODO : NotificationService
                    //ShellServices.NotificationService.NotifyInformation(true, GetHashCode(), Resources.Message_SaveSucceded);
                }
                else
                {
                    IsSaving = false;
                    IsBusy = false;
                    // TODO : ManageException
                    //ManageException(operationEventArgs.Error, Resources.Error_Saving);
                }
            }

            IsSaving = false;
            IsBusy = false;

            if (Saved != null)
                Saved(this, operationEventArgs);
        }

        #endregion

        #region ICancelable

        /// <summary>
        /// Indique que la commande <see cref="CancelCommand"/> est en cours
        /// </summary>
        [IgnoreChange]
        public bool IsCanceling
        {
            get { return _isCanceling; }
            set { SetProperty(ref _isCanceling, value); }
        }
        private bool _isCanceling;

        /// <summary>
        /// Commande permettant d'annuler les modifications de la vue
        /// </summary>
        public DelegateCommand CancelCommand
        {
            get { return _cancelCommand ?? (_cancelCommand = new DelegateCommand(ExecuteCancel, CanExecuteCancel)); }
        }
        private DelegateCommand _cancelCommand;

        /// <summary>
        /// Execute la commande <see cref="CancelCommand"/>. Annule les modifications de la vue
        /// </summary>
        protected virtual void ExecuteCancel()
        {
            IsBusy = true;
            IsCanceling = true;

            if (!Equals(Model, default(T)))
                Model.RejectChanges();

            if (ViewModels != null)
                foreach (var viewModel in ViewModels.OfType<ICancelable>())
                    viewModel.CancelCommand.Execute();

            RejectChanges();
            IsCanceling = false;
            IsBusy = false;
        }

        /// <summary>
        /// Determine si la commande <see cref="CancelCommand"/> peut être executée
        /// </summary>
        /// <returns>True ou false</returns>
        protected virtual bool CanExecuteCancel()
        {
            return !IsBusy && IsChanged && !ViewModelsIsBusy();
        }

        #endregion

        #region IDeletable

        /// <summary>
        /// Commande permettant de supprimer la vue
        /// </summary>
        public DelegateCommand DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand = new DelegateCommand(ExecuteDelete, CanExecuteDelete)); }
        }
        private DelegateCommand _deleteCommand;

        /// <summary>
        /// Execute la commande <see cref="DeleteCommand"/>. Processus de supression de la vue
        /// </summary>
        protected virtual async void ExecuteDelete()
        {
            if (!OnDeleting(new OperationCancelEventArgs()))
            {
                Exception exception = null;
                IsDeleting = true;
                BusyContent = Resources.Message_Deleting;

                try
                {
                    await Delete();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                finally
                {
                    OnDeleted(new OperationEventArgs(exception));
                }
            }
        }

        /// <summary>
        /// Determine si la commande <see cref="DeleteCommand"/> peut être executée
        /// </summary>
        /// <returns>True ou false</returns>
        protected virtual bool CanExecuteDelete()
        {
            var isActive = (!IsBusy && !IsChanged && (Mode == Mode.Edit || Mode == Mode.Delete)) && !ViewModelsIsBusy();
            DeleteCommand.IsActive = isActive;
            return isActive;
        }

        /// <summary>
        /// Indique que la commande <see cref="DeleteCommand"/> est en cours
        /// </summary>
        [IgnoreChange]
        public bool IsDeleting
        {
            get { return _isDeleting; }
            set { SetProperty(ref _isDeleting, value); }
        }
        private bool _isDeleting;

        /// <summary>
        /// Evénement déclenché avant le <see cref="Delete"/>
        /// </summary>
        public event EventHandler<OperationCancelEventArgs> Deleting;

        /// <summary>
        /// Déclenche l'événement <see cref="Deleting"/>
        /// </summary>
        protected virtual bool OnDeleting(OperationCancelEventArgs args)
        {
            if (args == null)
                return true;

            // Si ça n'a pas été annulé
            if (args.Cancel == false)
            {
                //TODO: Confirm message
                // Demande de confirmation
                //if (!args.CancelDefaultMessage)
                //    args.Cancel = !ShellServices.MessageService.ShowYesNo(string.Format(CultureInfo.InvariantCulture, Resources.Message_DeleteConfirm, ViewTitle));

                Deleting?.Invoke(this, args);
            }

            return args.Cancel;
        }

        /// <summary>
        /// Evénement déclenché après le <see cref="Delete"/>
        /// </summary>
        public event EventHandler<OperationEventArgs> Deleted;

        /// <summary>
        /// Déclenche l'événement <see cref="Deleted"/>
        /// </summary>
        protected virtual void OnDeleted(OperationEventArgs operationEventArgs)
        {
            if (operationEventArgs != null)
            {
                if (operationEventArgs.Succeed)
                {
                    // TODO : Close view
                    //ShellServices.NotificationService.NotifyInformation(true, GetHashCode(), Resources.Message_SaveSucceded);

                    //Closed += (s, e) => ShellServices.EventAggregator.GetEvent<RemoveFromJournalPubSubEvent>().Publish(this);

                    //// Close the view
                    //CloseCommand.Execute();
                }
                else
                {
                    IsDeleting = false;
                    IsBusy = false;

                    // TODO : Manage exception
                    //ManageException(operationEventArgs.Error, Resources.Error_Deleting);
                }
            }

            IsDeleting = false;
            IsBusy = false;

            Deleted?.Invoke(this, operationEventArgs);
        }

        #endregion

        #region IEditableObject

        readonly Dictionary<PropertyInfo, object> _storedProperties = new Dictionary<PropertyInfo, object>();
        bool _isEditing; // Only to manage many times calls

        /// <summary>
        /// Commence la modification d'un objet
        /// </summary>
        public virtual void BeginEdit()
        {
            if (!_isEditing)
            {
                var propertyInfos = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead && p.CanWrite);

                foreach (var property in propertyInfos)
                {
                    if (!IsEnumerableType(property.PropertyType) && !IsCollectionType(property.PropertyType))
                    {
                        var copyValue = property.GetValue(this, null);
                        _storedProperties[property] = copyValue;
                    }
                }
                _isEditing = true;
            }
        }

        /// <summary>
        /// Ignore les modifications apportées depuis le dernier appel BeginEdit()
        /// </summary>
        public virtual void CancelEdit()
        {
            if (_isEditing)
            {
                foreach (var pair in _storedProperties)
                {
                    pair.Key.SetValue(this, pair.Value, null);
                }
                _isEditing = false;
            }
        }

        /// <summary>
        /// Exécute un push sur des modifications apportées depuis le dernier appel System.ComponentModel.IEditableObject.BeginEdit()
        /// </summary>
        public virtual void EndEdit()
        {
            if (_isEditing)
            {
                _storedProperties.Clear();
                _isEditing = false;
            }
        }

        #endregion

        #region ModelInitialized

        /// <summary>
        /// Evénement déclenché lorque <see cref="Model"/> change de valeur
        /// </summary>
        public event EventHandler<ModelInitializedEventArgs> ModelInitialized;

        /// <summary>
        /// Déclenche l'événement <see cref="ModelInitialized"/>
        /// </summary>
        protected virtual void OnModelInitialized(T oldValue)
        {
            ModelInitialized?.Invoke(this, new ModelInitializedEventArgs(oldValue));

            if (!Equals(oldValue, default(T)))
            {
                oldValue.PropertyChanged -= Model_PropertyChanged;
                oldValue.ErrorsChanged -= Model_ErrorsChanged;
                //oldValue.Changed -= Model_Changed;
            }

            if (!Equals(Model, default(T)))
            {
                Model.PropertyChanged += Model_PropertyChanged;
                Model.ErrorsChanged += Model_ErrorsChanged;
                //Model.Changed += Model_Changed;
                Model.AcceptChanges();
            }
        }

        #endregion

        #region ModeChanged

        /// <summary>
        /// Indique le <see cref="Mode"/> de la vue
        /// </summary>
        [IgnoreChange]
        public Mode Mode
        {
            get { return _mode; }
            set
            {
                if (_mode != value)
                {
                    var previousMode = _mode;
                    _mode = value;
                    OnModeChanged(previousMode);
                    OnPropertyChanged();
                }
            }
        }
        private Mode _mode;

        /// <summary>
        /// Evénement déclenché au changement du <see cref="Mode"/>
        /// </summary>
        public event EventHandler ModeChanged;

        /// <summary>
        /// Déclenche l'événement <see cref="ModeChanged"/>
        /// </summary>
        protected virtual void OnModeChanged(Mode previousMode)
        {
            switch (Mode)
            {
                case Mode.None:
                    break;
                case Mode.New:
                    ViewUriIcon = (string.IsNullOrEmpty(ViewUriIconNew)) ? Resources.UriIcon_ViewNew : ViewUriIconNew;
                    break;
                case Mode.Edit:
                    ViewUriIcon = (string.IsNullOrEmpty(ViewUriIconEdit)) ? Resources.UriIcon_ViewEdit : ViewUriIconEdit;
                    break;
                case Mode.Delete:
                    break;
            }

            // Refresh commands
            DeleteCommand.RaiseCanExecuteChanged();

            if (ModeChanged != null)
                ModeChanged(this, new ModeChangedEventArgs(previousMode));
        }

        #endregion

        #region Event ModelPropertyChanged

        /// <summary>
        /// Evénement ModelPropertyChanged
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs> ModelPropertyChanged;

        /// <summary>
        /// Déclenche l'événement <see cref="ModelPropertyChanged"/> si une propriété de <see cref="Model"/> change
        /// </summary>
        protected virtual void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var property = sender.GetType().GetProperty(e.PropertyName);
            if (property?.GetCustomAttributes(typeof(IgnoreChangeAttribute), true).Length == 0)
            {
                OnChanged();
                ModelPropertyChanged?.Invoke(sender, e);
            }
        }

        #endregion

        #region Event ModelErrorsChanged

        /// <summary>
        /// Evénement ModelErrorsChanged
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ModelErrorsChanged;

        /// <summary>
        /// Déclenche l'événement <see cref="ModelErrorsChanged"/> si une propiété de <see cref="Model"/> contient une erreur
        /// </summary>
        protected virtual void OnModelErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            ModelErrorsChanged?.Invoke(sender, e);

            if (sender != null && e != null)
                OnErrorsChanged(sender, e.PropertyName);

            // Refresh commands
            SaveCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Overrides

        protected override void OnIsBusyChanged()
        {
            base.OnIsBusyChanged();

            // Refresh commands
            SaveCommand.RaiseCanExecuteChanged();
            SaveAndCloseCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
            // TODO: CloseCommand
            //CloseCommand.RaiseCanExecuteChanged();
        }

        protected override void OnChanged()
        {
            base.OnChanged();

            // Refresh commands
            SaveCommand.RaiseCanExecuteChanged();
            SaveAndCloseCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Rétablit l'état inchangé de l'objet en acceptant les modifications
        /// </summary>
        public override void AcceptChanges()
        {
            if (!Equals(Model, default(T)))
            {
                Model.ObjectState = ObjectState.Unchanged;
                Model.AcceptChanges();
            }

            base.AcceptChanges();
            EndEdit();
            BeginEdit();
        }

        /// <summary>
        /// Rétablit l'état inchangé de l'objet en rejetant les modifications
        /// </summary>
        public override void RejectChanges()
        {
            base.RejectChanges();
            CancelEdit();
            BeginEdit();

            // Refresh commands
            SaveCommand.RaiseCanExecuteChanged();
            SaveAndCloseCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
        }

        public override void Validate()
        {
            if (Model != null)
                Model.Validate();
            base.Validate();
        }

        /// <summary>
        /// Liste de propriétes techniques à ignorer pour l'événement Changed
        /// </summary>
        /// <param name="propertyName">Nom de la propriété</param>
        /// <returns>True ou false</returns>
        protected bool ExcludeThisProperty(string propertyName)
        {
            return propertyName != nameof(Model) &&
                   propertyName != nameof(ViewModelKey) &&
                   propertyName != nameof(IsChanged) &&
                   propertyName != nameof(IsDeleting) &&
                   propertyName != nameof(IsBusy) &&
                   propertyName != nameof(IsRefreshing) &&
                   propertyName != nameof(IsCanceling) &&
                   propertyName != nameof(BusyContent) &&
                   propertyName != nameof(Mode) &&
                   propertyName != nameof(ViewUriIcon) &&
                   propertyName != nameof(ViewTitle) &&
                   propertyName != nameof(IsSaving) &&
                   propertyName != "Items" &&
                   propertyName != "IsActive" &&
                   propertyName != "CurrentItem" &&
                   propertyName != "ObjectState" &&
                   propertyName != "IsViewLoaded" &&
                   propertyName != "IsLoading" &&
                   propertyName != "IsSearching" &&
                   propertyName != "IsClosing" &&
                   propertyName != "ShowContext";
        }

        /// <summary>
        /// Processus Load async à executer
        /// </summary>
        /// <returns>Task</returns>
        protected override async Task Load()
        {
            await base.Load();
        }

        protected override bool CanExecuteRefresh()
        {
            return base.CanExecuteRefresh() && !IsChanged;
        }

        #endregion

        #region Virtual

        /// <summary>
        /// Processus Save async à executer
        /// </summary>
        /// <returns>Task</returns>
        protected virtual async Task Save()
        {
            await Task.FromResult(false);
        }

        /// <summary>
        /// Processus Delete async à executer
        /// </summary>
        /// <returns>Task</returns>
        protected virtual async Task Delete()
        {
            await Task.FromResult(false);
        }

        #endregion

        #region Private

        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(ObjectState))
            {
                if ((Model.ObjectState == ObjectState.Detached || Model.ObjectState == ObjectState.Unchanged))
                    Model.ObjectState = ObjectState.Modified;

                // Raise the event ModelPropertyChanged
                OnModelPropertyChanged(sender, e);
            }
        }

        private void Model_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            // Raise the event ModelErrorsChanged
            OnModelErrorsChanged(sender, e);
        }

        bool IsEnumerableType(Type type)
        {
            return type.IsGenericType && (type.GetInterface("IEnumerable") != null);
        }

        bool IsCollectionType(Type type)
        {
            return type.IsGenericType && (type.GetInterface("ICollection") != null);
        }

        #endregion
    }
}
