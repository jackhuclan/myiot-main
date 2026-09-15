using System.ComponentModel;

namespace VgAutoDrill.Admin.Common.Util
{
    public class GetDescriptionByEnum<T> where T : Enum
    {
        public static string GetEnumDescription(T enumValue)
        {
            try
            {
                string value = enumValue.ToString();
                System.Reflection.FieldInfo field = enumValue.GetType().GetField(value);
                if (field == null)
                    return value;
                object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (objs.Length == 0)
                    return value;
                DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
                return descriptionAttribute.Description;
            }
            catch
            {
                return "";
            }
        }
    }
}
