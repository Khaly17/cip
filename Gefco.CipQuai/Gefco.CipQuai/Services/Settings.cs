using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Gefco.CipQuai.Annotations;
using Gefco.CipQuai.ApiClient.Models;
using Gefco.CipQuai.Extensions;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Forms.Internals;

namespace Gefco.CipQuai
{
    public class Settings : INotifyPropertyChanged
    {
        private static List<string> _pendingPayloads;

        [Preserve]
        public Settings()
        {
        }

        public string HockeyAppApiKey { get; } = "559de8a6de3948f68256bcf62b0dbf84";

        public ApplicationUser User { get; set; }

        private static ISettings AppSettings => CrossSettings.Current;

        public List<string> PendingPayloads
        {
            get
            {
                if (_pendingPayloads == null)
                {
                    var json = AppSettings.GetValueOrDefault(nameof(PendingPayloads), string.Empty);
                    if (string.IsNullOrEmpty(json))
                        _pendingPayloads = new List<string>();
                    else
                        _pendingPayloads = HelperJson.DeserializeCollection<string>(json);
                }

                return _pendingPayloads;
            }
            set
            {
                if (value != null)
                {
                    _pendingPayloads = value;
                    AppSettings.AddOrUpdateValue(nameof(PendingPayloads), HelperJson.Serialize(value));
                }
            }
        }

        public int RequestCode { get; set; }

        public string DebugServiceUri => ApiClient.Settings.DebugServiceUri;
        public string ServiceUri => ApiClient.Settings.ServiceUri;

        private static UpdateUserParameters _userToSave;
        public UpdateUserParameters UserToSave
        {
            get
            {
                if (_userToSave == null)
                {
                    var json = AppSettings.GetValueOrDefault(nameof(UserToSave), string.Empty);
                    if (string.IsNullOrEmpty(json))
                        _userToSave = null;
                    else
                        _userToSave = HelperJson.Deserialize<UpdateUserParameters>(json);
                }

                return _userToSave;
            }
            set
            {
                if (value != null)
                {
                    _userToSave = value;
                    AppSettings.AddOrUpdateValue(nameof(UserToSave), HelperJson.Serialize(value));
                }
            }
        }

        private static string _profilePicture;
        public string ProfilePicture
        {
            get
            {
                if (_profilePicture == null)
                {
                    var json = AppSettings.GetValueOrDefault(nameof(ProfilePicture), string.Empty);
                    if (string.IsNullOrEmpty(json))
                        _profilePicture = "User.svg";
                    else
                        _profilePicture = HelperJson.Deserialize<string>(json);
                }

                return _profilePicture;
            }
            set
            {
                if (value != null)
                {
                    _profilePicture = value;
                    AppSettings.AddOrUpdateValue(nameof(ProfilePicture), HelperJson.Serialize(value));
                    OnPropertyChanged();
                }
            }
        }

        private IList<Configuration> _configurations;
        public IList<Configuration> Configurations
        {
            get
            {
                if (_configurations == null)
                {
                    var json = AppSettings.GetValueOrDefault(nameof(Configurations), string.Empty);
                    if (string.IsNullOrEmpty(json))
                        _configurations = new List<Configuration>();
                    else
                        _configurations = HelperJson.DeserializeCollection<Configuration>(json);
                }

                return _configurations;
            }
            set
            {
                if (value != null)
                {
                    _configurations = value;
                    AppSettings.AddOrUpdateValue(nameof(Configurations), HelperJson.Serialize(value));
                }
            }
        }

        private IList<MotifDP> _motifDPs;
        public IList<MotifDP> MotifDPs
        {
            get
            {
                if (_motifDPs == null)
                {
                    var json = AppSettings.GetValueOrDefault(nameof(MotifDPs), string.Empty);
                    if (string.IsNullOrEmpty(json))
                        _motifDPs = new List<MotifDP>();
                    else
                        _motifDPs = HelperJson.DeserializeCollection<MotifDP>(json);
                }

                return _motifDPs;
            }
            set
            {
                if (value != null)
                {
                    _motifDPs = value;
                    AppSettings.AddOrUpdateValue(nameof(MotifDPs), HelperJson.Serialize(value));
                }
            }
        }

