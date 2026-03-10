namespace PosNet.Domain.Shared
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public T Data { get; }

        private Result(bool success, T data)
        {
            Success = success;
            Data = data;
        }

        public static Result<T> Ok(T data) => new Result<T>(true, data);
        public static Result<T> Fail() => new Result<T>(false, default);
    }
}
