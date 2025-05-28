using System;
using System.Collections.Generic;
using System.Text;
using Gefco.CipQuai.Controls;

namespace Gefco.CipQuai.Menu
{
    public class MenuPageItem : ObservableObject, IMenuPageItem
    {
        private bool _isSelected;

        public string Title { get; set; }

        public string IconSource { get; set; }
        public int IconWidth { get; set; }
        public int IconHeight { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value)
                    return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public Type TargetType { get; set; }
    }
}
