using System;
using FinWin.Models;
using MvvmHelpers;
using Xamarin.Forms;

namespace FinWin.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            StartItems = new ObservableRangeCollection<StartItem>()
            {
                new StartItem() {Icon = "contractSp", TitleResx = "Transactions_Title", Type = StartItemType.Contract},
                new StartItem()
                {
                    Icon = "paymentScheduleSp",
                    TitleResx = "Payments_Title",
                    Type = StartItemType.Schedules
                },
                new StartItem()
                {
                    Icon = "offerSp",
                    TitleResx = "Stock_Title",
                    Type = StartItemType.Offers
                },
                new StartItem()
                {
                    Icon = "notificationSp",
                    TitleResx = "Property_Title",
                    Type = StartItemType.Notification
                },
                new StartItem() {Icon = "supportSp", TitleResx = "Predictions_Title", Type = StartItemType.Support},
                new StartItem() {Icon = "moreSp", TitleResx = "Porfolio_Title", Type = StartItemType.More},
            };
        }

        ObservableRangeCollection<StartItem> _startItems;
        public ObservableRangeCollection<StartItem> StartItems
        {
            get { return _startItems; }
            set
            {
                SetProperty(ref _startItems, value);
            }
        }

        Command<StartItem> _startItemCommand;
        public Command<StartItem> StartItemCommand
        {
            get
            {
                _startItemCommand = _startItemCommand ?? new Command<StartItem>(DoStartItem);
                return _startItemCommand;
            }
        }

        void DoStartItem(StartItem obj)
        {

        }

        
    }
}
