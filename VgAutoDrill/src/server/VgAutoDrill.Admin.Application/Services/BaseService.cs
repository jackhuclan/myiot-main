using VgAutoDrill.Admin.Application.Helper;
using VgAutoDrill.Admin.Model.ViewModels;

namespace VgAutoDrill.Admin.Application.Services
{
    public class BaseService
    {
        public JwtUserInfo jwtUserInfo => HttpCurrentContext.GetUserInfo;

        public int UserId { get; set; }

        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BaseService()
        {
            if (HttpCurrentContext.IsAccessorNull)
            {
                //中控系统写入数据时，当前用户默认为admin
                UserId = 1;
                UserName = "admin";
            }
            else
            {
                UserId = jwtUserInfo.Id;
                UserName = jwtUserInfo.UserName;
            }
        }

        public ResponseDto<string> Success()
        {
            return new ResponseDto<string>
            {
                Code = ResponseCode.Success,
                Message = ""
            };
        }

        public ResponseDto<T> Success<T>(T data)
        {
            return new ResponseDto<T>
            {
                Code = ResponseCode.Success,
                Message = "",
                Data = data
            };
        }
        public ResponseDto<T> Success<T>(T data, string message)
        {
            return new ResponseDto<T>
            {
                Code = ResponseCode.Success,
                Message = message,
                Data = data
            };
        }
        public ResponseDto<T> Fail<T>(string message)
        {
            return new ResponseDto<T>
            {
                Code = ResponseCode.Fail,
                Message = message
            };
        }

        public ResponseDto<string> Fail(string message)
        {
            return new ResponseDto<string>
            {
                Code = ResponseCode.Fail,
                Message = message
            };
        }

        public ResponseDto<T> Fail<T>(string message, T data)
        {
            return new ResponseDto<T>
            {
                Code = ResponseCode.Fail,
                Message = message,
                Data = data
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public ResponseDto<T> Result<T>(ResponseCode code, string message)
        {
            return new ResponseDto<T>
            {
                Code = code,
                Message = message
            };
        }

    }
}
