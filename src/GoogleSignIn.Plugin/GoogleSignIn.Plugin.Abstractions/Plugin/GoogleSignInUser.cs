namespace GoogleSignIn.Plugin
{
    public class GoogleSignInUser
    {
        public string Token { get; set; }

        public override string ToString()
        {
            return "{ \n Token: " + Token + "\n}";
        }
    }
}
