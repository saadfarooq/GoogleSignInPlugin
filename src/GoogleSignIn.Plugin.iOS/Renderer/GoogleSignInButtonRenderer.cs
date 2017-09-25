using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Google.SignIn;
using Foundation;
using GoogleSignIn.Plugin;
using GoogleSignIn.Plugin.iOS;
using System;
using static GoogleSignIn.Plugin.GoogleSignInButton;

[assembly: ExportRenderer(typeof(GoogleSignInButton), typeof(GoogleSignInButtonRenderer))]
namespace GoogleSignIn.Plugin.iOS
{
    /// <summary>
    /// GoogleSignInPlugin Renderer
    /// </summary>
    public class GoogleSignInButtonRenderer : ViewRenderer<GoogleSignInButton, UIControl>, ISignInDelegate
    {

        public GoogleSignInButtonRenderer() {
            Console.WriteLine("Constructed");
        }
        public static void Load() { }

        protected override void OnElementChanged(ElementChangedEventArgs<GoogleSignInButton> e)
        {
            Console.WriteLine("OnElementChanged");
            base.OnElementChanged(e);
            if (Control == null)
            {
                SignIn.SharedInstance.ClientID = this.Element.IOSClientId;
                SignIn.SharedInstance.ServerClientID = Element.ServerClientId;
                SignInButton signInButton = new SignInButton
                {
                    ColorScheme = ButtonColorScheme.Dark
                };

                switch (this.Element.Size)
                {
                    case SizeOptions.IconOnly:
                        signInButton.Style = ButtonStyle.IconOnly;
                        break;
                    case SizeOptions.Wide:
                        signInButton.Style = ButtonStyle.Wide;
                        break;
                    default:
                        signInButton.Style = ButtonStyle.Standard;
                        break;
                }

                switch (this.Element.ColorScheme)
                {
                    case ColorSchemeOptions.Dark:
                        signInButton.ColorScheme = ButtonColorScheme.Dark;
                        break;

                    default:
                        signInButton.ColorScheme = ButtonColorScheme.Light;
                        break;
                }

                SetNativeControl(signInButton);

                SignIn.SharedInstance.UIDelegate = new GoogleSignInUIDelegate();
                SignIn.SharedInstance.Delegate = this;
            }
        }

        public void DidSignIn(SignIn signIn, GoogleUser user, NSError error)
        {
            if (user != null && error == null)
            {
                Console.WriteLine("User: " + user.ToString());
                switch (Element.TokenType)
                {
                    case TokenTypeOptions.AccessToken:
                        PublishToken(user.Authentication.AccessToken);
                        break;

                    case TokenTypeOptions.IdToken:
                        PublishToken(user.Authentication.IdToken);
                        break;

                    case TokenTypeOptions.ServerAuthCode:
                        PublishToken(user.ServerAuthCode);
                        break;
                }
            }
            else
            {
                Element.OnSignIn(new GoogleSignInEventArgs()
                {
                    ErrorString = error.Description
                });
            }
        }

        private void PublishToken(string token)
        {
            Element.OnSignIn(new GoogleSignInEventArgs()
            {
                user = new GoogleSignInUser()
                {
                    Token = token
                }
            });
        }
    }
}
