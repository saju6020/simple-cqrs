namespace SimpleCQRS.UAM.Common
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public static class ErrorCodes
    {
        public static string UnexpectedError => "40050";

        public static string OtpExpired => "40051";

        public static string UserAlreadyExists => "40052";

        public static string InvalidTokenNoTempUser => "40053";

        public static string InvalidTokenNoUserId => "40054";

        public static string InvalidOtpCode => "40055";

        public static string CouldNotParseToken => "40056";

        public static string InvalidTokenPhoneNumberMismatch => "40057";

        public static string UserNotFound => "40058";

        public static string InvalidTokenNoPhoneNumber => "40059";

        public static string InvalidTokenClientIdMismatch => "40060";

        public static string InvalidTokenNoClientId => "40061";

        public static string InvalidTokenNoTokenTypeClaim => "40062";

        public static string PhoneNumberIsUsedAlready => "40063";

        public static string UserNameIsUsedAlready => "40064";

        public static string UserLockedOut => "40065";

        public static string InvalidTokenClaimNotFound => "40066";

        public static string InvalidVerifyToken => "40067";

        public static string RateLimitReached => "40068";

        public static string SsoTokenExpired => "40069";

        public static string InvalidTokenType => "40070";

        public static string SignInNotAllowed => "40071";

        public static string RequiresTwoFactor => "40072";

        public static string PasswordSignInFailed => "40073";

        public static string OtpTokenNullOrEmpty => "40074";

        public static string OldPasswordMismatch => "40075";

        public static string EmailIsUsedAlready => "40076";

        public static string InvalidPayloadValue => "40077";

    }
}
