using Android;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodApp.Droid
{
    [Activity(Label = "Ben's Kitchen", Theme = "@style/MainTheme.Splash", MainLauncher =true, NoHistory = false)]
    public class SplashActivity : Activity, Animator.IAnimatorListener
    {
        const int RequestLocationId = 0;

        readonly string[] LocationPermissions =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation,
            Manifest.Permission.Internet,
            Manifest.Permission.Camera
        };
        public void OnAnimationCancel(Animator animation)
        {
        }

        public async void OnAnimationEnd(Animator animation)
        {
            await TryToGetPermissions();
        }

        public void OnAnimationRepeat(Animator animation)
        {
        }

        public void OnAnimationStart(Animator animation)
        {
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Window.AddFlags(WindowManagerFlags.Fullscreen);
            SetContentView(Resource.Layout.Splash);
            var animationView = FindViewById<LottieAnimationView>(Resource.Id.animation_view);
            animationView.AddAnimatorListener(this);
            // Create your application here
        }

        #region RuntimePermissions

        async Task TryToGetPermissions()
        {
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                await GetPermissionsAsync();
                return;
            }
        }

        async Task GetPermissionsAsync()
        {
            int permissionNotGranted = 0;
            int requestPermission;
            int counter = 0;
            foreach(var locationAccess in LocationPermissions)
            {
                if(CheckSelfPermission(locationAccess) == (int)Android.Content.PM.Permission.Granted)
                {

                }
                else
                {
                    requestPermission = counter;
                    permissionNotGranted++;
                }
                counter++;
            }

            if(permissionNotGranted == 0)
            {
                StartActivity(new Intent(this, typeof(MainActivity)));
                return;
            }

            if (ShouldShowRequestPermissionRationale(LocationPermissions[counter-1]))
            {
                //set alert for executing the task
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("Permissions Needed");
                alert.SetMessage("The application need special permissions to continue");
                alert.SetPositiveButton("Request Permissions", (senderAlert, args) =>
                {
                    RequestPermissions(LocationPermissions, RequestLocationId);
                });

                alert.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    TryToGetPermissions();
                });

                Dialog dialog = alert.Create();
                dialog.Show();


                return;
            }

            RequestPermissions(LocationPermissions, RequestLocationId);

        }
        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] == (int)Android.Content.PM.Permission.Granted)
                        {
                            TryToGetPermissions();
                        }
                        else
                        {
                            TryToGetPermissions();
                        }
                    }
                    break;
            }
            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        #endregion
    }
}