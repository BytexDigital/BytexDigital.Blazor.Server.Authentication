﻿using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BytexDigital.Blazor.Server.Authentication
{
    public interface IPrincipalProvider
    {
        Task<ClaimsPrincipal> CreateClaimsPrincipalAsync(string userId, CancellationToken cancellationToken = default);
    }
}
