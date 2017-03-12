using System;
using System.Windows.Input;
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
        public string ClientIdiOS { get; set; }

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

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(GoogleSignInButton), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
    }
}
