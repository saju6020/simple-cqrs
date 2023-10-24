// <copyright file="RequireLogedIn.cs" company="Shohoz">
// Copyright © 2015-2020 Shohoz. All Rights Reserved.
// </copyright>

namespace Shohoz.Platform.Infrastructure.EndpointRoleFeatureMap.AuthorizationHandlers
{
    using Microsoft.AspNetCore.Authorization;

    public class RequireLogedIn : IAuthorizationRequirement
    {
    }
}
