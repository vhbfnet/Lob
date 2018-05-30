using Lob.Core;
using Lob.Mvvm.Properties;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lob.Mvvm
{
    /// <summary>
    /// View model pour la gestion des recherches
    /// </summary>
    /// <typeparam name="T">Type de l'entité</typeparam>
    /// <typeparam name="TCriteria">Type de l'entité criteria</typeparam>
    public abstract class SearchViewModel<T, TCriteria> : ListViewModel<T>, ISearchable
        where T : IValidatable, new()
        where TCriteria : IValidatable, INotifyPropertyChanged, INotifyDataErrorInfo, IChangeTracking, IRevertibleChangeTracking, new()
    {
        #region Properties

        /// <summary>
        /// Gets or sets the default search key.
        /// </summary>
        /// <value>
        /// The default search key.
        /// </value>
        [IgnoreChange]
        public string SearchKey
        {
            get { return _searchKey; }
            set { SetProperty(ref _searchKey, value); }
        }
        private string _searchKey;

        /// <summary>
        /// Gets or sets the criteria.
        /// </summary>
        /// <value>
        /// The criteria.
        /// </value>
        [IgnoreChange]
        public TCriteria Criteria
        {
            get { return _criteria; }
            set
            {
                if (!ReferenceEquals(value, _criteria))
                {
                    var olValue = _criteria;
                    _criteria = value;
                    OnPropertyChanged();
                    OnCriteriaInitialized(olValue);
                }
            }
        }
        private TCriteria _criteria;

        /// <summary>
        /// Gets or sets a value indicating navigation auto if only one item in the items' list.
        /// </summary>
        /// <value>
        ///   <c>true</c> to navigate auto.
        /// </value>
        [IgnoreChange]
        public bool NavigateAutoIfOnlyOneItem
        {
            get { return _navigateAutoIfOnlyOneItem; }
            set { SetProperty(ref _navigateAutoIfOnlyOneItem, value); }
        }
        private bool _navigateAutoIfOnlyOneItem = true;

        #endregion

        #region ISearchable

        /// <summary>
        /// Evénement déclenché avant le <see cref="Search"/>
        /// </summary>
        public event EventHandler Searching;

        /// <summary>
        /// Déclenche l'événement <see cref="Searching"/>
        /// </summary>
        protected virtual void OnSearching()
        {
            IsBusy = true;
            IsSearching = true;

            if (Searching != null)
                Searching(this, EventArgs.Empty);
        }

        /// <summary>
        /// Evénement déclenché après le <see cref="Search"/>
        /// </summary>
        public event EventHandler<OperationEventArgs> Searched;

        /// <summary>
        /// Déclenche l'événement <see cref="Searched"/>
        /// </summary>
        protected virtual void OnSearched(OperationEventArgs operationEventArgs)
        {
            if (operationEventArgs != null && !operationEventArgs.Succeed)
            {
                IsSearching = false;
                IsBusy = false;
                // TODO : ManageException
                //ManageException(operationEventArgs.Error, Resources.Error_Searching);
            }

            IsSearching = false;
            IsBusy = false;
            SetFocus();

            if (NavigateAutoIfOnlyOneItem && operationEventArgs.Succeed && Items.Count == 1)
            {
                CurrentItem = Items.First();
                DetailCommand.Execute();
            }

            Searched?.Invoke(this, operationEventArgs);
        }

        /// <summary>
        /// Indique que la commande <see cref="SearchCommand"/> est en cours
        /// </summary>
        [IgnoreChange]
        public bool IsSearching
        {
            get { return _isSearching; }
            set { SetProperty(ref _isSearching, value); }
        }
        private bool _isSearching;

        #region SearchCommand

        /// <summary>
        /// Commande permettant de rechercher des éléments de type T
        /// </summary>
        public DelegateCommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new DelegateCommand(ExecuteSearch, CanExecuteSearch));
            }
        }
        private DelegateCommand _searchCommand;

        /// <summary>
        /// Execute la commande <see cref="SearchCommand"/>. Processus de recherche
        /// </summary>
        protected virtual async void ExecuteSearch()
        {
            Exception exception = null;
            OnSearching();
            BusyContent = Resources.Message_Searching;

            try
            {
                await Search();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                OnSearched(new OperationEventArgs(exception));
            }
        }

        /// <summary>
        /// Determine si la commande <see cref="SearchCommand"/> peut être executée
        /// </summary>
        /// <returns>True ou false</returns>
        protected virtual bool CanExecuteSearch()
        {
            return !IsBusy && !HasErrors;
        }

        #endregion

        #region ClearSearchCommand

        /// <summary>
        /// Commande permettant d'effacer les critères de rechercher
        /// </summary>
        public DelegateCommand ClearSearchCommand
        {
            get
            {
                return _clearSearchCommand ?? (_clearSearchCommand = new DelegateCommand(ExecuteClearSearch, CanExecuteClearSearch));
            }
        }
        private DelegateCommand _clearSearchCommand;

        /// <summary>
        /// Execute la commande <see cref="ClearSearchCommand"/>.
        /// </summary>
        protected virtual void ExecuteClearSearch()
        {
            Exception exception = null;

            try
            {
                ClearSearch();
                AcceptChanges();
            }
            catch (Exception ex)
            {
                exception = ex;
                // TODO : ManageException
                //ManageException(ex);
            }
        }

        /// <summary>
        /// Determine si la commande <see cref="ClearSearchCommand"/> peut être executée
        /// </summary>
        /// <returns>True ou false</returns>
        protected virtual bool CanExecuteClearSearch()
        {
            return true;
        }

        #endregion

        #endregion

        #region Events

        #region CriteriaInitialized

        /// <summary>
        /// Evénement déclenché lorque <see cref="Criteria"/> change de valeur
        /// </summary>
        public event EventHandler<ModelInitializedEventArgs> CriteriaInitialized;

        /// <summary>
        /// Déclenche l'événement <see cref="CriteriaInitialized"/>
        /// </summary>
        protected virtual void OnCriteriaInitialized(TCriteria oldValue)
        {
            CriteriaInitialized?.Invoke(this, new ModelInitializedEventArgs(oldValue));

            if (!Equals(oldValue, default(TCriteria)))
            {
                oldValue.PropertyChanged -= Criteria_PropertyChanged;
                oldValue.ErrorsChanged -= Criteria_ErrorsChanged;
                //oldValue.Changed -= Model_Changed;
            }

            if (!Equals(Criteria, default(T)))
            {
                Criteria.PropertyChanged += Criteria_PropertyChanged;
                Criteria.ErrorsChanged += Criteria_ErrorsChanged;
                //Model.Changed += Model_Changed;
                Criteria.AcceptChanges();
            }
        }

        #endregion

        #region Event CriteriaPropertyChanged

        /// <summary>
        /// Evénement CriteriaPropertyChanged
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs> CriteriaPropertyChanged;

        /// <summary>
        /// Déclenche l'événement <see cref="CriteriaPropertyChanged"/> si une propriété de <see cref="Criteria"/> change
        /// </summary>
        protected virtual void OnCriteriaPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var property = sender.GetType().GetProperty(e.PropertyName);
            if (property?.GetCustomAttributes(typeof(IgnoreChangeAttribute), true).Length == 0)
            {
                OnChanged();
                CriteriaPropertyChanged?.Invoke(sender, e);
            }
        }

        #endregion

        #region Event CriteriaErrorsChanged

        /// <summary>
        /// Evénement ModelErrorsChanged
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> CriteriaErrorsChanged;

        /// <summary>
        /// Déclenche l'événement <see cref="CriteriaErrorsChanged"/> si une propiété de <see cref="Criteria"/> contient une erreur
        /// </summary>
        protected virtual void OnCriteriaErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            CriteriaErrorsChanged?.Invoke(sender, e);

            if (sender != null && e != null)
                OnErrorsChanged(sender, e.PropertyName);

            // Refresh commands
            SearchCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #endregion

        #region Construtors

        public SearchViewModel()
        {
            Criteria = new TCriteria();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Uri de l'icône de la vue
        /// </summary>
        [IgnoreChange]
        public override string ViewUriIcon
        {
            get { return _viewUriIcon; }
            set { SetProperty(ref _viewUriIcon, value); }
        }
        private string _viewUriIcon = Resources.UriIcon_ViewSearch;

        /// <summary>
        /// Titre de la vue
        /// </summary>
        [IgnoreChange]
        public override string ViewTitle
        {
            get { return _viewTitle; }
            set
            {
                if (SetProperty(ref _viewTitle, value))
                    OnHeaderChanged();
            }
        }
        private string _viewTitle = Resources.Lable_SearchTitle;

        /// <summary>
        /// Déclenche l'événement IsBusyChanged
        /// </summary>
        protected override void OnIsBusyChanged()
        {
            base.OnIsBusyChanged();

            // Refresh commands
            SearchCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Met le focus sur un champ en utilisant le behavior FocusBehavior. 
        /// Méthode executée suite à l'événement <see cref="Refreshed"/> ou <see cref="Searched"/>
        /// </summary>
        protected override void SetFocus()
        {
            base.SetFocus();
        }

        public override void Validate()
        {
            if (Criteria != null)
                Criteria.Validate();

            base.Validate();
        }

        #endregion

        #region Virtual

        /// <summary>
        /// Processus Search async à executer
        /// </summary>
        /// <returns>Task</returns>
        protected virtual Task Search()
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// Processus ClearSearch async à executer
        /// </summary>
        /// <returns>Task</returns>
        protected virtual void ClearSearch()
        {
            if (Items != null)
                Items.Clear();

            // Reset criteria
            Criteria.RejectChanges();
        }

        #endregion

        #region Private

        private void Criteria_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Raise the event ModelPropertyChanged
            OnCriteriaPropertyChanged(sender, e);
        }

        private void Criteria_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            // Raise the event ModelErrorsChanged
            OnCriteriaErrorsChanged(sender, e);
        }

        #endregion
    }
}
