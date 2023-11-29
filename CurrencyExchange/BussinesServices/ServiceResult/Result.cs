namespace BussinesServices.ServiceResult
{
    internal class Result(ServiceError? error) : IResult
    {
        public ServiceError? Error { get; } = error;

        public static Result Success() => new Result(null);
        public static Result Fail(ServiceError error) => new Result(error);

        public static Result<TResult> Success<TResult>() where TResult : class
            => new Result<TResult>(null, null);
        public static Result<TResult> Success<TResult>(TResult result) where TResult : class
            => new Result<TResult>(result, null);
        public static Result<TResult> Fail<TResult>(ServiceError error) where TResult : class
            => new Result<TResult>(null, error);
    }

    internal class Result<TResult> : Result, IResult<TResult>
         where TResult : class
    {

        public Result(TResult? result, ServiceError? error): base(error)
        {
            Data = result;
        }
        public TResult? Data { get; }

        public static Result<TResult> SuccessResult(TResult result) => new Result<TResult>(result, null);
    }
}

