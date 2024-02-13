namespace Shohoz.Platform.Infrastructure.Localization.Contracts
{
    using System.Globalization;

    public interface IShohozLocalizer
    {
        string GetLocalization(string key);
        string GetLocalization(string key,CultureInfo cultureInfo);
    }
}
