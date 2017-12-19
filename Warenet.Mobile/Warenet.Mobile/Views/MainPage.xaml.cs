using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warenet.Mobile.Helpers;
using Warenet.Mobile.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Warenet.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            InitHelpers();
        }

        void InitHelpers()
        {
            string serverName = Settings.ServerName;
            if (!string.IsNullOrEmpty(serverName))
            {
                LoginService.SetApiHelper(serverName);
            }
        }
    }
}