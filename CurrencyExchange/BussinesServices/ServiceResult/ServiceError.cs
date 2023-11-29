namespace BussinesServices.ServiceResult
{
    public class ServiceError(int code, string message)
    {
        public readonly int Code = code;
        public readonly string Message = message;
    }
}
