namespace Platform.Infrastructure.Localization.Service
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Microsoft.Extensions.DependencyInjection;
    using Askmethat.Aspnet.JsonLocalizer.Extensions;
    using Shohoz.Platform.Infrastructure.Localization.Contracts;


    public static class LocalizationProvider
    {
        public static void UseShohozLocalization(this IServiceCollection services, int cacheDurationInDays = 30)
        {
            services.AddJsonLocalization(opt =>
            {
                opt.CacheDuration = TimeSpan.FromDays(cacheDurationInDays);
                opt.ResourcesPath = "Resource";
                opt.SupportedCultureInfos = new HashSet<CultureInfo>()
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("bn-BD")
                };
                opt.DefaultCulture = new CultureInfo("en-US");
                opt.DefaultUICulture = new CultureInfo("en-US");
            });
            services.AddScoped<IShohozLocalizer, ShohozLocalizer>();
        }

    }
}