        private IList<MotifNC> _MotifNCs;
        public IList<MotifNC> MotifNCs
        {
            get
            {
                if (_MotifNCs == null)
                {
                    var json = AppSettings.GetValueOrDefault(nameof(MotifNCs), string.Empty);
                    if (string.IsNullOrEmpty(json))
                        _MotifNCs = new List<MotifNC>();
                    else
                        _MotifNCs = HelperJson.DeserializeCollection<MotifNC>(json);
                }

                return _MotifNCs;
            }
            set
            {
                if (value != null)
                {
                    _MotifNCs = value;
                    AppSettings.AddOrUpdateValue(nameof(MotifNCs), HelperJson.Serialize(value));
                }
            }
        }

        private IList<Resource> _resources;
        public IList<Resource> Resources
        {
            get
            {
                if (_resources == null)
                {
                    var json = AppSettings.GetValueOrDefault(nameof(Resources), string.Empty);
                    if (string.IsNullOrEmpty(json))
                        _resources = new List<Resource>();
                    else
                        _resources = HelperJson.DeserializeCollection<Resource>(json);
                }

                return _resources;
            }
            set
            {
                if (value != null)
                {
                    _resources = value;
                    AppSettings.AddOrUpdateValue(nameof(Resources), HelperJson.Serialize(value));
                }
            }
        }

        private IList<RemorqueStatus> _remorqueStatuses;
        public IList<RemorqueStatus> RemorqueStatuses
        {
            get
            {
                if (_remorqueStatuses == null)
                {
                    var json = AppSettings.GetValueOrDefault(nameof(RemorqueStatuses), string.Empty);
                    if (string.IsNullOrEmpty(json))
                        _remorqueStatuses = new List<RemorqueStatus>();
                    else
                        _remorqueStatuses = HelperJson.DeserializeCollection<RemorqueStatus>(json);
                }

                return _remorqueStatuses;
            }
            set
            {
                if (value != null)
                {
                    _remorqueStatuses = value;
                    AppSettings.AddOrUpdateValue(nameof(RemorqueStatuses), HelperJson.Serialize(value));
                }
            }
        }


        private IList<DeclarationDoublePlancher> _declarationDoublePlanchers;
        public IList<DeclarationDoublePlancher> DeclarationDoublePlanchers
        {
            get
            {
                if (_declarationDoublePlanchers == null)
                {
                    var json = AppSettings.GetValueOrDefault(nameof(DeclarationDoublePlanchers), string.Empty);
                    if (string.IsNullOrEmpty(json))
                        _declarationDoublePlanchers = new List<DeclarationDoublePlancher>();
                    else
                        _declarationDoublePlanchers = HelperJson.DeserializeCollection<DeclarationDoublePlancher>(json);
                }

                return _declarationDoublePlanchers;
            }
            set
            {
                if (value != null)
                {
                    _declarationDoublePlanchers = value;
                    AppSettings.AddOrUpdateValue(nameof(DeclarationDoublePlanchers), HelperJson.Serialize(value));
                }
            }
        }

        private IList<Traction> _tractions;
        public IList<Traction> Tractions
        {
            get
            {
                if (_tractions == null)
                {
                    var json = AppSettings.GetValueOrDefault(nameof(Tractions), string.Empty);
                    if (string.IsNullOrEmpty(json))
                        _tractions = new List<Traction>();
                    else
                        _tractions = HelperJson.DeserializeCollection<Traction>(json);
                }

                return _tractions;
            }
            set
            {
                if (value != null)
                {
                    _tractions = value;
                    AppSettings.AddOrUpdateValue(nameof(Tractions), HelperJson.Serialize(value));
                }
            }
        }

        private IList<Agence> _Agences;
        public IList<Agence> Agences
        {
            get
            {
                if (_Agences == null)
                {
                    var json = AppSettings.GetValueOrDefault(nameof(Agences), string.Empty);
                    if (string.IsNullOrEmpty(json))
                        _Agences = new List<Agence>();
                    else
                        _Agences = HelperJson.DeserializeCollection<Agence>(json);
                }

                return _Agences;
            }
            set
            {
                if (value != null)
                {
                    _Agences = value;
                    AppSettings.AddOrUpdateValue(nameof(Agences), HelperJson.Serialize(value));
                }
            }
        }

