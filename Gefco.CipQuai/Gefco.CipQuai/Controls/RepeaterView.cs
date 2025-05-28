using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Input;
using Xamarin.Forms;

namespace Gefco.CipQuai.Controls
{
    public class RepeaterView : StackLayout
    {
        /// <summary>
        ///     Event delegate definition fo the <see cref="ItemCreated" /> event
        /// </summary>
        /// <param name="sender">The sender(this).</param>
        /// <param name="args">The <see cref="RepeaterViewItemAddedEventArgs" /> instance containing the event data.</param>
        /// Element created at 15/11/2014,3:12 PM by Charles
        public delegate void RepeaterViewItemAddedEventHandler(
            object sender,
            RepeaterViewItemAddedEventArgs args);

        /// <summary>
        ///     Definition for <see cref="ItemTemplate" />
        /// </summary>
        /// Element created at 15/11/2014,3:11 PM by Charles
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(RepeaterView), default(DataTemplate));

        /// <summary>
        ///     Definition for <see cref="ItemsSource" />
        /// </summary>
        /// Element created at 15/11/2014,3:11 PM by Charles
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(RepeaterView), null, BindingMode.OneWay, null, ItemsChanged);

        /// <summary>
        ///     Definition for <see cref="ItemClickCommand" />
        /// </summary>
        /// Element created at 15/11/2014,3:11 PM by Charles
        public static BindableProperty ItemClickCommandProperty =
            BindableProperty.Create(nameof(ItemClickCommand), typeof(ICommand), typeof(RepeaterView), null);

        /// <summary>
        ///     The Collection changed handler
        /// </summary>
        /// Element created at 15/11/2014,3:13 PM by Charles
        private IDisposable _collectionChangedHandle;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RepeaterView" /> class.
        /// </summary>
        /// Element created at 15/11/2014,3:13 PM by Charles
        public RepeaterView()
        {
            Spacing = 0;
        }

        /// <summary>Gets or sets the items source.</summary>
        /// <value>The items source.</value>
        /// Element created at 15/11/2014,3:13 PM by Charles
        public IEnumerable ItemsSource
        {
            get => (IEnumerable) GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        /// <summary>Gets or sets the item click command.</summary>
        /// <value>The item click command.</value>
        /// Element created at 15/11/2014,3:13 PM by Charles
        public ICommand ItemClickCommand
        {
            get => (ICommand) GetValue(ItemClickCommandProperty);
            set => SetValue(ItemClickCommandProperty, value);
        }

        /// <summary>
        ///     The item template property
        ///     This can be used on it's own or in combination with
        ///     the <see cref="ItemTemplateSelector" />
        /// </summary>
        /// Element created at 15/11/2014,3:10 PM by Charles
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate) GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        /// <summary>Occurs when a view has been created.</summary>
        /// Element created at 15/11/2014,3:13 PM by Charles
        public event RepeaterViewItemAddedEventHandler ItemCreated;

        /// <summary>
        ///     Gives codebehind a chance to play with the
        ///     newly created view object :D
        /// </summary>
        /// <param name="view">The visual view object</param>
        /// <param name="model">The item being added</param>
        protected virtual void NotifyItemAdded(View view, object model)
        {
            ItemCreated?.Invoke(this, new RepeaterViewItemAddedEventArgs(view, model));
        }

        /// <summary>
        ///     Select a datatemplate dynamically
        ///     Prefer the TemplateSelector then the DataTemplate
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual DataTemplate GetTemplateFor(Type type)
        {
            return ItemTemplate;
        }

        /// <summary>
        ///     Creates a view based on the items type
        ///     While we do have T, T could very well be
        ///     a common superclass or an interface by
        ///     using the items actual type we support
        ///     both inheritance based polymorphism
        ///     and shape based polymorphism
        /// </summary>
        /// <param name="item"></param>
        /// <returns>A View that has been initialized with <see cref="item" /> as it's BindingContext</returns>
        /// <exception cref="InvalidVisualObjectException"></exception>
        /// Thrown when the matched datatemplate inflates to an object not derived from either
        /// <see cref="Xamarin.Forms.View" />
        /// or
        /// <see cref="Xamarin.Forms.ViewCell" />
        protected virtual View ViewFor(object item)
        {
            var template = GetTemplateFor(item.GetType());
            object content;
            if (!(template is Xamarin.Forms.DataTemplateSelector))
            {
                content = template.CreateContent();
            }
            else
            {
                content = ((Xamarin.Forms.DataTemplateSelector)template).SelectTemplate(item, this).CreateContent();
            }

            if (!(content is View) && !(content is ViewCell))
                throw new Exception(content.GetType() + "");
            var view = (content is View) ? content as View : ((ViewCell) content).View;
            view.BindingContext = item;
            view.GestureRecognizers.Add(
                new TapGestureRecognizer {Command = ItemClickCommand, CommandParameter = item});
            return view;
        }

        /// <summary>
        ///     Reset the collection of bound objects
        ///     Remove the old collection changed eventhandler (if any)
        ///     Create new cells for each new item
        /// </summary>
        /// <param name="bindable">The control</param>
        /// <param name="oldValue">Previous bound collection</param>
        /// <param name="newValue">New bound collection</param>
        private static void ItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as RepeaterView;
            if (control == null)
                throw new Exception(
                    "Invalid bindable object passed to ReapterView::ItemsChanged expected a ReapterView<T> received a "
                    + bindable.GetType().Name);

            control._collectionChangedHandle?.Dispose();

            control._collectionChangedHandle = new CollectionChangedHandle<View>(
                control.Children,
                (IEnumerable)newValue,
                control.ViewFor,
                (v, m, i) => control.NotifyItemAdded(v, m));
        }
    }

    /// <summary>
    ///     Argument for the <see cref="RepeaterView.ItemCreated" /> event
    /// </summary>
    /// Element created at 15/11/2014,3:13 PM by Charles
    public class RepeaterViewItemAddedEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RepeaterViewItemAddedEventArgs" /> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="model">The model.</param>
        /// Element created at 15/11/2014,3:14 PM by Charles
        public RepeaterViewItemAddedEventArgs(View view, object model)
        {
            View = view;
            Model = model;
        }

        /// <summary>Gets or sets the view.</summary>
        /// <value>The visual element.</value>
        /// Element created at 15/11/2014,3:14 PM by Charles
        public View View { get; set; }

        /// <summary>Gets or sets the model.</summary>
        /// <value>The original viewmodel.</value>
        /// Element created at 15/11/2014,3:14 PM by Charles
        public object Model { get; set; }
    }

    public class CollectionChangedHandle<TSyncType> : IDisposable where TSyncType : class
    {
        private readonly Action<TSyncType> _cleanup;
        private readonly INotifyCollectionChanged _itemsSourceCollectionChangedImplementation;
        private readonly Action<TSyncType, object, int> _postadd;
        private readonly Func<object, TSyncType> _projector;
        private readonly IEnumerable _sourceCollection;
        private readonly IList<TSyncType> _target;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CollectionChangedHandle{TSyncType,T}" /> class.
        /// </summary>
        /// <param name="target">The collection to be kept in sync with <see cref="source" />source</param>
        /// <param name="source">The original collection</param>
        /// <param name="projector">A function that returns {TSyncType} for a {T}</param>
        /// <param name="postadd">A functino called right after insertion into the synced collection</param>
        /// <param name="cleanup">
        ///     A function that performs any needed cleanup when {TSyncType} is removed from the
        ///     <see cref="target" />
        /// </param>
        public CollectionChangedHandle(IList<TSyncType> target, IEnumerable source, Func<object, TSyncType> projector, Action<TSyncType, object, int> postadd = null, Action<TSyncType> cleanup = null)
        {
            if (source == null)
                return;
            _itemsSourceCollectionChangedImplementation = source as INotifyCollectionChanged;
            _sourceCollection = source;
            _target = target;
            _projector = projector;
            _postadd = postadd;
            _cleanup = cleanup;
            InitialPopulation();
            if (_itemsSourceCollectionChangedImplementation == null)
                return;
            _itemsSourceCollectionChangedImplementation.CollectionChanged += CollectionChanged;
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_itemsSourceCollectionChangedImplementation == null)
                return;
            _itemsSourceCollectionChangedImplementation.CollectionChanged -= CollectionChanged;
        }

        /// <summary>Keeps <see cref="_target" /> in sync with <see cref="_sourceCollection" />.</summary>
        /// <param name="sender">The sender, completely ignored.</param>
        /// <param name="args">The <see cref="NotifyCollectionChangedEventArgs" /> instance containing the event data.</param>
        /// Element created at 15/11/2014,2:57 PM by Charles
        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Reset)
            {
                SafeClearTarget();
            }
            else
            {
                //Create a temp list to prevent multiple enumeration issues

                var tlist = new List<object>();
                foreach (var item in _sourceCollection)
                {
                    tlist.Add(item);
                }

                if (args.OldItems != null)
                {
                    var syncitem = _target[args.OldStartingIndex];
                    if (syncitem != null)
                        _cleanup?.Invoke(syncitem);
                    _target.RemoveAt(args.OldStartingIndex);
                }

                if (args.NewItems == null)
                    return;
                foreach (var obj in args.NewItems)
                {
                    var item = obj;
                    if (item == null)
                        continue;
                    var index = tlist.IndexOf(item);
                    var newsyncitem = _projector(item);
                    _target.Insert(index, newsyncitem);
                    _postadd?.Invoke(newsyncitem, item, index);
                }
            }
        }

        /// <summary>Initials the population.</summary>
        /// Element created at 15/11/2014,2:53 PM by Charles
        private void InitialPopulation()
        {
            SafeClearTarget();
            foreach (var t in _sourceCollection)
            {
                if (t == null)
                    continue;
                _target.Add(_projector(t));
            }
        }

        private void SafeClearTarget()
        {
            while (_target.Count > 0)
            {
                var syncitem = _target[0];
                _target.RemoveAt(0);
                _cleanup?.Invoke(syncitem);
            }
        }
    }
}