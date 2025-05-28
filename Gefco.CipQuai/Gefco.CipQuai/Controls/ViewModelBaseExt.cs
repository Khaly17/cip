using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Gefco.CipQuai.Services;
using Xamarin.Forms;

namespace Gefco.CipQuai.Controls
{
    /// <summary>
    ///     View model base ext.
    /// </summary>
    public abstract class ViewModelBaseExt : ObservableObject
    {
        private bool _isLoading;

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="ViewModelBaseExt" /> class.
        /// </summary>
        protected ViewModelBaseExt()
        {
            InitializeViewModel();
            OpenMenuCommand = new RelayCommand(OpenMenu, () => !IsLoading);
            LoadingCommands.Add(OpenMenuCommand);
            GoBackCommand = new RelayCommand(GoBack, () => !IsLoading);
            LoadingCommands.Add(GoBackCommand);
            GoHomeCommand = new RelayCommand(GoHome, () => !IsLoading);
            LoadingCommands.Add(GoHomeCommand);
            PopCommand = new RelayCommand(Pop, () => !IsLoading);
            LoadingCommands.Add(PopCommand);
        }

        #endregion

        protected virtual void OnViewModelLoaded()
        {
            ViewModelLoaded?.Invoke();
        }

        #region Property

        public IDialogService DialogService => ServiceLocator.Current.GetInstance<IDialogService>();

        public IMyNavigation NavigationService => ServiceLocator.Current.GetInstance<IMyNavigation>();

        public List<RelayCommand> LoadingCommands = new List<RelayCommand>();
        public bool IsLoading
        {
            get => _isLoading;

            set
            {
                _isLoading = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanExecute));
                foreach (var command in LoadingCommands)
                {
                    command.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance can execute.
        /// </summary>
        /// <value><c>true</c> if this instance can execute; otherwise, <c>false</c>.</value>
        public virtual bool CanExecute => !_isLoading;

        #endregion

        #region Abstract method

        public bool Initialized { get; set; } = false;
        /// <summary>
        ///     Initializes the view model. Call it in the constructor.
        /// </summary>
        public virtual void InitializeViewModel()
        {
            Initialized = true;
        }

        public bool IsLoaded { get; set; } = false;
        public event Action ViewModelLoaded;

        /// <summary>
        ///     Called in OnAppearing by the page using this view model. It allows to refresh some data source etc.
        /// </summary>
        public virtual async Task LoadViewModel()
        {
            IsLoaded = true;
            return;
        }

        /// <summary>
        ///     Called in OnDisappearing by the page using this view model. It allows to clear some data source etc.
        /// </summary>
        public virtual void UnloadViewModel()
        {
        }

        /// <summary>
        ///     Locks the view model while performing the given action.
        ///     Changed the IsLoading property.
        /// </summary>
        /// <param name="task">The task.</param>
        public async void Await(Task task)
        {
            IsLoading = true;

            task.RunSynchronously();
            await Task.WhenAll(task);

            IsLoading = false;
        }

        #endregion

        public RelayCommand OpenMenuCommand { get; }
        private void OpenMenu()
        {
            MainPage.Instance.IsPresented = true;
        }

        public RelayCommand GoBackCommand { get; }
        private async void GoBack()
        {
            IsLoading = true;
            await NavigationService.PopAsync(true);
            IsLoading = false;
        }

        public RelayCommand GoHomeCommand { get; }
        private async void GoHome()
        {
            IsLoading = true;
            await NavigationService.PopToRootAsync(true);
            IsLoading = false;
        }

        public RelayCommand PopCommand { get; }
        private async void Pop()
        {
            IsLoading = true;
            await NavigationService.PopModalAsync(true);
            IsLoading = false;
        }

    }
}
