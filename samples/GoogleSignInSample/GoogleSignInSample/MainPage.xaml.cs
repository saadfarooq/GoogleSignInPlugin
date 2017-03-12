using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            SignInButton.Command = new Command((signInUser) =>
            {
                Debug.WriteLine("SignInUser: {0}", signInUser);
            });
        }


    }
}
