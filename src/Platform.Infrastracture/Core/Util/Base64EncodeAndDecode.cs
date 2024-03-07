namespace Platform.Infrastructure.Util
{
    using System;
    using System.Text;

    public static class Base64EncodeAndDecode
    {
        public static string Base64Decode(this string encodedString)
        {
            return Encoding.Default.GetString(Convert.FromBase64String(encodedString));
        }

        public static string Base64Encode(this string stringValue)
        {
            return Convert.ToBase64String(Encoding.Default.GetBytes(stringValue));
        }
    }
}
