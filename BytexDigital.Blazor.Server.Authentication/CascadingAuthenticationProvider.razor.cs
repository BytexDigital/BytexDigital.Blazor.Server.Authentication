using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using System.Threading;
using System.Threading.Tasks;

namespace BytexDigital.Blazor.Server.Authentication
{
    public partial class CascadingAuthenticationProvider : ComponentBase
    {
        [Inject]
        public IServerAuthenticationService AuthenticationService { get; set; }

        [Inject]
        public DelayedAuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        protected Task<AuthenticationState> _authenticationStateTask = default;

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        protected override void OnInitialized()
        {
            AuthenticationStateProvider.AuthenticationStateChanged += OnStateChanged;

            _authenticationStateTask = AuthenticationStateProvider.GetAuthenticationStateAsync();

            AuthenticationService.InitializeFromCookiesAsync(_cancellationTokenSource.Token).GetAwaiter().GetResult();
        }

        private async void OnStateChanged(Task<AuthenticationState> task)
        {
            await InvokeAsync(() =>
            {
                if (task != _authenticationStateTask)
                {
                    _authenticationStateTask = task;
                    StateHasChanged();
                }
            });
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            AuthenticationStateProvider.AuthenticationStateChanged -= OnStateChanged;
        }
    }
}
