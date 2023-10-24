For enabling Role feature map:
1. Make sure feature.json is OK including Id, VerticalId etc
2. Make sure appsettings has ServiceId maching with id of feature.json
3. Set UseEndpointProtection inside AddCoreServices of Shohoz Bootstrapper
4. Confirm Controller header attribute [Authorize(ApiProtectionType.Protected)]
