using Firebase.Auth;
using MvvmCross.Core.ViewModels;
using System.Threading.Tasks;

namespace FriendNav.Core.ViewModels
{
    public class FirstViewModel
        : MvxViewModel
    {

        public async override Task Initialize()
        {
            await base.Initialize();

            var authProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyD_zHJElZIVW3OSefLkrRY5NipPLTMsUnk"));

            var auth = await authProvider.SignInWithEmailAndPasswordAsync("c@test.com", "theday");
        }

        string hello = "Hello MvvmCross";
        public string Hello
        {
            get { return hello; }
            set { SetProperty(ref hello, value); }
        }
    }
}
