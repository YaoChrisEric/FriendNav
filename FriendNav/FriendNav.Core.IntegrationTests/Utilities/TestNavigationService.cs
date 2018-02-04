using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FriendNav.Core.IntegrationTests.Utilities
{
    public class TestNavigationService : IMvxNavigationService
    {
        public event BeforeNavigateEventHandler BeforeNavigate;
        public event AfterNavigateEventHandler AfterNavigate;
        public event BeforeCloseEventHandler BeforeClose;
        public event AfterCloseEventHandler AfterClose;
        public event BeforeChangePresentationEventHandler BeforeChangePresentation;
        public event AfterChangePresentationEventHandler AfterChangePresentation;

        private readonly List<TestNavigation> _navigations = new List<TestNavigation>();

        public IReadOnlyCollection<TestNavigation> TestNavigations => _navigations;

        public Task<bool> CanNavigate(string path)
        {
            throw new NotImplementedException();
        }

        public bool ChangePresentation(MvxPresentationHint hint)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Close(IMvxViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Close<TResult>(IMvxViewModelResult<TResult> viewModel, TResult result)
        {
            throw new NotImplementedException();
        }

        public Task Navigate(IMvxViewModel viewModel, IMvxBundle presentationBundle = null)
        {
            return Task.Run(() => _navigations.Add(new TestNavigation
            {
                ViewModel = viewModel
            }));
        }

        public Task Navigate<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param, IMvxBundle presentationBundle = null)
        {
            return Task.Run(() => _navigations.Add(new TestNavigation
            {
                ViewModel = viewModel,
                Parameter = param
            }));
        }

        public Task<TResult> Navigate<TResult>(IMvxViewModelResult<TResult> viewModel, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<TResult> Navigate<TParameter, TResult>(IMvxViewModel<TParameter, TResult> viewModel, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task Navigate(Type viewModelType, IMvxBundle presentationBundle = null)
        {
            throw new NotImplementedException();
        }

        public Task Navigate<TParameter>(Type viewModelType, TParameter param, IMvxBundle presentationBundle = null)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> Navigate<TResult>(Type viewModelType, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<TResult> Navigate<TParameter, TResult>(Type viewModelType, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task Navigate(string path, IMvxBundle presentationBundle = null)
        {
            throw new NotImplementedException();
        }

        public Task Navigate<TParameter>(string path, TParameter param, IMvxBundle presentationBundle = null)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> Navigate<TResult>(string path, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<TResult> Navigate<TParameter, TResult>(string path, TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task Navigate<TViewModel>(IMvxBundle presentationBundle = null) where TViewModel : IMvxViewModel
        {
            throw new NotImplementedException();
        }

        public Task Navigate<TViewModel, TParameter>(TParameter param, IMvxBundle presentationBundle = null) where TViewModel : IMvxViewModel<TParameter>
        {
            return Task.Run(() => _navigations.Add(new TestNavigation
            {
                Parameter = param
            }));
        }

        public Task<TResult> Navigate<TViewModel, TResult>(IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TViewModel : IMvxViewModelResult<TResult>
        {
            throw new NotImplementedException();
        }

        public Task<TResult> Navigate<TViewModel, TParameter, TResult>(TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken)) where TViewModel : IMvxViewModel<TParameter, TResult>
        {
            throw new NotImplementedException();
        }
    }
}
