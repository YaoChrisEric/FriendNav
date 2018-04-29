using Firebase.Auth;
using FriendNav.Core.Services.Interfaces;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using MvvmCross.Core.ViewModels;
using FriendNav.Core.Utilities;

namespace FriendNav.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            Mvx.RegisterSingleton<IFirebaseAuthProvider>(new FirebaseAuthProvider(new FirebaseConfig("AIzaSyD_zHJElZIVW3OSefLkrRY5NipPLTMsUnk")));

            CreatableTypes()
                .EndingWith("Repository")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart(new AppStart());
        }
    }
}
