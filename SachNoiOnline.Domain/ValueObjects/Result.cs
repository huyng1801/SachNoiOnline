using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SachNoiOnline.Domain.ValueObjects
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public Result(bool success, string message, T data = default)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static Result<T> SuccessResult(T data, string message = "Success")
        {
            return new Result<T>(true, message, data);
        }

        public static Result<T> FailureResult(string message)
        {
            return new Result<T>(false, message);
        }
    }
}
