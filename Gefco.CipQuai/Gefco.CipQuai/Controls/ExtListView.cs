using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Gefco.CipQuai.Controls
{

    /// <summary>
    ///     List view.
    /// </summary>
    public class ExtListView : ListView
    {
        /// <summary>
        ///     Raise the item appearing.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (IsLoading || ItemsSource == null)
            {
                return;
            }

            CommandLoadMore?.Execute(e.Item);
        }

        /// <summary>
        ///     Raises the item selected event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // We force the selected item so the view model can handle differents events.
            SelectedItem = e.SelectedItem;

            if (e.SelectedItem != null && Command != null && Command.CanExecute(e))
            {
                Command.Execute(e.SelectedItem);
            }

            if (DesactivateSelection)
            {
                SelectedItem = null;
            }
        }

        #region Bindable property

        /// <summary>
        ///     The is loading property.
        /// </summary>
        public static BindableProperty IsLoadingProperty = BindableProperty.Create(nameof(IsLoading), typeof(bool), typeof(ExtListView), false);

        /// <summary>
        ///     The deselection auto property.
        /// </summary>
        public static BindableProperty DesactivateSelectionProperty = BindableProperty.Create(nameof(DesactivateSelection), typeof(bool), typeof(ExtListView), false);

        /// <summary>
        ///     The command load more property.
        /// </summary>
        public static BindableProperty CommandLoadMoreProperty = BindableProperty.Create(nameof(CommandLoadMore), typeof(ICommand), typeof(ExtListView));

        /// <summary>
        ///     The command clicked property.
        /// </summary>
        public static BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ExtListView));

        #endregion

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExtListView" /> class.
        /// </summary>
        [Preserve]
        public ExtListView() : base(ListViewCachingStrategy.RecycleElement)
        {
            //this.ItemTapped += this.OnItemTapped;
            ItemSelected += OnItemSelected;
            ItemAppearing += OnItemAppearing;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:XTracker.Controls.ExtListView" /> class.
        /// </summary>
        /// <param name="strategy">Strategy.</param>
        [Preserve]
        public ExtListView(ListViewCachingStrategy strategy) : base(strategy)
        {
            ItemSelected += OnItemSelected;
            ItemAppearing += OnItemAppearing;
        }

        #endregion

        #region Property

        /// <summary>
        ///     Gets or sets the is loading.
        /// </summary>
        /// <value>The is loading.</value>
        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="ListView" /> deselection auto.
        /// </summary>
        /// <value><c>true</c> if deselection auto; otherwise, <c>false</c>.</value>
        public bool DesactivateSelection
        {
            get => (bool)GetValue(DesactivateSelectionProperty);
            set => SetValue(DesactivateSelectionProperty, value);
        }

        /// <summary>
        ///     Gets or sets the command clicked.
        /// </summary>
        /// <value>The command clicked.</value>
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        ///     Gets or sets the command load more.
        /// </summary>
        /// <value>The command load more.</value>
        public ICommand CommandLoadMore
        {
            get => (ICommand)GetValue(CommandLoadMoreProperty);
            set => SetValue(CommandLoadMoreProperty, value);
        }


        #endregion
    }
}
