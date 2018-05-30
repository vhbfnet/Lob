using Lob.Mvvm.Properties;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Lob.Mvvm
{
    /// <summary>
    /// View model pour la gestion d'une liste d'éléments  
    /// </summary>
    /// <typeparam name="T">Type de l'entité</typeparam>
    public abstract class ListViewModel<T> : WorkspaceViewModel, IItem<T>, IDetail
        where T : new()
    {
        #region Properties

        /// <summary>
        /// Elément courant de la liste
        /// </summary>
        [IgnoreChange]
        public T CurrentItem
        {
            get { return _currentItem; }
            set
            {
                if (!ReferenceEquals(value, _currentItem))
                {
                    var olValue = _currentItem;
                    _currentItem = value;
                    OnCurrentItemChanged(olValue);
                    OnPropertyChanged();
                }
            }
        }
        private T _currentItem;

        /// <summary>
        /// Liste de items de type T
        /// </summary>
        [IgnoreChange]
        public ObservableCollection<T> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new ObservableCollection<T>();
                    OnItemsInitialized(null);
                }

                //return _items;

                return _items; // ?? (_items = new ObservableCollection<T>());
            }
            set
            {
                if (!ReferenceEquals(value, _items))
                {
                    var olValue = _items;
                    _items = value;
                    OnItemsInitialized(olValue);
                    OnPropertyChanged();
                }
            }
        }
        private ObservableCollection<T> _items;

        /// <summary>
        /// Liste des items selectionnés
        /// </summary>
        public ObservableCollection<T> SelectedItems
        {
            get { return _selectedItems ?? (_selectedItems = new ObservableCollection<T>()); }
            set { SetProperty(ref _selectedItems, value); }
        }
        private ObservableCollection<T> _selectedItems;

        #endregion

        #region Construtors

        public ListViewModel()
        {
            Items = new ObservableCollection<T>();
        }

        #endregion

        #region CurrentItemChanged

        /// <summary>
        /// Evénement indiquant que l'élément courant a changé
        /// </summary>
        public event EventHandler<CurrentItemChangedEventArgs> CurrentItemChanged;

        /// <summary>
        /// Déclenche l'événement <see cref="CurrentItemChanged"/>
        /// </summary>
        /// <param name="oldItem">Ancienne valeur</param>
        protected virtual void OnCurrentItemChanged(T oldItem)
        {
            if (CurrentItem != null && !SelectedItems.Contains(CurrentItem))
                _selectedItems.Add(CurrentItem);

            CurrentItemChanged?.Invoke(this, new CurrentItemChangedEventArgs(oldItem));

            if (oldItem is INotifyPropertyChanged npc)
                npc.PropertyChanged -= CurrentItem_PropertyChanged;

            npc = CurrentItem as INotifyPropertyChanged;
            if (npc != null)
                npc.PropertyChanged += CurrentItem_PropertyChanged;

            // TODO :Refresh commands
            //DetailCommand.RaiseCanExecuteChanged();
        }

        void CurrentItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCurrentItemPropertyChanged(sender, e);
        }

        #endregion

        #region CurrentItemPropertyChanged

        /// <summary>
        /// Informe qu'une propriété de l'élément courant a changé
        /// </summary>
        public event EventHandler<PropertyChangedEventArgs> CurrentItemPropertyChanged;

        /// <summary>
        /// Déclenche l'événement <see cref="CurrentItemPropertyChanged"/>
        /// </summary>
        protected virtual void OnCurrentItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CurrentItemPropertyChanged?.Invoke(sender, e);
        }

        #endregion

        #region Event CurrentItemErrorsChanged

        /// <summary>
        /// Informe qu'une propriété de l'élément courant contient des erreurs de validation
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> CurrentItemErrorsChanged;

        /// <summary>
        /// Déclenche l'événement <see cref="CurrentItemErrorsChanged"/>
        /// </summary>
        protected virtual void OnCurrentItemErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            CurrentItemErrorsChanged?.Invoke(sender, e);
        }

        #endregion

        #region Event SelectedItemsChanged

        /// <summary>
        /// Evénement SelectedItemsChanged
        /// </summary>
        public event EventHandler SelectedItemsChanged;

        /// <summary>
        /// Déclenche l'événement SelectedItemsChanged
        /// </summary>
        protected virtual void OnSelectedItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            SelectedItemsChanged?.Invoke(this, e);

            // Refresh Command
            DetailCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region ItemsInitialized

        /// <summary>
        /// Evénement déclenché lorque <see cref="Items"/> change de valeur
        /// </summary>
        public event EventHandler<ItemsInitializedEventArgs> ItemsInitialized;

        /// <summary>
        /// Déclenche l'événement <see cref="ItemsInitialized"/>
        /// </summary>
        /// <param name="oldValue">Ancienne valeur de la collection</param>
        protected virtual void OnItemsInitialized(ICollection<T> oldValue)
        {
            if (oldValue != null)
                SelectedItems.CollectionChanged -= SelectedItems_CollectionChanged;

            SelectedItems.CollectionChanged += SelectedItems_CollectionChanged;

            ItemsInitialized?.Invoke(this, new ItemsInitializedEventArgs(oldValue));
        }

        #endregion

        #region DetailCommand

        /// <summary>
        /// Commande permettant d'afficher le détail des éléments sélectionnés
        /// </summary>
        public DelegateCommand DetailCommand
        {
            get { return _detailCommand ?? (_detailCommand = new DelegateCommand(ExecuteDetailCommand, CanExecuteDetailCommand)); }
        }
        private DelegateCommand _detailCommand;

        /// <summary>
        /// Execute la commande <see cref="DetailCommand"/>
        /// </summary>
        protected virtual void ExecuteDetailCommand()
        {
            foreach (var item in SelectedItems)
            {
                var actionName = GetDetailAction(item);

                // TODO : Execute detail
                //if (ShellServices.RoutesService.RouteExist(actionName))
                //{
                //    var navigationParameters = new NavigationParameters();
                //    navigationParameters.Add("Mode", Mode.Edit.ToString());

                //    foreach (var key in GetKeyAttribute(item))
                //        navigationParameters.Add("ModelKey" + key.Key, key.Value);

                //    ShellServices.NavigationService.Navigate(actionName, navigationParameters);
                //}
                //else
                //    ShellServices.NotificationService.NotifyInformation(true, string.Format(Resources.Info_ConventionViewDetail, actionName));
            }
        }

        /// <summary>
        /// Defines the action name to navigate to the detail view.
        /// Default convention is EntitityNameEdit
        /// </summary>
        /// <param name="item">Current selected item</param>
        /// <returns>A navigation action name</returns>
        protected virtual string GetDetailAction(T item)
        {
            return string.Format("{0}Edit", item.GetType().Name); // Action by convention
        }

        /// <summary>
        /// Determine si la commande <see cref="DetailCommand"/> peut être executée
        /// </summary>
        /// <returns>True ou false</returns>
        protected virtual bool CanExecuteDetailCommand()
        {
            return (SelectedItems != null && SelectedItems.Count > 0);
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
        private string _viewUriIcon = Resources.UriIcon_ViewList;

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
        private string _viewTitle = Resources.Label_ListTitle;

        public override void RejectChanges()
        {
            SelectedItems.Clear();
            base.RejectChanges();
        }

        #endregion

        #region Virtuals

        #endregion

        #region Private

        private List<KeyValuePair<string, object>> GetKeyAttribute(object instance)
        {
            var props = instance.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(KeyAttribute)));
            var sb = new List<KeyValuePair<string, object>>();

            foreach (var pi in props)
                sb.Add(new KeyValuePair<string, object>(pi.Name, pi.GetValue(instance, null)));

            return sb;
        }

        private string GetQueryByKeyAttribute(object instance)
        {
            var props = instance.GetType().GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(KeyAttribute)));
            var sb = new StringBuilder();
            int i = 0;
            foreach (var pi in props)
            {
                i++;
                sb.Append(pi.Name);
                sb.Append("=");
                sb.Append(GetValue(instance, pi));
                if (i < props.Count())
                    sb.Append(" and ");
            }
            return sb.ToString();
        }

        private object GetValue(object instance, PropertyInfo pi)
        {
            if (pi.PropertyType == typeof(string) || pi.PropertyType == typeof(Guid))
                return string.Format("\"{0}\"", pi.GetValue(instance, null));

            return pi.GetValue(instance, null);
        }

        private void SelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                    CurrentItem = (T)item;
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
            {
                CurrentItem = SelectedItems.LastOrDefault();
            }

            OnSelectedItemsChanged(e);
        }

        #endregion
    }
}
