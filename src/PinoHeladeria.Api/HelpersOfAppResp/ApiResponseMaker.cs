using PinoHeladeria.Application.DTOs;

namespace PinoHeladeria.API.HelpersOfAppResp
{
    public static class ApiResponseMaker
    {
        public static ApiResponse<T> Create<T>(int statusCode, T? Data, string message, object Meta = null)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Data = Data,
                Message = message,
                Meta = Meta
            };

        }
    }
}
