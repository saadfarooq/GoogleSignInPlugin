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
using Xamarin.Forms;
using System.Threading.Tasks;
using Android.Gms.Auth;
using Android.Gms.Common;
using Android.Gms.Plus;
using static GoogleSignIn.Plugin.GoogleSignInButton;

namespace GoogleSignIn.Plugin.Android
{
    [Activity(Label = "GoogleSignInActivity", Theme = "@android:style/Theme.Translucent")]
    public class GoogleSignInActivity : Activity, IConnectionCallbacks
    {
        private GoogleApiClient googleApiClient;
        private readonly int RC_SIGN_IN = 10;
        public const string EXTRA_CLIENT_ID = "GoogleSignIn.Plugin.Android.GoogleSignInActivity.EXTRA_CLIENT_ID";
        public const string EXTRA_SERVER_CLIENT_ID = "GoogleSignIn.Plugin.Android.GoogleSignInActivity.EXTRA_SERVER_CLIENT_ID";
        public const string EXTRA_TOKEN_TYPE = "GoogleSignIn.Plugin.Android.GoogleSignInActivity.EXTRA_TOKEN_TYPE";
        private TokenTypeOptions tokenTypeEnum;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            var clientId = Intent.Extras.GetString(EXTRA_CLIENT_ID);
            var serverClientId = Intent.GetStringExtra(EXTRA_SERVER_CLIENT_ID);

            Console.WriteLine("Requesting client id: {0}", clientId);

            var gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestEmail();

            string tokenType = Intent.GetStringExtra(EXTRA_TOKEN_TYPE);
            tokenTypeEnum = (TokenTypeOptions) Enum.Parse(typeof(TokenTypeOptions), tokenType);
            switch (tokenTypeEnum)
            {
                case TokenTypeOptions.AccessToken:
                    // no-op
                    break;

                case TokenTypeOptions.IdToken:
                    gso.RequestIdToken(serverClientId);
                    break;

                case TokenTypeOptions.ServerAuthCode:
                    gso.RequestServerAuthCode(serverClientId);
                    break;
            }
            
            gso.Build();

            googleApiClient = new Builder(this)
                .AddConnectionCallbacks(this)
                .AddApi(Auth.GOOGLE_SIGN_IN_API, gso.Build())
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

        protected override async void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == RC_SIGN_IN)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                if (result.IsSuccess)
                {
                    GoogleSignInAccount acct = result.SignInAccount;
                    Console.WriteLine("Sign in with Google user: {0}", acct);

                    switch(tokenTypeEnum)
                    {
                        case TokenTypeOptions.AccessToken:
                            string accessToken = await GetAccessToken(this, acct.Email);
                            PublishToken(accessToken);
                            break;

                        case TokenTypeOptions.IdToken:
                            PublishToken(acct.IdToken);
                            break;

                        case TokenTypeOptions.ServerAuthCode:
                            PublishToken(acct.ServerAuthCode);
                            break;   
                    }
                }
                else
                {
                    var statusCodeString = GoogleSignInStatusCodes.GetStatusCodeString(result.Status.StatusCode);
                    Console.WriteLine("Sign in with Google failed: {0}", statusCodeString);
                    MessagingCenter.Send(this, "Failure", statusCodeString);
                }
            }
            Finish();
        }

        private void PublishToken(string accessToken)
        {
            MessagingCenter.Send(this, "Success", new GoogleSignInUser()
            {
                Token = accessToken
            });
        }

        private Task<string> GetAccessToken(Context context, string accountName)
        {
            return Task.Run<string>(() =>
            {
                return GoogleAuthUtil.GetToken(
                context: context,
                accountName: accountName,
                scope: "oauth2:" + Scopes.Email + " " + Scopes.Profile);
            });
        }

        public void OnConnectionSuspended(int cause)
        {

        }
    }
}