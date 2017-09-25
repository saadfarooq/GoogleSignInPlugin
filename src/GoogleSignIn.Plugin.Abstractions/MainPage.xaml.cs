using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoogleSignInSample
{
    public partial class MainPage : ContentPage
    {
        public ICommand SignInCommand { get; set; }
        public MainPage()
        {
            InitializeComponent();

            SignInButton.SignInEvent += SignInButton_SignInEvent;
        }

        private void SignInButton_SignInEvent(object sender, GoogleSignIn.Plugin.GoogleSignInEventArgs e)
        {
            if (e.HasError())
            {
                Debug.WriteLine("Error occured signing in: {0}", e.ErrorString);
            } else
            {
                Debug.WriteLine("SignIn Successful: {0}", e.user);
            }
        }
    }
}
