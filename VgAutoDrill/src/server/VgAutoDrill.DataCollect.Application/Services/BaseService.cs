using VgAutoDrill.DataCollect.Application.Models;

namespace VgAutoDrill.DataCollect.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CollectResponseDto<string> Success()
        {
            return new CollectResponseDto<string>
            {
                Code = CollectResponseCode.Success,
                Message = ""
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public CollectResponseDto<T> Success<T>(T data)
        {
            return new CollectResponseDto<T>
            {
                Code = CollectResponseCode.Success,
                Message = "",
                Data = data
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public CollectResponseDto<T> Fail<T>(string message)
        {
            return new CollectResponseDto<T>
            {
                Code = CollectResponseCode.Fail,
                Message = message
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public CollectResponseDto<string> Fail(string message)
        {
            return new CollectResponseDto<string>
            {
                Code = CollectResponseCode.Fail,
                Message = message
            };
        }
    }
}
