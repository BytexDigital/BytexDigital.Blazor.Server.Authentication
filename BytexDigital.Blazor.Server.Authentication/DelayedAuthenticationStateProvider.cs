using Microsoft.AspNetCore.Components.Authorization;

using Nito.AsyncEx;

using System;
using System.Threading.Tasks;

namespace BytexDigital.Blazor.Server.Authentication
{
    public class DelayedAuthenticationStateProvider : Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider
    {
        private AsyncAutoResetEvent _initialStateReady = new AsyncAutoResetEvent(false);
        private bool _isHoldingFakeAuthTask;
        private Task<AuthenticationState> _authenticationStateTask;
        private Task<AuthenticationState> _forFakeStateTaskRealTask;

        public DelayedAuthenticationStateProvider()
        {
            _isHoldingFakeAuthTask = true;
            _authenticationStateTask = Task.Run(async () =>
            {
                await _initialStateReady.WaitAsync();
                return await _forFakeStateTaskRealTask;
            });
        }

        public void SetAuthenticationStateTask(Task<AuthenticationState> task)
        {
            if (_isHoldingFakeAuthTask)
            {
                _forFakeStateTaskRealTask = task;
                _isHoldingFakeAuthTask = false;

                if (!_initialStateReady.IsSet)
                {
                    _initialStateReady.Set();
                }

                //NotifyAuthenticationStateChanged(_authenticationStateTask);
            }
            else
            {
                _authenticationStateTask = task;

                if (!_initialStateReady.IsSet)
                {
                    _initialStateReady.Set();
                }

                NotifyAuthenticationStateChanged(_authenticationStateTask);
            }
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync() => _authenticationStateTask;

        public void Dispose()
        {

        }
    }
}
