using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gefco.CipQuai.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DebugPage : ContentPage
    {
        public ObservableCollection<DebugListItem> Items { get; set; }
        public DebugPage()
        {
            InitializeComponent();

            Items = new ObservableCollection<DebugListItem>
            {
                new DebugListItem(DebugType.Menu, $"Add SP ({App.Settings.AddDeclarationRemorques.Count})"),
                new DebugListItem(DebugType.Menu, $"Add DP ({App.Settings.AddDeclarationDoublePlanchers.Count})"),
                new DebugListItem(DebugType.Menu, $"Add NC ({App.Settings.AddDeclarationNonConformites.Count})"),
                new DebugListItem(DebugType.Menu, $"Add BP ({App.Settings.AddDeclarationBonnePratiques.Count})"),
                new DebugListItem(DebugType.Menu, $"Add CR ({App.Settings.AddDeclarationBonnePratiques.Count})"),
                new DebugListItem(DebugType.Menu, $"Update DP ({App.Settings.UpdateDeclarationDoublePlanchers.Count})"),
                new DebugListItem(DebugType.Menu, $"Add Picture DP ({App.Settings.UploadPicture.Count(p => p.PictureType == Settings.UploadPictureType.DP)})"),
                new DebugListItem(DebugType.Menu, $"Add Picture NC ({App.Settings.UploadPicture.Count(p => p.PictureType == Settings.UploadPictureType.NC)})"),
                new DebugListItem(DebugType.Menu, $"Add Picture BP ({App.Settings.UploadPicture.Count(p => p.PictureType == Settings.UploadPictureType.BP)})"),
                new DebugListItem(DebugType.Menu, $"Add Picture CR ({App.Settings.UploadPicture.Count(p => p.PictureType == Settings.UploadPictureType.CR)})"),
            };

            MyListView.ItemsSource = Items;
        }

        private DebugPage(string title, List<DeclarationSimplePlancher> list)
        {
            InitializeComponent();
            Title = title;
            Items = new ObservableCollection<DebugListItem>(list.Select(DebugListItem.Create));
            MyListView.ItemsSource = Items;
        }
        private DebugPage(string title, List<DeclarationDoublePlancher> list)
        {
            InitializeComponent();
            Title = title;
            Items = new ObservableCollection<DebugListItem>(list.Select(DebugListItem.Create));
            MyListView.ItemsSource = Items;
        }
        private DebugPage(string title, List<DeclarationControleReception> list)
        {
            InitializeComponent();
            Title = title;
            Items = new ObservableCollection<DebugListItem>(list.Select(DebugListItem.Create));
            MyListView.ItemsSource = Items;
        }
        private DebugPage(string title, List<DeclarationNonConformite> list)
        {
            InitializeComponent();
            Title = title;
            Items = new ObservableCollection<DebugListItem>(list.Select(DebugListItem.Create));
            MyListView.ItemsSource = Items;
        }
        private DebugPage(string title, List<DeclarationBonnePratique> list)
        {
            InitializeComponent();
            Title = title;
            Items = new ObservableCollection<DebugListItem>(list.Select(DebugListItem.Create));
            MyListView.ItemsSource = Items;
        }
        private DebugPage(string title, List<Settings.UploadPictureParameter> list)
        {
            InitializeComponent();
            Title = title;
            Items = new ObservableCollection<DebugListItem>(list.Select(DebugListItem.Create));
            MyListView.ItemsSource = Items;
        }


        private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var listItem = e.Item as DebugListItem;
            if (listItem == null)
            {
                return;
            }
            switch (listItem.DebugType)
            {
                case DebugType.Menu:
                    var title = listItem.Name;
                    if (title.Contains("Add DP")) await ServiceNavigation.Instance.PushAsync(new DebugPage(title, App.Settings.AddDeclarationDoublePlanchers.ToList()));
                    else if (title.Contains("Add CR")) await ServiceNavigation.Instance.PushAsync(new DebugPage(title, App.Settings.AddDeclarationControleReceptions.ToList()));
                    else if (title.Contains("Add SC")) await ServiceNavigation.Instance.PushAsync(new DebugPage(title, App.Settings.AddDeclarationRemorques.ToList()));
                    else if (title.Contains("Add NC")) await ServiceNavigation.Instance.PushAsync(new DebugPage(title, App.Settings.AddDeclarationNonConformites.ToList()));
                    else if (title.Contains("Add BP")) await ServiceNavigation.Instance.PushAsync(new DebugPage(title, App.Settings.AddDeclarationBonnePratiques.ToList()));
                    else if (title.Contains("Update DP")) await ServiceNavigation.Instance.PushAsync(new DebugPage(title, App.Settings.UpdateDeclarationDoublePlanchers.ToList()));
                    else if (title.Contains("Add Picture CR")) await ServiceNavigation.Instance.PushAsync(new DebugPage(title, App.Settings.UploadPicture.Where(p => p.PictureType == Settings.UploadPictureType.CR).ToList()));
                    else if (title.Contains("Add Picture SP")) await ServiceNavigation.Instance.PushAsync(new DebugPage(title, App.Settings.UploadPicture.Where(p => p.PictureType == Settings.UploadPictureType.SP).ToList()));
                    else if (title.Contains("Add Picture DP")) await ServiceNavigation.Instance.PushAsync(new DebugPage(title, App.Settings.UploadPicture.Where(p => p.PictureType == Settings.UploadPictureType.DP).ToList()));
                    else if (title.Contains("Add Picture NC")) await ServiceNavigation.Instance.PushAsync(new DebugPage(title, App.Settings.UploadPicture.Where(p => p.PictureType == Settings.UploadPictureType.NC).ToList()));
                    else if (title.Contains("Add Picture BP")) await ServiceNavigation.Instance.PushAsync(new DebugPage(title, App.Settings.UploadPicture.Where(p => p.PictureType == Settings.UploadPictureType.BP).ToList()));
                    break;
                case DebugType.SP:
                    if (Title.Contains("Add SP"))
                        await App.SyncAddRemorque(true);
                    break;
                case DebugType.DP:
                    if (Title.Contains("Add DP"))
                        await App.SyncAddDoublePlancher(true);
                    else
                        await App.SyncUpdateDoublePlancher(true);
                    break;
                case DebugType.NC:
                    if (Title.Contains("Add NC"))
                        await App.SyncAddNonConformite(true);
                    break;
                case DebugType.BP:
                    if (Title.Contains("Add BP"))
                        await App.SyncAddBonnePratique(true);
                    break;
                case DebugType.CR:
                    if (Title.Contains("Add CR"))
                        await App.SyncAddControleReception(true);
                    break;
                case DebugType.Picture:
                    await App.SyncAddPictures(true);
                    break;
            }

            //Deselect Item
            ((ListView) sender).SelectedItem = null;
        }
    }

    public enum DebugType
    {
        Menu,
        DP,
        NC,
        BP,
        SP,
        Picture,
        CR,
    }
}