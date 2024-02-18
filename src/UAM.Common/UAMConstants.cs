namespace SimpleCQRS.UAM.Common
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Constants class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class UAMConstants
    {
        public static string PASSWORD_REQUIRED => "PASSWORD_REQUIRED";

        public static string PASSWORD_TOKEN_REQUIRED => "PASSWORD_TOKEN_REQUIRED";

        public static string REQUIRED_FIELD => "REQUIRED_FIELD";      

        public static string PHONE_NUMBER_FORMAT_MUST_MATCH => "PHONE_NUMBER_FORMAT_MUST_MATCH";

        public static string CORRELATION_ID_REQUIRED => "CORRELATION_ID_REQUIRED";

        public static string OTP_CODE_REQUIRED => "OTP_CODE_REQUIRED";

        public static string OTP_TOKEN_REQUIRED => "OTP_TOKEN_REQUIRED";

        public static string USERNAME_IS_REQUIRED => "USERNAME_IS_REQUIRED";

        public static string INVALID_USERNAME_FORMAT => "INVALID_USERNAME_FORMAT";

        public static string ID_IS_REQUIRED => "ID_IS_REQUIRED";

        public static string EMAIL_IS_REQUIRED => "EMAIL_IS_REQUIRED";

        public static string PHONE_NUMBER_REQUIRED => "PHONE_NUMBER_REQUIRED";

        public static string PHONENUMBER_MIN_LENGTH => "PHONENUMBER_MIN_LENGTH";

        public static string INVALID_PHONENUMBER_FORMAT => "INVALID_PHONENUMBER_FORMAT";     

        public static string USER_ID => "sub";

        public static string PASSWORD_VALIDATION_REQUIREMENT => "PASSWORD_VALIDATION_REQUIREMENT";

        public static string VALID_EMAIL_IS_REQUIRED => "VALID_EMAIL_IS_REQUIRED";

        public static string PASSWORD_IS_REQUIRED => "PASSWORD_IS_REQUIRED";

        public static string USER_ROLE_UPDATED => "USER_ROLE_UPDATED";

        public static string ROLE_IS_REQUIRED => "ROLE_IS_REQUIRED";       

        public static string LANGUAGE_TAG_VALIDATION_REQUIREMENT => "LANGUAGE_TAG_VALIDATION_REQUIREMENT";

        public static string LANGUAGE_IS_REQUIRED => "LANGUAGE_IS_REQUIRED";

        public static string DEFAULT_LANGUAGE => "bn-BD";
     
        public static string SUCCESS => "Success";     

        public static string USER_ROLE => "user";

        public static string ADMIN_ROLE => "admin";

        public static string TEST_ADMIN_USER_ID => "97f1c1d9-4219-4fc3-adf4-19fdb9dd8846";

        public static string TEST_TENANT_ID => "97f1c1d9-4219-4fc3-adf4-19fdb9dd8846";

        public static string USER_ROLE_ID => "8e8a41a1-82c8-40d9-88fe-af5f9f6f3940";

        public static string ADMIN_ROLE_ID => "d6dcb44c-5c69-4f86-8626-b48ac5215414";
    }
}
