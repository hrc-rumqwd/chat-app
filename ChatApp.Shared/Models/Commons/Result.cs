namespace ChatApp.Shared.Models.Commons
{
    public class Result
    {
        public string Error { get; set; }
        public bool IsSuccess { get; set; }

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

        public static Result<T> Success(T value)
            => new(value, true, null);



        public static Result<T> Failure(string? error = "")
            => new(default, false, error);
    }   
}
