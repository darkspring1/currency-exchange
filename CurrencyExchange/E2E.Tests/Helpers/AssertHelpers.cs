using BussinesServices.ServiceResult;
namespace E2E.Tests.Helpers
{
    internal static class AssertHelpers
    {
        public static void ExpectedServiceErrorAsync(ServiceError? error)
        {
            Assert.NotNull(error);
            Assert.NotNull(error.Message);
            Assert.True(error.Message.Length > 0);
            Assert.True(error.Code > 0);
        }
    }
}
