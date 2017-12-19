using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warenet.Mobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Warenet.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MDPage : MasterDetailPage
    {
        public MDPage()
        {
            InitializeComponent();
            masterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MDPageMenuItem;
            if (item == null) return;

            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;

            if (item.Title == "Logout")
            {
                Application.Current.MainPage = new MainPage();
            }
            else
            {
                Detail = new NavigationPage(page);
                // close the slide-out
                IsPresented = false;
            }
        }
    }
}