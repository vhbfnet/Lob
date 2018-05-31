using Lob.Core;
using Lob.Mvvm.Properties;
using Prism.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lob.Mvvm
{
    /// <summary>
    /// Workspace view model
    /// </summary>
    /// <seealso cref="Lob.Mvvm.ViewModelBase" />
    /// <seealso cref="Lob.Mvvm.IHeaderInfo" />
    public class WorkspaceViewModel : ViewModelBase, IHeaderInfo, IChangeTrackingExtended, IBusy, IRevertibleChangeTracking, IRefreshable, ILoadable
    {
        #region Properties

        /// <summary>
        /// Obtient ou définit le titre de la vue
        /// </summary>
        [IgnoreChange]
        public virtual string ViewTitle
        {
            get { return _viewTitle; }
            set
            {
                if (SetProperty(ref _viewTitle, value))
                    OnHeaderChanged();
            }
        }
        private string _viewTitle = Resources.Label_ViewModelTitle;

        /// <summary>
        /// Uri de l'icône de la vue
        /// </summary>
        [IgnoreChange]
        public virtual string ViewUriIcon
        {
            get { return _viewUriIcon; }
            set
            {
                if (SetProperty(ref _viewUriIcon, value))
                    OnHeaderChanged();
            }
        }
        private string _viewUriIcon = Resources.UriIcon_View;

        /// <summary>
        /// Clé du view model
        /// </summary>
        [IgnoreChange]
        public Guid ViewModelKey
        {
            get { return _viewModelkey; }
            set { SetProperty(ref _viewModelkey, value); }
        }
        private Guid _viewModelkey = Guid.NewGuid();

        /// <summary>
        /// Obtient ou définit la date de création de la vue
        /// </summary>
        [IgnoreChange]
        public DateTime ViewCreated
        {
            get { return _viewCreated; }
            set
            {
                if (SetProperty(ref _viewCreated, value))
                    OnHeaderChanged();
            }
        }
        private DateTime _viewCreated = DateTime.Now;

        /// <summary>
        /// Obtient l'état changé de l'objet. Retourne true si changé sinon false
        /// </summary>
        [IgnoreChange]
        public bool IsChanged
        {
            get { return _isChanged; }
            set { SetProperty(ref _isChanged, value); }
        }
        private bool _isChanged;

        /// <summary>
        /// Liste des view models
        /// </summary>
        protected ObservableCollection<WorkspaceViewModel> ViewModels { get; set; }

        #endregion

        #region ViewLoadedCommand

        /// <summary>
        /// Cette commande doit être appelée depuis la vue sur l'événement Loaded
        /// </summary>
        public DelegateCommand ViewLoadedCommand
        {
            get { return _viewLoadedCommand ?? (_viewLoadedCommand = new DelegateCommand(ExecuteViewLoaded, CanExecuteViewLoaded)); }
        }
        private DelegateCommand _viewLoadedCommand;

        /// <summary>
        /// Execute la commande <see cref="ViewLoadedCommand"/>
        /// </summary>
        protected virtual void ExecuteViewLoaded()
        {
            if (!IsViewLoaded)
                IsViewLoaded = true;
        }

        /// <summary>
        /// Determine si la commande <see cref="ViewLoadedCommand"/> peut être executée
        /// </summary>
        /// <returns>True ou false</returns>
        protected virtual bool CanExecuteViewLoaded()
        {
            return !IsViewLoaded;
        }

        #endregion

        #region OnViewLoaded

        /// <summary>
        /// Indique si la vue est chargé ou pas
        /// </summary>
        [IgnoreChange]
        public bool IsViewLoaded
        {
            get { return _isViewLoaded; }
            set
            {
                if (SetProperty(ref _isViewLoaded, value))
                    OnViewLoaded();
            }
        }
        private bool _isViewLoaded;

        /// <summary>
        /// Informe que la valeur de la propriété <see cref="IsViewLoaded"/> a changé
        /// </summary>
        public event EventHandler ViewLoaded;

        /// <summary>
        /// Déclenche l'évenement ViewLoaded
        /// </summary>
        protected virtual void OnViewLoaded()
        {
            ViewLoaded?.Invoke(this, EventArgs.Empty);
            LoadCommand.Execute();
        }

        #endregion

        #region OnHeaderChanged

        /// <summary>
        /// Informe que la valeur de la propriété IsHeaderChanged a changé
        /// </summary>
        public event EventHandler HeaderChanged;

        /// <summary>
        /// Déclenche l'évenement HeaderChanged
        /// </summary>
        protected virtual void OnHeaderChanged()
        {
            HeaderChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region OnChanged

        /// <summary>
        /// Event raised when the object changes
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Déclenche l'événement <see cref="Changed"/>
        /// </summary>
        protected virtual void OnChanged()
        {
            IsChanged = true;

            // Valide le view model
            Validate();

            // Mise à jour du titre
            SetTitle();

            Changed?.Invoke(this, EventArgs.Empty);

            // Refresh commands
            RefreshCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region IsBusyChanged

        /// <summary>
        /// Gets or sets a value indicating whether is busy.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is busy; otherwise, <c>false</c>.
        /// </value>
        [IgnoreChange]
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (SetProperty(ref _isBusy, value))
                    OnIsBusyChanged();
            }
        }
        private bool _isBusy;

        /// <summary>
        /// Gets or sets the content of the busy.
        /// </summary>
        /// <value>
        /// The content of the busy.
        /// </value>
        [IgnoreChange]
        public string BusyContent
        {
            get { return _busyContent; }
            set { SetProperty(ref _busyContent, value); }
        }
        private string _busyContent;

        /// <summary>
        /// Informe que la valeur de la propriété <see cref="IsBusy"/> a changé.
        /// </summary>
        public event EventHandler IsBusyChanged;

        /// <summary>
        /// Déclenche l'événement <see cref="IsBusyChanged"/>
        /// </summary>
        protected virtual void OnIsBusyChanged()
        {
            IsBusyChanged?.Invoke(this, EventArgs.Empty);

            // Refresh Commands
            RefreshCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region ILoadable

        /// <summary>
        /// Evénement déclenché avant le <see cref="Load"/>
        /// </summary>
        public event EventHandler Loading;

        /// <summary>
        /// Déclenche l'événement <see cref="Loading"/>
        /// </summary>
        protected virtual void OnLoading()
        {
            IsBusy = true;
            IsLoading = true;

            Loading?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Evénement déclenché après le <see cref="Load"/>
        /// </summary>
        public event EventHandler<OperationEventArgs> Loaded;

        /// <summary>
        /// Déclenche l'événement <see cref="Loaded"/>
        /// </summary>
        /// <param name="operationEventArgs">Argument de l'événement d'éxecution d'un opération</param>
        protected virtual void OnLoaded(OperationEventArgs operationEventArgs)
        {
            if (operationEventArgs != null && !operationEventArgs.Succeed)
            {
                IsBusy = false;
                IsLoading = false;

                // TODO : ManageException
                //ManageException(operationEventArgs.Error, Resources.Error_Loading);

                //TODO : Close on error
                //CloseCommand.Execute();
                //ShellServices.EventAggregator.GetEvent<RemoveFromJournalPubSubEvent>().Publish(this);

                return;
            }

            SynchronizeChilds();
            SetTitle();
            IsBusy = false;
            IsLoading = false;
            AcceptChanges();

            Loaded?.Invoke(this, operationEventArgs);
        }

        /// <summary>
        /// Commande permettant de charger les données de la vue
        /// </summary>
        public DelegateCommand LoadCommand
        {
            get
            {
                return _loadCommand ?? (_loadCommand = new DelegateCommand(ExecuteLoad, CanExecuteLoad));
            }
        }
        private DelegateCommand _loadCommand;

        /// <summary>
        /// Execute la commande <see cref="LoadCommand"/>. Processus de chargement des données de la vue
        /// </summary>
        protected virtual async void ExecuteLoad()
        {
            Exception exception = null;
            OnLoading();
            BusyContent = Resources.Message_Loading;

            try
            {
                await Load();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                OnLoaded(new OperationEventArgs(exception));
            }
        }

        /// <summary>
        /// Determine si la commande <see cref="LoadCommand"/> peut être executée
        /// </summary>
        /// <returns>True ou false</returns>
        protected virtual bool CanExecuteLoad()
        {
            return !IsLoading && !IsRefreshing;
        }

        /// <summary>
        /// Indique que la commande <see cref="LoadCommand"/> est en cours
        /// </summary>
        [IgnoreChange]
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }
        private bool _isLoading;

        #endregion

        #region IRefreshable

        /// <summary>
        /// Evénement déclenché avant le <see cref="Refresh"/>
        /// </summary>
        public event EventHandler Refreshing;

        /// <summary>
        /// Déclenche l'événement <see cref="Refreshing"/>
        /// </summary>
        protected virtual void OnRefreshing()
        {
            IsBusy = true;
            IsRefreshing = true;

            if (Refreshing != null)
                Refreshing(this, EventArgs.Empty);
        }

        /// <summary>
        /// Evénement déclenché après le <see cref="Refresh"/>
        /// </summary>
        public event EventHandler Refreshed;

        /// <summary>
        /// Déclenche l'événement <see cref="Refreshed"/>
        /// </summary>
        /// <param name="operationEventArgs">Argument de l'événement d'éxecution d'un opération</param>
        protected virtual void OnRefreshed(OperationEventArgs operationEventArgs)
        {
            if (operationEventArgs != null && !operationEventArgs.Succeed)
            {
                IsBusy = false;
                IsRefreshing = false;
                // TODO : ManageException
                //ManageException(operationEventArgs.Error, Resources.Error_Refreshing);

                // TODO: CloseCommand
                //CloseCommand.Execute();
                return;
            }

            SynchronizeChilds();
            SetFocus();
            ViewCreated = DateTime.Now;
            IsBusy = false;
            IsRefreshing = false;

            if (Refreshed != null)
                Refreshed(this, EventArgs.Empty);
        }

        /// <summary>
        /// Commande permettant de raffraîchir les données de la vue
        /// </summary>
        public DelegateCommand RefreshCommand
        {
            get
            {
                return _refreshCommand ?? (_refreshCommand = new DelegateCommand(ExecuteRefresh, CanExecuteRefresh));
            }
        }
        private DelegateCommand _refreshCommand;

        /// <summary>
        /// Execute la commande <see cref="RefreshCommand"/>. Processus de raffraîssichement de la vue
        /// </summary>
        protected virtual async void ExecuteRefresh()
        {
            Exception exception = null;
            OnRefreshing();
            BusyContent = Resources.Message_Refreshing;

            try
            {
                await Refresh();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                OnRefreshed(new OperationEventArgs(exception));
            }
        }

        /// <summary>
        /// Determine si la commande <see cref="RefreshCommand"/> peut être executée
        /// </summary>
        /// <returns>True ou false</returns>
        protected virtual bool CanExecuteRefresh()
        {
            return !IsBusy && !ViewModelsIsBusy();
        }

        /// <summary>
        /// Indique que la commande <see cref="RefreshCommand"/> est en cours
        /// </summary>
        [IgnoreChange]
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }
        private bool _isRefreshing;

        #endregion

        #region IRevertibleChangeTracking

        /// <summary>
        /// Rétablit l'état inchangé de l'objet en rejetant les modifications
        /// </summary>
        public virtual void RejectChanges()
        {
            if (ViewModels != null)
            {
                foreach (var viewModel in ViewModels.OfType<IRevertibleChangeTracking>().ToList())
                {
                    if (viewModel.IsChanged)
                        viewModel.RejectChanges();
                }
            }

            ClearErrors();  // TODO : Comment or not
            IsChanged = false;
        }

        #endregion

        #region Virtuals

        /// <summary>
        /// Resets the object’s state to unchanged by accepting the modifications.
        /// </summary>
        public virtual void AcceptChanges()
        {
            if (ViewModels != null)
            {
                foreach (var viewModel in ViewModels.OfType<IChangeTrackingExtended>().ToList())
                {
                    if (viewModel.IsChanged)
                        viewModel.AcceptChanges();
                }
            }

            IsChanged = false;
        }

        /// <summary>
        /// Mise à jour du titre de la vue. 
        /// Méthode executée suite à l'événement Loaded ou <see cref="Changed"/>
        /// </summary>
        protected virtual void SetTitle()
        {
        }

        /// <summary>
        /// Met le focus sur un champ en utilisant le behavior FocusBehavior. 
        /// Méthode executée suite à l'événement <see cref="Refreshed"/>
        /// </summary>
        protected virtual void SetFocus()
        {
        }

        /// <summary>
        /// Override this to synchronize the model with the childs view models
        /// This method is called after refresh and after save (only is is succeded)
        /// </summary>
        protected virtual void SynchronizeChilds()
        {
        }

        /// <summary>
        /// Processus Refresh async à executer
        /// </summary>
        /// <returns>Task</returns>
        protected virtual async Task Refresh()
        {
            await Task.FromResult(false);
        }

        /// <summary>
        /// Processus Load async à executer
        /// </summary>
        /// <returns>Task</returns>
        protected virtual async Task Load()
        {
            await Task.FromResult(false);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Views the models is busy.
        /// </summary>
        /// <returns></returns>
        protected bool ViewModelsIsBusy()
        {
            return (ViewModels != null && ViewModels.Select(v => v.IsBusy).Aggregate((a, b) => a | b));
        }

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Optimization
            if (!IsBusy) 
            {
                var property = GetType().GetProperty(propertyName);
                if (property?.GetCustomAttributes(typeof(IgnoreChangeAttribute), true).Length == 0)
                    OnChanged();
            }

            base.OnPropertyChanged(propertyName);
        }

        #endregion
    }
}
