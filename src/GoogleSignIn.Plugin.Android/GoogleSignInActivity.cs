using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Android.Gms.Auth.Api;
using static Android.Gms.Common.Apis.GoogleApiClient;

namespace GoogleSignIn.Plugin.Android
{
    [Activity(Label = "GoogleSignInActivity", Theme = "@android:style/Theme.Translucent")]
    public class GoogleSignInActivity : Activity, IConnectionCallbacks
    {
        private GoogleApiClient googleApiClient;
        private readonly int RC_SIGN_IN = 10;
        public const string EXTRA_CLIENT_ID = "GoogleSignIn.Plugin.Android.GoogleSignInActivity.EXTRA_CLIENT_ID";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            var clientId = this.Intent.Extras.GetString(EXTRA_CLIENT_ID);

            Console.WriteLine("Requesting client id: {0}", clientId);

            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestIdToken(clientId)
                .RequestEmail()
                .Build();

            googleApiClient = new Builder(this)
                .AddConnectionCallbacks(this)
                .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                .AddOnConnectionFailedListener((result) =>
                {
                    Console.WriteLine("Got failure: {0}", result);
                })
                .Build();
            googleApiClient.Connect();
        }

        public void OnConnected(Bundle connectionHint)
        {
            Intent signInIntent = Auth.GoogleSignInApi.GetSignInIntent(googleApiClient);
            StartActivityForResult(signInIntent, RC_SIGN_IN);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == RC_SIGN_IN)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                if (result.IsSuccess)
                {
                    GoogleSignInAccount acct = result.SignInAccount;
                    Console.WriteLine("Sign in with Google user: {0}", acct);
                    // Get account information
                    //MessagingCenter.Send(this, "Success", new GoogleSignInUser()
                    //{
                    //    ServerAuthCode = acct.IdToken
                    //});
                }
                else
                {
                    var statusCodeString = GoogleSignInStatusCodes.GetStatusCodeString(result.Status.StatusCode);
                    Console.WriteLine("Sign in with Google failed: {0}", statusCodeString);
                }
            }
            Finish();
        }

        public void OnConnectionSuspended(int cause)
        {

        }
    }
}