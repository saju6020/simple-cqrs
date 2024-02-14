// <copyright file="SwaggerAuthConfig.cs" company="Shohoz">
// Copyright © 2015-2021 Shohoz. All Rights Reserved.
// </copyright>

namespace Shohoz.Platform.Infrastructure.Swagger.Configs
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    ///   SwaggerAuthConfig
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SwaggerAuthConfig
    {
        /// <summary>
        ///  Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///  Password
        /// </summary>
        public string Password { get; set; }
    }
}
