using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Gefco.CipQuai.Controls;

namespace Gefco.CipQuai.DoubleDeckPage
{
    public class ViewModel : ViewModelBaseExt
    {
        private string _agenceArrivee;
        private string _agenceDepart;
        private int _doubleDeckCount = 4;
        private string _endPlace;

        private bool _isInvalid;
        private string _selectedRemorque;
        private string _startPlace;

        public ViewModel()
        {
            ValidateCommand = new RelayCommand(async () => await ValidateAndGoAsync(), CanExecute);
        }

        public string PageTitle => "Chargement DP";
        public string PageInvite => $"{DoubleDeckCount} remorques DP sont à charger\r\nsur votre site";
        public string RemorqueInvite => $"Remorque";
        public string AgenceDepartLabel => $"Agence de départ";

        public string AgenceDepart
        {
            get => _agenceDepart;
            set
            {
                if (value == _agenceDepart)
                    return;
                _agenceDepart = value;
                OnPropertyChanged();
            }
        }

        public string AgenceArriveeLabel => $"Agence d'arrivée";

        public string AgenceArrivee
        {
            get => _agenceArrivee;
            set
            {
                if (value == _agenceArrivee)
                    return;
                _agenceArrivee = value;
                OnPropertyChanged();
            }
        }

        public string RemorqueNumberLabel => $"Remorque N°";
        public string ValidationSummary => $"Aucune remorque sélectionnée";

        public List<string> Remorques => new List<string>
        {
            "Bordeaux - Nantes",
            "Bordeaux - Toulouse"
        };

        public string SelectedRemorque
        {
            get => _selectedRemorque;
            set
            {
                if (value == _selectedRemorque)
                    return;
                _selectedRemorque = value;
                var strings = _selectedRemorque?.Split('-');
                if (strings?.Length == 2)
                {
                    AgenceDepart = strings[0].Trim();
                    AgenceArrivee = strings[1].Trim();
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(ValidateCommand));
            }
        }

        public int DoubleDeckCount
        {
            get => _doubleDeckCount;
            set
            {
                if (value == _doubleDeckCount)
                    return;
                _doubleDeckCount = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PageInvite));
            }
        }

        public RelayCommand ValidateCommand { get; set; }
        public override bool CanExecute => SelectedRemorque != null;

        public bool IsInvalid
        {
            get => _isInvalid;
            set
            {
                if (value == _isInvalid)
                    return;
                _isInvalid = value;
                OnPropertyChanged();
            }
        }

        private async Task ValidateAndGoAsync()
        {
            Validate();
            if (!IsInvalid)
                await NavigationService.PopAsync();
        }

        public void Validate()
        {
            IsInvalid = string.IsNullOrEmpty(SelectedRemorque);
        }

        public void Reset()
        {
            IsInvalid = false;
            DoubleDeckCount = 4;
            SelectedRemorque = null;
            AgenceDepart = null;
            AgenceArrivee = null;
        }
    }
}