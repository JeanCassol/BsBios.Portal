using System;
using System.ComponentModel;
using System.Reflection;

namespace BsBios.Portal.Common
{
    public static class EnumExtension
    {
        public static string Descricao(this Enum enumeration)
        {
            string value = enumeration.ToString();
            Type type = enumeration.GetType();
            
            var descAttribute = (DescriptionAttribute[])type.GetField(value).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return descAttribute.Length > 0 ? descAttribute[0].Description : value;
        }

        public static Enum GetEnumByDescription(Type value, string description)
        {
            FieldInfo[] fis = value.GetFields();
            foreach (FieldInfo fi in fis)
            {
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    if (attributes[0].Description == description)
                    {
                        return (Enum)Enum.Parse(value, fi.Name);
                    }
                }
            }
            return null;
        }
    }

}
