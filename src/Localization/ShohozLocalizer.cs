namespace Platform.Infrastructure.Localization
{
    using System.Globalization;
    using Askmethat.Aspnet.JsonLocalizer.Localizer;
    using Shohoz.Platform.Infrastructure.Localization.Contracts;

    public class ShohozLocalizer : IShohozLocalizer
    {
        private readonly IJsonStringLocalizer _localizer;
        public ShohozLocalizer(IJsonStringLocalizer localizer)
        {
            this._localizer = localizer;
        }

        public string GetLocalization(string key)
        {
            var str = _localizer[key];
            return _localizer[key];
        }

        public string GetLocalization(string key, CultureInfo cultureInfo)
        {
            CultureInfo.CurrentUICulture = cultureInfo;
            CultureInfo.CurrentCulture = cultureInfo;
            return _localizer[key];
        }
    }
}
