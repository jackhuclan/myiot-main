namespace VgAutoDrill.Admin.Common.Util
{
    public static class StringHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetUUID()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static bool ContainsTypeId(string? ancestors, int? typeId)
        {
            if (string.IsNullOrEmpty(ancestors) || !typeId.HasValue)
            {
                return false;
            }

            var inputs = ancestors.Split(',', StringSplitOptions.RemoveEmptyEntries);
            return inputs.Any(str => str == typeId.ToString());
        }
    }
}
