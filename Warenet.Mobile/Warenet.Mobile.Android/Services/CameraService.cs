using Android.Hardware;
using static Android.Hardware.Camera;
using Android.Util;
using System;
using System.Collections.Generic;
using Warenet.Mobile;
using Warenet.Mobile.Droid;
using Xamarin.Forms;
using Java.Lang;

[assembly: Dependency(typeof(CameraService))]
namespace Warenet.Mobile.Droid
{
    public class CameraService : ICamera
    {
        private Camera camera;
        private Parameters mParams;

        public CameraService()
        {
            if (camera == null)
            {
                try
                {
                    camera = Camera.Open();
                    mParams = camera.GetParameters();
                }
                catch (RuntimeException ex)
                {
                    Log.Info("ERROR", ex.Message);
                }
            }
        }

        public bool TurnFlashLight(bool on)
        {
            if (camera == null || mParams == null)
                return false;

            if (on)
            {
                mParams = camera.GetParameters();
                mParams.FlashMode = Parameters.FlashModeTorch;
                camera.SetParameters(mParams);
                camera.StartPreview();
            }
            else
            {
                mParams = camera.GetParameters();
                mParams.FlashMode = Parameters.FlashModeOff;
                camera.SetParameters(mParams);
                camera.StartPreview();
            }

            return true;
        }

    }
}