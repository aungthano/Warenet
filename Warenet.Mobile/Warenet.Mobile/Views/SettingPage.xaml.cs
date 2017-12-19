using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Warenet.Mobile.Models;
using Warenet.Mobile.Services;
using Warenet.Mobile.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Warenet.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        ContentViewModel contentVm;

        public SettingPage()
        {
            InitializeComponent();

            contentVm = new ContentViewModel();
            this.AddProgressDisplay();
            this.Content.BindingContext = contentVm;

            txtServerName.Text = Settings.ServerName;

            LoadSettings();
        }

        private void btnTestCon_Clicked(object sender, EventArgs e)
        {
            TestConnection();
        }

        private async void btnRefresh_Clicked(object sender, EventArgs e)
        {
            await LoadSites();
        }

        private void btnSaveSettings_Clicked(object sender, EventArgs e)
        {
            SaveSettings();
        }

        async Task<bool> LoadSites()
        {
            contentVm.IsBusy = true;
            bool result = false;
            pkrSite.Items.Clear();

            string serverName = txtServerName.Text.Trim();
            if (!string.IsNullOrEmpty(serverName))
            {
                if (LoginService.SetApiHelper(serverName))
                {
                    string[] sites = await LoginService.GetSites();
                    if (sites != null)
                    {
                        foreach (string site in sites)
                        {
                            pkrSite.Items.Add(site);
                        }
                    }
                }
                result = true;
            }
            
            contentVm.IsBusy = false;
            return result;
        }

        async void TestConnection()
        {
            contentVm.IsBusy = true;

            string site = (string)pkrSite.SelectedItem;
            if (!string.IsNullOrEmpty(site))
            {
                bool isValid = await LoginService.IsValidSite(site);
                if (isValid)
                {
                    await DisplayAlert("Connection","Test connection succeeded.","OK");
                }
                else
                {
                    await DisplayAlert("Connection", "Test connection failed!", "OK");
                }
            }

            contentVm.IsBusy = false;
        }

        void SaveSettings()
        {
            string serverName = txtServerName.Text;
            string site = (string)pkrSite.SelectedItem;

            if (!string.IsNullOrWhiteSpace(serverName))
            {
                Settings.ServerName = serverName;
            }

            if (!string.IsNullOrWhiteSpace(site))
            {
                Settings.Site = site;
            }
        }

        async void LoadSettings()
        {
            txtServerName.Text = Settings.ServerName;

            bool isLoaded = await LoadSites();
            if (isLoaded)
            {
                string defaultSite = Settings.Site;
                string[] sites = pkrSite.Items.ToArray();
                if (sites.Contains(defaultSite))
                {
                    pkrSite.SelectedItem = defaultSite;
                }
                else
                {
                    await DisplayAlert("Settings", "Load setting failed!", "OK");
                }
            }
        }
    }

    public class ContentViewModel : INotifyPropertyChanged
    {
        public ContentViewModel() { }

        private bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged("IsBusy");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}