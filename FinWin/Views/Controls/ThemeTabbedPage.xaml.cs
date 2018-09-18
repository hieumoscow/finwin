using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace FinWin.Views.Controls
{
    public partial class ThemeTabbedPage : TabbedPage
    {
        public virtual void Init()
        {

        }
        public ThemeTabbedPage()
        {
            InitializeComponent();
            Current = this;
        }


        public static int BadgeNumber { get; set; } = -1;

        private Action<int> _setBadgeAction;
        public Action<int> SetBadgeAction
        {
            get { return _setBadgeAction; }
            set
            {
                _setBadgeAction = value;
                if (BadgeNumber != -1)
                {
                    _setBadgeAction.Invoke(BadgeNumber);
                    BadgeNumber = -1;
                }
            }
        }

        public Action HideBadgeAction;

        public static void SetBadge(int badgeNumber)
        {
            //if (Current != null && Current.CurrentPage != null)
            //    if (!Current.CurrentPage.GetType().Name.Equals(nameof(NotificationsPage)))
            if (badgeNumber == 0)
                HideBadge();
            else
            {
                if (Current?.SetBadgeAction == null)
                    BadgeNumber = badgeNumber;
                else
                    Current?.SetBadgeAction?.Invoke(badgeNumber);
            }
        }

        public static void HideBadge()
        {
            Current?.HideBadgeAction?.Invoke();
        }

        public static ThemeTabbedPage Current;

    }
}
