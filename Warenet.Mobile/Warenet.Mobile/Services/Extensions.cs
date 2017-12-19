using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Warenet.Mobile.Services
{
    public static class Extensions
    {
        public static void AddProgressDisplay(this ContentPage page)
        {
            var content = page.Content;

            var grid = new Grid();
            grid.Children.Add(content);
            var gridProgress = new Grid { BackgroundColor = Color.FromHex("#64FFE0B2"), Padding = new Thickness(50) };
            gridProgress.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            gridProgress.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            gridProgress.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            gridProgress.SetBinding(VisualElement.IsVisibleProperty, "IsBusy");
            var activity = new ActivityIndicator
            {
                IsEnabled = true,
                IsVisible = true,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                IsRunning = true,
            };
            gridProgress.Children.Add(activity, 0, 1);
            grid.Children.Add(gridProgress);
            page.Content = grid;
        }
    }
}