namespace BussinesServices.ServiceResult
{

    public interface IResult
    {
        ServiceError? Error { get; }
    }

    public interface IResult<TData> : IResult
    {
        TData? Data { get; }
    }
}

