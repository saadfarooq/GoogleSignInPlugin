using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Google.SignIn;
using Foundation;
using GoogleSignIn.Plugin;
using GoogleSignIn.Plugin.iOS;

[assembly: ExportRenderer(typeof(GoogleSignInButton), typeof(GoogleSignInButtonRenderer))]
namespace GoogleSignIn.Plugin.iOS
{
    /// <summary>
    /// GoogleSignInPlugin Renderer
    /// </summary>
    public class GoogleSignInButtonRenderer : ViewRenderer<GoogleSignInButton, UIControl>, ISignInDelegate
    {

        protected override void OnElementChanged(ElementChangedEventArgs<GoogleSignInButton> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
            {
                SignIn.SharedInstance.ClientID = this.Element.ClientIdiOS;
                SignInButton signInButton = new SignInButton();

                switch (this.Element.Size)
                {
                    case GoogleSignInButton.SizeOptions.IconOnly:
                        signInButton.Style = ButtonStyle.IconOnly;
                        break;
                    case GoogleSignInButton.SizeOptions.Wide:
                        signInButton.Style = ButtonStyle.Wide;
                        break;
                    default:
                        signInButton.Style = ButtonStyle.Standard;
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
                if (this.Element.Command != null)
                {
                    this.Element.Command.Execute(new GoogleSignInUser()
                    {
                        ServerAccessToken = user.ServerAuthCode
                    });
                }
            }
        }
    }
}