        private IList<Agence> _AgencesCR;
        public IList<Agence> AgencesCR
        {
            get
            {
                if (_Agences == null)
                {
                    var json = AppSettings.GetValueOrDefault(nameof(AgencesCR), string.Empty);
                    if (string.IsNullOrEmpty(json))
                        _Agences = new List<Agence>();
                    else
                        _Agences = HelperJson.DeserializeCollection<Agence>(json);
                }

                return _Agences;
            }
            set
            {
                if (value != null)
                {
                    _Agences = value;
                    AppSettings.AddOrUpdateValue(nameof(AgencesCR), HelperJson.Serialize(value));
                }
            }
        }

        private IList<ApplicationUser> _acteurs;
        public IList<ApplicationUser> Acteurs
        {
            get
            {
                if (_acteurs == null)
                {
                    var json = AppSettings.GetValueOrDefault(nameof(Acteurs), string.Empty);
                    if (string.IsNullOrEmpty(json))
                        _acteurs = new List<ApplicationUser>();
                    else
                        _acteurs = HelperJson.DeserializeCollection<ApplicationUser>(json);
                }

                return _acteurs;
            }
            set
            {
                if (value != null)
                {
                    _acteurs = value;
                    AppSettings.AddOrUpdateValue(nameof(Acteurs), HelperJson.Serialize(value));
                }
            }
        }

        private DateTime _lastResourceUpdate = new DateTime(2019, 1, 1);
        public DateTime LastResourceUpdate
        {
            get
            {
                if (_lastResourceUpdate == new DateTime(2019, 1, 1))
                {
                    var json = AppSettings.GetValueOrDefault(nameof(LastResourceUpdate), string.Empty);
                    if (string.IsNullOrEmpty(json))
                        _lastResourceUpdate = new DateTime(2019, 1, 1);
                    else
                        _lastResourceUpdate = HelperJson.Deserialize<DateTime>(json);
                }

                return _lastResourceUpdate;
            }
            set
            {
                if (value > new DateTime(2019, 1, 1))
                {
                    _lastResourceUpdate = value;
                    AppSettings.AddOrUpdateValue(nameof(LastResourceUpdate), HelperJson.Serialize(value));
                }
            }
        }

        public event EventHandler Updated;

        public void InvokeUpdated()
        {
            Updated?.Invoke(this, EventArgs.Empty);
        }

        public void Clean()
        {
        }

        public class UpdateUserParameters
        {
            /// <summary>
            /// Initializes a new instance of the UpdateUserParameters class.
            /// </summary>
            public UpdateUserParameters()
            {
            }

            public UpdateUserParameters(string userId = default(string), string email = default(string), string firstName = default(string), string lastName = default(string), string phoneNumber = default(string), string language = default(string), string password = default(string), string foreignKey = default(string), System.Guid accountId = default(System.Guid))
            {
                UserId = userId;
                Email = email;
                FirstName = firstName;
                LastName = lastName;
                PhoneNumber = phoneNumber;
                Password = password;
            }

            public string UserId { get; set; }

