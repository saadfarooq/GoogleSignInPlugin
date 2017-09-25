using System;
using Xamarin.Forms;

namespace GoogleSignIn.Plugin
{
    /// <summary>
    /// GoogleSignButton control for Xamarin
    /// </summary>
    public class GoogleSignInButton : View
    {
        public SizeOptions Size { get; set; }
        public ColorSchemeOptions ColorScheme { get; set; }
        public string ServerClientId { get; set; }
        public string AndroidClientId { get; set; }
        public string IOSClientId { get; set; }
        public TokenTypeOptions TokenType { get; set; }

        public enum TokenTypeOptions
        {
            AccessToken,
            IdToken,
            ServerAuthCode
        }

        public enum SizeOptions
        {
            IconOnly,
            Standard,
            Wide
        }

        public enum ColorSchemeOptions
        {
            Light,
            Dark
        }

        public event EventHandler<GoogleSignInEventArgs> SignInEvent;

        public void OnSignIn(GoogleSignInEventArgs googleSignInEventArgs)
        {
            SignInEvent?.Invoke(this, googleSignInEventArgs);
        }
    }

    public class GoogleSignInEventArgs : EventArgs
    {
        public GoogleSignInUser user;

        public string ErrorString { get; set; }

        public bool HasError()
        {
            return !String.IsNullOrEmpty(ErrorString);
        }
    }
}
