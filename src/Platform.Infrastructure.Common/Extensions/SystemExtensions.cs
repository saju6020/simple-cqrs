namespace Platform.Infrastructure.Common.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    public static class SystemExtensions
    {
        public static string GetValue(this Enum e)
        {
            var attribute =
                e.GetType()
                    .GetTypeInfo()
                    .GetMember(e.ToString())
                    .FirstOrDefault(member => member.MemberType == MemberTypes.Field)
                    .GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .SingleOrDefault()
                    as DescriptionAttribute;

            return attribute?.Description ?? e.ToString();
        }

        public static T GetEnum<T>(this Guid value)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (Guid.Parse(attribute.Description) == value)
                        return (T)field.GetValue(null);
                }
            }
            return default(T);
        }
    }
}
