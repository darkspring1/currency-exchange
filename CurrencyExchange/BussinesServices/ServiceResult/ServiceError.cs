namespace BussinesServices.ServiceResult
{
    public class ServiceError(int code, string message)
    {
        public int Code { get; } = code;
        public string Message { get; } = message;
    }
}
