using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warenet.Mobile.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Warenet.Mobile.Services;
using Warenet.Mobile.Models;

namespace Warenet.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        ContentViewModel contentVm;

        public LoginPage()
        {
            InitializeComponent();

            contentVm = new ContentViewModel();
            this.AddProgressDisplay();
            this.Content.BindingContext = contentVm;

            LoadSettings();
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            bool isValidUser = await LoginUser();
            if (isValidUser)
            {
                SaveSettings();
                Application.Current.MainPage = new MDPage();
            }
        }

        void LoadSettings()
        {
            txtUserId.Text = Settings.UserId;
        }

        void SaveSettings()
        {
            Settings.UserId = txtUserId.Text;
        }

        async Task<bool> LoginUser()
        {
            contentVm.IsBusy = true;

            bool isValidUser = false;
            string userId = txtUserId.Text != null ? txtUserId.Text.Trim() : "";
            string pwd = txtPwd.Text != null ? txtPwd.Text.Trim() : "";
            string site = Settings.Site;
            bool isValidAuthData = !(string.IsNullOrEmpty(userId) ||
                                     string.IsNullOrEmpty(pwd) ||
                                     string.IsNullOrEmpty(site));

            if (isValidAuthData)
            {
                isValidUser = await LoginService.AuthenticateUser(userId, pwd, site);
                if (!isValidUser)
                {
                    lblMsg.Text = "Incorrect User Name or Password!";
                }
                else
                {
                    lblMsg.Text = "";
                }
            }

            contentVm.IsBusy = false;
            return isValidUser;
        }
    }
}