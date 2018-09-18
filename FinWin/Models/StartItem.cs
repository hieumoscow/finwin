using System;
namespace FinWin.Models
{
    public enum StartItemType { Contract, Notification, Support, More, Schedules, Offers, Profile }
    public class StartItem : BaseObject
    {
        private StartItemType _type;
        public StartItemType Type
        {
            get { return _type; }
            set
            {
                SetProperty(ref _type, value);
            }
        }

        private string _icon;
        public string Icon
        {
            get { return _icon; }
            set
            {
                SetProperty(ref _icon, value);
            }
        }

        public string Title => this[TitleResx];

        private string _titleResx;
        public string TitleResx
        {
            get { return _titleResx; }
            set
            {
                SetProperty(ref _titleResx, value);
                OnPropertyChanged(nameof(Title));
            }
        }


        private int _badgeNumber;
        public int BadgeNumber
        {
            get { return _badgeNumber; }
            set
            {
                SetProperty(ref _badgeNumber, value);
            }
        }


        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                SetProperty(ref _isEnabled, value);
            }
        }

    }
}
