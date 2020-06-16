![Nuget](https://img.shields.io/nuget/vpre/BytexDigital.Blazor.Server.Authentication.svg?style=flat-square)

# BytexDigital.Blazor.Server.Authentication

This library adds a simple way of being able to sign in, sign out and remember the signed in user (e.g. via cookies) directly from Blazor without the need of redirecting the user to Razor pages.

Existing Blazor components such as `AuthorizeView` will continue to work as expected.

> :warning: **This library is meant only for server-sided Blazor.**

## Download
[:arrow_forward: **BytexDigital.Blazor.Server.Authentication** Nuget package](https://www.nuget.org/packages/BytexDigital.Blazor.Server.Authentication/)

[:arrow_forward: **BytexDigital Blazor.Server.Authentication.Identity** Nuget package](https://www.nuget.org/packages/BytexDigital.Blazor.Server.Authentication.Identity/)

## How to use?
#### 1. Install BytexDigital.Blazor.Server.Authentication.Identity
#### 2. Register the necessary services
Make sure to register the services **after other calls to add Authentication services**.

##### With Identity
```csharp
services
    .AddAuthenticationService()
    .AddCookiePrincipalStorage()
    .AddIdentityPrincipalProvider<ApplicationUser>();
```

##### Without Identity
```csharp
services
    .AddAuthenticationService()
    .AddCookiePrincipalStorage()
    .AddPrincipalProvider<ImplementationOfIPrincipalProvider>();
```

#### 3. Add the Javascript to your `_Host.cshtml`
```html
<script src="/_content/BytexDigital.Blazor.Server.Authentication/bundle.js"></script>
```

#### Edit your `App.razor` to wrap your content in a `CascadingAuthenticationProvider`

```cshtml
<BytexDigital.Blazor.Server.Authentication.CascadingAuthenticationProvider>
	<Router AppAssembly="@typeof(Program).Assembly">
		<Found Context="routeData">
			<RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
		</Found>
		<NotFound>
			<LayoutView Layout="@typeof(MainLayout)">
				<NotFound></NotFound>
			</LayoutView>
		</NotFound>
	</Router>
</BytexDigital.Blazor.Server.Authentication.CascadingAuthenticationProvider>
```


## Sign in and sign out
Use `IServerAuthenticationService.SignInAsAsync` and `IServerAuthenticationService.SignOutAsync` to change the signed in user.
Reloading the page is not necessary. You can use `IServerAuthenticationService.GetSignedInIdOrDefault` and `IServerAuthenticationService.IsSignedIn` to get information about the current authentication status.

## Questions
### What is the `IPrincipalProvider`?
The `IPrincipalProvider` is used to convert a user id to a `ClaimsPrincipal` object. The included `IdentityPrincipalProvider` will convert the user id to a principal using a `UserStore<TUser>`.