using System.ComponentModel;

namespace VgAutoDrill.Admin.Common.Util
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="val">枚举值</param>
        /// <returns>描述</returns>
        public static string GetDescription(this Enum val)
        {
            if (val == null)
            {
                return string.Empty;
            }
            var field = val.GetType().GetField(val.ToString());
            if (field == null)
            {
                return val.ToString();
            }
            var customAttribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return customAttribute == null ? val.ToString() : ((DescriptionAttribute)customAttribute).Description;
        }
    }
}
