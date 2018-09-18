using System;
using MvvmHelpers;

namespace FinWin.Models
{
    public class BaseObject : ObservableObject
    {
        public string this[string index]
        {
            get
            {
                //var ret = AppResources.ResourceManager.GetString(index, AppResources.Culture);
                return index;
            }
        }
    }
}
