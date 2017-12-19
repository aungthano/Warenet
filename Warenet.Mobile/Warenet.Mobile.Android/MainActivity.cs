using Android.App;
using Android.Content.PM;
using Android.OS;
using ZXing.Mobile;

[assembly: UsesPermission(Android.Manifest.Permission.Flashlight)]
namespace Warenet.Mobile.Droid
{
    [Activity(Label = "Warenet.Mobile", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            //TabLayoutResource = Resource.Layout.Tabbar;
            //ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            MobileBarcodeScanner.Initialize(Application);
        }
    }
}