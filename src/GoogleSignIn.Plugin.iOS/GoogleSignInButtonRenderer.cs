﻿using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Google.SignIn;
using Foundation;
using GoogleSignIn.Plugin;
using GoogleSignIn.Plugin.iOS;
using System;

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
                SignIn.SharedInstance.ClientID = this.Element.ServerClientId;
                SignInButton signInButton = new SignInButton
                {
                    ColorScheme = ButtonColorScheme.Dark
                };

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

                switch (this.Element.ColorScheme)
                {
                    case GoogleSignInButton.ColorSchemeOptions.Dark:
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
            Console.WriteLine("User: " + user.ToString());
            if (user != null && error == null)
            {
                if (this.Element.Command != null)
                {
                    this.Element.Command.Execute(new GoogleSignInUser()
                    {
                        ServerAuthCode = user.ServerAuthCode,
                        AccessToken = user.Authentication.AccessToken
                    });
                }
            }
        }
    }
}
