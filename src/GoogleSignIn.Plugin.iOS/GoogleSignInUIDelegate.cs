using Foundation;
using Google.SignIn;
using UIKit;

namespace GoogleSignIn.Plugin.iOS
{
    class GoogleSignInUIDelegate : SignInUIDelegate
    {
        public override void WillDispatch(SignIn signIn, NSError error)
        {
        }
        public override void PresentViewController(SignIn signIn, UIViewController viewController)
        {
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(viewController, true, null);

        }

        public override void DismissViewController(SignIn signIn, UIViewController viewController)
        {
            UIApplication.SharedApplication.KeyWindow.RootViewController.DismissViewController(true, null);
        }
    }
}
