using Autofac;
using Firebase.Auth;
using FriendNav.Core.IntegrationTests.Services;
using FriendNav.Core.IntegrationTests.Utilities;
using FriendNav.Core.Repositories;
using FriendNav.Core.Repositories.Interfaces;
using FriendNav.Core.Services;
using FriendNav.Core.Services.Interfaces;
using FriendNav.Core.Utilities;
using FriendNav.Core.ViewModels;
using Moq;
using MvvmCross.Core.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendNav.Core.IntegrationTests.TestModel
{
    public class TestAppContext
    {
        public IContainer TestContainer { get; set; }

        public TestNavigationService TestNavigationService { get; set; }

        public static TestAppContext ConstructTestAppContext()
        {
            var builder = new ContainerBuilder();

            var testNavigationService = new TestNavigationService();

            builder.RegisterInstance(testNavigationService)
                .As<IMvxNavigationService>();
            builder.RegisterInstance(new Mock<INotificationService>().Object);

            builder.RegisterType<TestTask>()
                .As<ITask>();

            builder.RegisterInstance(new FirebaseAuthProvider(new FirebaseConfig("AIzaSyD_zHJElZIVW3OSefLkrRY5NipPLTMsUnk")))
                .As<IFirebaseAuthProvider>();

            builder.RegisterType<FirebaseAuthService>()
                .As<IFirebaseAuthService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<FirebaseClientService>()
                .As<IFirebaseClientService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ChatRepository>()
                .As<IChatRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MessageRepository>()
                .As<IMessageRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<NavigateRequestRepository>()
                .As<INavigateRequestRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<NavigationRequestService>()
                .As<INavigationRequestService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MapRepository>()
                .As<IMapRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<TestNavigationRequestService>();

            builder.RegisterType<LoginViewModel>();

            builder.RegisterType<FriendListViewModel>();

            builder.RegisterType<ChatViewModel>();

            builder.RegisterType<RequestViewModel>();

            return new TestAppContext
            {
                TestContainer = builder.Build(),
                TestNavigationService = testNavigationService
            };
        }
    }
}
