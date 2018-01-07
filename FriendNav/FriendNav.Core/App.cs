using Firebase.Auth;
using FriendNav.Core.Services.Interfaces;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;

namespace FriendNav.Core
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            Mvx.RegisterSingleton<IFirebaseAuthProvider>(new FirebaseAuthProvider(new FirebaseConfig("AIzaSyD_zHJElZIVW3OSefLkrRY5NipPLTMsUnk")));

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart(new AppStart());
        }
    }
}
