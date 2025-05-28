using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.Controls;
using Gefco.CipQuai.Extensions;
using Gefco.CipQuai.Services;

namespace Gefco.CipQuai.Pages
{
    public class DebugListItem : ViewModelBaseExt
    {
        public DebugType DebugType { get; }
        public string Name { get; }
        public int Count { get; }
        private int _current;
        public int Current
        {
            get => _current;
            set
            {
                _current = value;
                RaisePropertyChanged();
            }
        }
        public object Item { get; }

        public DebugListItem(DebugType debugType, string name, object item = null)
        {
            DebugType = debugType;
            Name = name;
            Item = item;
        }
        public DebugListItem(DebugType debugType, string name, int count)
        {
            DebugType = debugType;
            Name = name;
            Count = count;
            ErrorCommand = new RelayCommand(Error);
            //SyncCommand = new RelayCommand(Sync, () => !IsSynching);
        }
        private void Error()
        {
            if (!ErrorMessage.IsNullOrEmpty())
            {
                DialogService.ShowMessageBox("Erreur", ErrorMessage);
            }
        }
        private bool _isSynching;
        public bool IsSynching
        {
            get => _isSynching;
            set
            {
                _isSynching = value;
                RaisePropertyChanged();
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage == value)
                    return;
                _errorMessage = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(HasError));
            }
        }

        public RelayCommand ErrorCommand { get; set; }
        //public RelayCommand SyncCommand { get; set; }
        //public async void Sync()
        //{
        //    if (IsSynching)
        //        return;
        //    IsSynching = true;
        //    Current = 0;
        //    ErrorMessage = null;
        //    switch (DebugType)
        //    {
        //        case DebugType.DP:
        //            await SyncDP1();
        //            await SyncDP2();
        //            break;
        //        case DebugType.NC:
        //            await SyncNC();
        //            break;
        //        case DebugType.BP:
        //            await SyncBP();
        //            break;
        //        case DebugType.SP:
        //            await SyncSP();
        //            break;
        //        case DebugType.CR:
        //            await SyncCR();
        //            break;
        //        case DebugType.Picture:
        //            await SyncPic();
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //    IsSynching = false;
        //}
        public async Task SyncAsync()
        {
            if (IsSynching)
                return;
            IsSynching = true;
            Current = 0;
            ErrorMessage = null;
            switch (DebugType)
            {
                case DebugType.DP:
                    await SyncDP1();
                    await SyncDP2();
                    break;
                case DebugType.NC:
                    await SyncNC();
                    break;
                case DebugType.BP:
                    await SyncBP();
                    break;
                case DebugType.SP:
                    await SyncSP();
                    break;
                case DebugType.CR:
                    await SyncCR();
                    break;
                case DebugType.Picture:
                    await SyncPic();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            IsSynching = false;
        }
        public bool HasError
        {
            get => !ErrorMessage.IsNullOrEmpty();
        }
        private async Task SyncPic()
        {
            App.Settings.UploadPicture = App.Settings.UploadPicture.DistinctBy(p => p.Picture.DeclarationId + p.Picture.PicturePath).ToList();
            var items = App.Settings.UploadPicture.DistinctBy(p => p.Picture.DeclarationId + p.Picture.PicturePath).ToList();
            if (items.Any())
                try
                {
                    PictureServiceResult result = null;
                    foreach (var item in items)
                    {
                        var filePath = item.Picture.PicturePath;
                        var bytes = File.ReadAllBytes(filePath);
                        var fileContent = Convert.ToBase64String(bytes);
                        switch (item.PictureType)
                        {
                            case Settings.UploadPictureType.SP:
                                result = await DataService.UploadPicSP(fileContent, Path.GetFileName(filePath), (PictureType) item.Picture.PictureType, item.Picture.DeclarationId);
                                break;
                            case Settings.UploadPictureType.CR:
                                result = await DataService.UploadPicCR(fileContent, Path.GetFileName(filePath), (PictureType) item.Picture.PictureType, item.Picture.DeclarationId);
                                break;
                            case Settings.UploadPictureType.DP:
                                result = await DataService.UploadPicDP(fileContent, Path.GetFileName(filePath), (PictureType) item.Picture.PictureType, item.Picture.DeclarationId);
                                break;
                            case Settings.UploadPictureType.NC:
                                result = await DataService.UploadPicNC(fileContent, Path.GetFileName(filePath), (PictureType) item.Picture.PictureType, item.Picture.DeclarationId);
                                break;
                            case Settings.UploadPictureType.BP:
                                result = await DataService.UploadPicBP(fileContent, Path.GetFileName(filePath), (PictureType) item.Picture.PictureType, item.Picture.DeclarationId);
                                break;
                            case Settings.UploadPictureType.Profile:
                                result = await DataService.UploadProfilePic(fileContent, Path.GetFileName(filePath));
                                break;
                        }
                        if (result?.IsSuccess ?? false)
                            App.Settings.UploadPicture.Remove(item);
                        else if (result?.ErrorMessage != null)
                            ErrorMessage += result?.ErrorMessage + Environment.NewLine + Environment.NewLine;
                        Current++;
                        await Task.Delay(TimeSpan.FromSeconds(0.25));
                    }
                }
                catch (Exception exception)
                {
                    ErrorMessage += exception.ToString() + Environment.NewLine + Environment.NewLine;
                }
                finally
                {
                    App.Settings.UploadPicture = App.Settings.UploadPicture;
                }
        }
        private async Task SyncDP1()
        {
            var items = App.Settings.AddDeclarationDoublePlanchers.ToList();
            if (items.Any())
                try
                {
                    foreach (var item in items)
                    {
                        var result = await DataService.AddDeclarationDoublePlancher(item, item.TractionId);
                        if (result?.IsSuccess ?? false)
                        {
                            if (result.ErrorMessage.Contains("DeclarationExistsException"))
                                App.Settings.AddDeclarationDoublePlanchers.Remove(item);
                        }
                        else if (result?.ErrorMessage != null)
                            ErrorMessage += result?.ErrorMessage + Environment.NewLine + Environment.NewLine;
                        Current++;
                        await Task.Delay(TimeSpan.FromSeconds(0.25));
                    }
                }
                catch (Exception exception)
                {
                    ErrorMessage += exception.ToString() + Environment.NewLine + Environment.NewLine;
                }
                finally
                {
                    App.Settings.AddDeclarationDoublePlanchers = App.Settings.AddDeclarationDoublePlanchers;
                }
        }
        private async Task SyncDP2()
        {
            var items = App.Settings.UpdateDeclarationDoublePlanchers.ToList();
            if (items.Any())
                try
                {
                    foreach (var item in items)
                    {
                        var result = await DataService.UpdateDeclarationDoublePlancherAsync(item, item.CurrentStatus.Id);
                        if (result?.IsSuccess ?? false)
                        {
                            if (result.ErrorMessage.Contains("DeclarationInProgressException") || result.ErrorMessage.Contains("InvalidDeclarationException"))
                                App.Settings.UpdateDeclarationDoublePlanchers.Remove(item);
                        }
                        else if (result?.ErrorMessage != null)
                            ErrorMessage += result?.ErrorMessage + Environment.NewLine + Environment.NewLine;
                        Current++;
                        await Task.Delay(TimeSpan.FromSeconds(0.25));
                    }
                }
                catch (Exception exception)
                {
                    ErrorMessage += exception.ToString() + Environment.NewLine + Environment.NewLine;
                }
                finally
                {
                    App.Settings.UpdateDeclarationDoublePlanchers = App.Settings.UpdateDeclarationDoublePlanchers;
                }
        }
        private async Task SyncNC()
        {
            var items = App.Settings.AddDeclarationNonConformites.ToList();
            if (items.Any())
                try
                {
                    foreach (var item in items)
                    {
                        var result = await DataService.AddDeclarationNonConformite(item);
                        if (result?.IsSuccess ?? false)
                        {
                            App.Settings.AddDeclarationNonConformites.Remove(item);
                        }
                        else if (result?.ErrorMessage != null)
                            ErrorMessage += result?.ErrorMessage + Environment.NewLine + Environment.NewLine;
                        Current++;
                        await Task.Delay(TimeSpan.FromSeconds(0.25));
                    }
                }
                catch (Exception exception)
                {
                    ErrorMessage += exception.ToString() + Environment.NewLine + Environment.NewLine;
                }
                finally
                {
                    App.Settings.AddDeclarationNonConformites = App.Settings.AddDeclarationNonConformites;
                }
        }
        private async Task SyncBP()
        {
            var items = App.Settings.AddDeclarationBonnePratiques.ToList();
            if (items.Any())
                try
                {
                    foreach (var item in items)
                    {
                        var result = await DataService.AddDeclarationBonnePratique(item);
                        if (result?.IsSuccess ?? false)
                        {
                            App.Settings.AddDeclarationBonnePratiques.Remove(item);
                        }
                        else if (result?.ErrorMessage != null)
                            ErrorMessage += result?.ErrorMessage + Environment.NewLine + Environment.NewLine;
                        Current++;
                        await Task.Delay(TimeSpan.FromSeconds(0.25));
                    }
                }
                catch (Exception exception)
                {
                    ErrorMessage += exception.ToString() + Environment.NewLine + Environment.NewLine;
                }
                finally
                {
                    App.Settings.AddDeclarationBonnePratiques = App.Settings.AddDeclarationBonnePratiques; 
                }
        }
        private async Task SyncSP()
        {
            var items = App.Settings.AddDeclarationRemorques.ToList();
            if (items.Any())
                try
                {
                    foreach (var item in items)
                    {
                        var result = await DataService.AddDeclarationRemorque(item);
                        if (result?.IsSuccess ?? false)
                        {
                            if (result.ErrorMessage.Contains("DeclarationExistsException"))
                                App.Settings.AddDeclarationRemorques.Remove(item);
                        }
                        else if (result?.ErrorMessage != null)
                            ErrorMessage += result?.ErrorMessage + Environment.NewLine + Environment.NewLine;
                        Current++;
                        await Task.Delay(TimeSpan.FromSeconds(0.25));
                    }
                }
                catch (Exception exception)
                {
                    ErrorMessage += exception.ToString() + Environment.NewLine + Environment.NewLine;
                }
                finally
                {
                    App.Settings.AddDeclarationRemorques = App.Settings.AddDeclarationRemorques;
                }
        }
        private async Task SyncCR()
        {
            var items = App.Settings.AddDeclarationControleReceptions.ToList();
            if (items.Any())
                try
                {
                    foreach (var item in items)
                    {
                        var result = await DataService.AddDeclarationControleReception(item);
                        if (result?.IsSuccess ?? false)
                        {
                            if (result.ErrorMessage.Contains("DeclarationExistsException"))
                                App.Settings.AddDeclarationControleReceptions.Remove(item);
                        }
                        else if (result?.ErrorMessage != null)
                            ErrorMessage += result?.ErrorMessage + Environment.NewLine + Environment.NewLine;
                        Current++;
                        await Task.Delay(TimeSpan.FromSeconds(0.25));
                    }
                }
                catch (Exception exception)
                {
                    ErrorMessage += exception.ToString() + Environment.NewLine + Environment.NewLine;
                }
                finally
                {
                    App.Settings.AddDeclarationControleReceptions = App.Settings.AddDeclarationControleReceptions;
                }
        }

        public static DebugListItem Create(DeclarationDoublePlancher item) => new DebugListItem(DebugType.DP, item.Traction.Name, item);
        public static DebugListItem Create(DeclarationControleReception item) => new DebugListItem(DebugType.CR, item.Traction.Name, item);
        public static DebugListItem Create(DeclarationSimplePlancher item) => new DebugListItem(DebugType.SP, item.Traction.Name, item);
        public static DebugListItem Create(DeclarationNonConformite item)  => new DebugListItem(DebugType.NC, item.AgenceConcernée.Name, item);
        public static DebugListItem Create(DeclarationBonnePratique item)  => new DebugListItem(DebugType.BP, item.AutreAgentConcerné, item);
        public static DebugListItem Create(Settings.UploadPictureParameter item) => new DebugListItem(DebugType.Picture, item.Picture.PicturePath, item);
    }
}