            public string Email { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string PhoneNumber { get; set; }

            public string Password { get; set; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static List<UploadPictureParameter> _uploadPicture;
        public List<UploadPictureParameter> UploadPicture
        {
            get
            {
                if (_uploadPicture == null)
                {
                    var json = AppSettings.GetValueOrDefault(nameof(UploadPicture), string.Empty);
                    if (string.IsNullOrEmpty(json))
                        _uploadPicture = new List<UploadPictureParameter>();
                    else
                        try
                        {
                            _uploadPicture = new List<UploadPictureParameter>(HelperJson.DeserializeCollection<UploadPictureParameter>(json));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                }

                return _uploadPicture;
            }
            set
            {
                if (value != null)
                {
                    _uploadPicture = value.DistinctBy(p => p.Picture.DeclarationId + p.Picture.PicturePath).ToList();
                    AppSettings.AddOrUpdateValue(nameof(UploadPicture), HelperJson.Serialize(value));
                    App.InvokeSyncHappened();
                }
            }
        }

        private static List<DeclarationDoublePlancher> _addDeclarationDoublePlanchers;
        public List<DeclarationDoublePlancher> AddDeclarationDoublePlanchers
        {
            get
            {
                if (_addDeclarationDoublePlanchers == null)
                {
                    var json = AppSettings.GetValueOrDefault(nameof(AddDeclarationDoublePlanchers), string.Empty);
                    if (string.IsNullOrEmpty(json))
                        _addDeclarationDoublePlanchers = new List<DeclarationDoublePlancher>();
                    else
                        try
                        {
                            _addDeclarationDoublePlanchers = new List<DeclarationDoublePlancher>(HelperJson.DeserializeCollection<DeclarationDoublePlancher>(json));
                        }
                        catch (Exception e)
                        {
                            _addDeclarationDoublePlanchers = new List<DeclarationDoublePlancher>();
                        }
                }

                return _addDeclarationDoublePlanchers;
            }
            set
            {
                if (value != null)
                {
                    _addDeclarationDoublePlanchers = value.DistinctBy(p => p.TractionId).ToList();
                    AppSettings.AddOrUpdateValue(nameof(AddDeclarationDoublePlanchers), HelperJson.Serialize(value));
                    App.InvokeSyncHappened();
                }
            }
        }

        private static List<DeclarationDoublePlancher> _updateDeclarationDoublePlanchers;
        public List<DeclarationDoublePlancher> UpdateDeclarationDoublePlanchers
        {
            get
            {
                if (_updateDeclarationDoublePlanchers == null)
                {
                    var json = AppSettings.GetValueOrDefault(nameof(UpdateDeclarationDoublePlanchers), string.Empty);
                    if (string.IsNullOrEmpty(json))
                        _updateDeclarationDoublePlanchers = new List<DeclarationDoublePlancher>();
                    else
                        try
                        {
                            _updateDeclarationDoublePlanchers = new List<DeclarationDoublePlancher>(HelperJson.DeserializeCollection<DeclarationDoublePlancher>(json));
                        }
                        catch (Exception e)
                        {
                            _updateDeclarationDoublePlanchers = new List<DeclarationDoublePlancher>();
                        }
                }

                return _updateDeclarationDoublePlanchers;
            }
            set
            {
                if (value != null)
                {
                    _updateDeclarationDoublePlanchers = value.DistinctBy(p => p.TractionId).ToList();
                    AppSettings.AddOrUpdateValue(nameof(UpdateDeclarationDoublePlanchers), HelperJson.Serialize(value));
                    App.InvokeSyncHappened();
                }
            }
        }

        private static List<DeclarationBonnePratique> _addDeclarationBonnePratiques;
        public List<DeclarationBonnePratique> AddDeclarationBonnePratiques
        {
            get
            {
                if (_addDeclarationBonnePratiques == null)
                {
                    var json = AppSettings.GetValueOrDefault(nameof(AddDeclarationBonnePratiques), string.Empty);
                    if (string.IsNullOrEmpty(json))
                        _addDeclarationBonnePratiques = new List<DeclarationBonnePratique>();
                    else
                        try
                        {
                            _addDeclarationBonnePratiques = new List<DeclarationBonnePratique>(HelperJson.DeserializeCollection<DeclarationBonnePratique>(json));
                        }
                        catch (Exception e)
                        {
                            _addDeclarationBonnePratiques = new List<DeclarationBonnePratique>();
                        }
                }

                return _addDeclarationBonnePratiques;
            }
            set
            {
                if (value != null)
                {
                    _addDeclarationBonnePratiques = value.DistinctBy(p => p.AutreAgentConcerné).ToList();
                    AppSettings.AddOrUpdateValue(nameof(AddDeclarationBonnePratiques), HelperJson.Serialize(value));
            App.InvokeSyncHappened();
                }
            }
        }

        private static List<DeclarationSimplePlancher> _addDeclarationRemorques;
        public List<DeclarationSimplePlancher> AddDeclarationRemorques
        {
            get
            {
                if (_addDeclarationRemorques == null)
                {
                    var json = AppSettings.GetValueOrDefault(nameof(AddDeclarationRemorques), string.Empty);
                    if (string.IsNullOrEmpty(json))
                        _addDeclarationRemorques = new List<DeclarationSimplePlancher>();
                    else
                        try
                        {
                            _addDeclarationRemorques = new List<DeclarationSimplePlancher>(HelperJson.DeserializeCollection<DeclarationSimplePlancher>(json));
                        }
                        catch (Exception e)
                        {
                            _addDeclarationRemorques = new List<DeclarationSimplePlancher>();
                        }
                }

                return _addDeclarationRemorques;
            }
            set
            {
                if (value != null)
                {
                    _addDeclarationRemorques = value.DistinctBy(p => p.Id).ToList();
                    AppSettings.AddOrUpdateValue(nameof(AddDeclarationRemorques), HelperJson.Serialize(value));
            App.InvokeSyncHappened();
                }
            }
        }

        private static List<DeclarationControleReception> _addDeclarationControleReceptions;
        public List<DeclarationControleReception> AddDeclarationControleReceptions
        {
            get
            {
                if (_addDeclarationControleReceptions == null)
                {
                    var json = AppSettings.GetValueOrDefault(nameof(AddDeclarationControleReceptions), string.Empty);
                    if (string.IsNullOrEmpty(json))
                        _addDeclarationControleReceptions = new List<DeclarationControleReception>();
                    else
                        try
                        {
                            _addDeclarationControleReceptions = new List<DeclarationControleReception>(HelperJson.DeserializeCollection<DeclarationControleReception>(json));
                        }
                        catch (Exception e)
                        {
                            _addDeclarationControleReceptions = new List<DeclarationControleReception>();
                        }
                }

                return _addDeclarationControleReceptions;
            }
            set
            {
                if (value != null)
                {
                    _addDeclarationControleReceptions = value.DistinctBy(p => p.Id).ToList();
                    AppSettings.AddOrUpdateValue(nameof(AddDeclarationControleReceptions), HelperJson.Serialize(value));
            App.InvokeSyncHappened();
                }
            }
        }


        private static List<DeclarationNonConformite> _addDeclarationNonConformites;
        public List<DeclarationNonConformite> AddDeclarationNonConformites
        {
            get
            {
                if (_addDeclarationNonConformites == null)
                {
                    var json = AppSettings.GetValueOrDefault(nameof(AddDeclarationNonConformites), string.Empty);
                    if (string.IsNullOrEmpty(json))
                        _addDeclarationNonConformites = new List<DeclarationNonConformite>();
                    else
                        try
                        {
                            _addDeclarationNonConformites = new List<DeclarationNonConformite>(HelperJson.DeserializeCollection<DeclarationNonConformite>(json));
                        }
                        catch (Exception e)
                        {
                            _addDeclarationNonConformites = new List<DeclarationNonConformite>();
                        }
                }

                return _addDeclarationNonConformites;
            }
            set
            {
                if (value != null)
                {
                    _addDeclarationNonConformites = value.DistinctBy(p => p.AgenceConcernée.Id).ToList();
                    AppSettings.AddOrUpdateValue(nameof(AddDeclarationNonConformites), HelperJson.Serialize(value));
            App.InvokeSyncHappened();
                }
            }
        }


        private string _userPin;

        public string UserPin
        {
            get
            {
                if (_userPin.IsNullOrEmpty())
                {
                    _userPin = AppSettings.GetValueOrDefault(nameof(UserPin), string.Empty);
                }

                return _userPin;
            }
            set
            {
                _userPin = value;
                AppSettings.AddOrUpdateValue(nameof(UserPin), value);
            }
        }

        public enum UploadPictureType
        {
            DP,
            NC,
            BP,
            SP,
            Profile,
            CR
        }

        public class UploadPictureParameter
        {
            public UploadPictureType PictureType { get; set; }
            public Picture Picture { get; set; }
        }
    }
}