using System.Net;

namespace ChatApp.Shared.Models.Commons
{
    public class Result
    {
        public string Error { get; set; }
        public bool IsSuccess { get; set; }
        public HttpStatusCode HttpCode { get; set; }

    }

    public class Result<T> : Result
    {
        public T Data { get; }

        protected Result(T data, bool success, string error)
        {
            Data = data;
            IsSuccess = success;
            Error = error;
        }

        protected Result(T data, bool success, string error, HttpStatusCode code)
        {
            Data = data;
            IsSuccess = success;
            Error = error;
            HttpCode = code;
        }

        public static Result<T> Success(T value)
        {
            return new(value, true, null);
        }

        public static Result<T> Failure(string? error = "")
        {
            return new(default, false, error);
        }

        public static Result<T> Failure<T>(string? error = "", HttpStatusCode code = HttpStatusCode.BadRequest)
        {
            return new(default, false, error, code);
        }
    }
}
