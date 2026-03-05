namespace ChatApp.Shared.Models.Commons
{
    public class ResultBase
    {
        public string Message { get; set; }
        public bool IsSucceed { get; set; }

        public static T Success<T> (string? message = "")
            where T : ResultBase, new()
        {
            return new()
            {
                IsSucceed = true,
                Message = message
            };
        }

        public static T Failed<T> (string? message = "")
            where T : ResultBase, new()
        {
            return new()
            {
                IsSucceed = false,
                Message = message
            };
        }
    }

    public class ResultBase<T> : ResultBase
    {
        public T Data { get; set; }
    }   
}
