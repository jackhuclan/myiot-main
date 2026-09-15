namespace VgAutoDrill.Admin.Application.Cache.Interfaces
{
    public interface IMemoryCacheManager : ICacheManager
    {
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存Value</param>
        /// <param name="expiresIn">缓存时长(分钟)</param>
        /// <returns></returns>
        bool SetBySeconds(string key, object value, int expiredSeconds);
    }
}
