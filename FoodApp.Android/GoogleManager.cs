using Android.App;
using Android.Content;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FoodApp.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using FoodApp.Interface;
using FoodApp.Models;
using Android.Gms.Auth.Api;
using System.IO;

[assembly: Dependency(typeof(GoogleManager))]
namespace FoodApp.Droid
{
    public class GoogleManager : Java.Lang.Object, IGoogleManager, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        public Action<GoogleUser, string> _onLoginComplete;
        public static GoogleSignInClient _googleApiClient { get; set; }
        private const int RC_SIGN_IN = 9001;
        public static GoogleManager Instance { get; private set; }
        Context _context;

        public GoogleManager()
        {
            _context = global::Android.App.Application.Context;
            Instance = this;
        }

        public void Login(Action<GoogleUser, string> onLoginComplete)
        {
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestEmail()
                .RequestProfile()
                .Build();
            _googleApiClient = GoogleSignIn.GetClient((_context).ApplicationContext, gso);

            _onLoginComplete = onLoginComplete;
            Intent signInIntent = _googleApiClient.SignInIntent;
            ((MainActivity)Forms.Context).StartActivityForResult(signInIntent, RC_SIGN_IN);
            //_googleApiClient.Connect();
        }

        public void Logout()
        {
            var gsoBuilder = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn).RequestEmail();

            GoogleSignIn.GetClient(_context, gsoBuilder.Build())?.SignOut();
            try
            {
                _googleApiClient.Dispose();
            }
            catch(Exception e)
            {

            }

        }

        public void OnAuthCompleted(GoogleSignInResult result)
        {
            if (result.IsSuccess)
            {
                GoogleSignInAccount accountt = result.SignInAccount;
                _onLoginComplete?.Invoke(new GoogleUser()
                {
                    Name = accountt.DisplayName,
                    Email = accountt.Email,
                    Picture = new Uri((accountt.PhotoUrl != null ? $"{accountt.PhotoUrl}" : $"https://autisticdating.net/imgs/profile-placeholder.jpg"))
                }, string.Empty);
            }
            else
            {
                _onLoginComplete?.Invoke(null, "An error occured!");
            }
        }

        public void OnConnected(Bundle connectionHint)
        {

        }

        public void OnConnectionSuspended(int cause)
        {
            _onLoginComplete?.Invoke(null, "Canceled!");
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            _onLoginComplete?.Invoke(null, result.ErrorMessage);
        }
    }
}