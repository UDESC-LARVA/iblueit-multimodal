using System;
using System.ComponentModel;
using System.Linq;

namespace Assets._Game.Scripts.Core.Api.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum genericEnum)
        {
            var genericEnumType = genericEnum.GetType();
            var memberInfo = genericEnumType.GetMember(genericEnum.ToString());
            if (memberInfo.Length <= 0) return genericEnum.ToString();
            var attribs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attribs.Any() ? ((DescriptionAttribute)attribs.ElementAt(0)).Description : genericEnum.ToString();
        }

        //Code from https://stackoverflow.com/a/4367868/13254193
        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", nameof(description));
            // or return default(T);
        }
    }
}