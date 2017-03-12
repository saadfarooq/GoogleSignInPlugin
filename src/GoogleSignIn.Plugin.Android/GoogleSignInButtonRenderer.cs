using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Gms.Common;
using Android.Content;
using GoogleSignIn.Plugin;
using GoogleSignIn.Plugin.Android;

[assembly: ExportRenderer(typeof(GoogleSignInButton), typeof(GoogleSignInButtonRenderer))]
namespace GoogleSignIn.Plugin.Android
{
    /// <summary>
    /// GoogleSignInButton Renderer
    /// </summary>
    public class GoogleSignInButtonRenderer : ViewRenderer<GoogleSignInButton, FrameLayout>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<GoogleSignInButton> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
            {
                SignInButton signInButton = new SignInButton(Context);
                signInButton.SetColorScheme(SignInButton.ColorDark);

                switch (this.Element.Size)
                {
                    case GoogleSignInButton.SizeOptions.IconOnly:
                        signInButton.SetSize(SignInButton.SizeIconOnly);
                        break;
                    case GoogleSignInButton.SizeOptions.Wide:
                        signInButton.SetSize(SignInButton.SizeWide);
                        break;
                    default:
                        signInButton.SetSize(SignInButton.SizeStandard);
                        break;
                }

                switch (this.Element.ColorScheme)
                {
                    case GoogleSignInButton.ColorSchemeOptions.Dark:
                        signInButton.SetColorScheme(SignInButton.ColorDark);
                        break;

                    default:
                        signInButton.SetColorScheme(SignInButton.ColorLight);
                        break;
                }

                signInButton.Click += SignInButton_Click;

                SetNativeControl(signInButton);
            }
        }

        private void SignInButton_Click(object sender, EventArgs e)
        {
            MessagingCenter.Subscribe<GoogleSignInActivity, GoogleSignInUser>(this, "Success", (s, args) =>
            {
                Console.WriteLine("Received GoogleSignInUser object: {0}", args);
                if (this.Element.Command != null)
                {
                    Console.WriteLine("Sending args to SignInButton");
                    this.Element.Command.Execute(args);
                }
            });
            Intent intent = new Intent(Forms.Context, typeof(GoogleSignInActivity));
            intent.PutExtra(GoogleSignInActivity.EXTRA_CLIENT_ID, this.Element.ClientIdiOS);
            Forms.Context.StartActivity(intent);
        }
    }
}