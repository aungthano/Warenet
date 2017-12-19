using Android.Media;
using Android.OS;
using Warenet.Mobile.Droid;
using Xamarin.Forms;
using Android.Views;
using System;

[assembly: Dependency(typeof(AudioService))]

namespace Warenet.Mobile.Droid
{
    public class AudioService : IAudio
    {
        private MediaPlayer mediaPlayer;

        public AudioService() {}

        public bool PlayWavSuccess()
        {
            var am = (AudioManager)Android.App.Application.Context.GetSystemService(Android.App.Application.AudioService);

            if (am.RingerMode == RingerMode.Normal)
            {
                mediaPlayer = MediaPlayer.Create(global::Android.App.Application.Context,Resource.Raw.beep);
                mediaPlayer.Start();
            }

            var v = (Vibrator)Android.App.Application.Context.GetSystemService(Android.App.Application.VibratorService);
            v.Vibrate(150);

            return true;
        }

        public bool PlayWavFail()
        {
            var am = (AudioManager)Android.App.Application.Context.GetSystemService(Android.App.Application.AudioService);

            if (am.RingerMode == RingerMode.Normal)
            {
                mediaPlayer = MediaPlayer.Create(global::Android.App.Application.Context, Resource.Raw.failbeep);
                mediaPlayer.Start();
            }

            var v = (Vibrator)Android.App.Application.Context.GetSystemService(Android.App.Application.VibratorService);
            v.Vibrate(300);

            return true;
        }
    }
}