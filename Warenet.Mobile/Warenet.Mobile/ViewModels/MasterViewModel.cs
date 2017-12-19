using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Warenet.Mobile.Models;
using Warenet.Mobile.Views;

namespace Warenet.Mobile.ViewModels
{
    class MasterViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<MDPageMenuItem> MenuItems { get; set; }

        public MasterViewModel()
        {
            MenuItems = new ObservableCollection<MDPageMenuItem>(new[]
            {
                new MDPageMenuItem { Id = 0, Title = "GRN Barcode", TargetType = typeof(GrnBarCode), Visible = true },
                new MDPageMenuItem { Id = 0, Title = "Logout", TargetType = typeof(MainPage), Visible = true },
            });
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}