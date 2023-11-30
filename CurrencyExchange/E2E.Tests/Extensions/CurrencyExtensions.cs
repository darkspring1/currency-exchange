using BussinesServices.Dto;
using E2E.Tests.Helpers;
using System.Net;
using System.Net.Http.Json;

namespace E2E.Tests.Extensions
{
    internal static class CurrencyExtensions
    {
        public static async Task CreateUsdAsync(this HttpClient client)
        {
            var response = await client.PostAsJsonAsync("/currencies", new CreateCurrencyDto { Id = "usd", Name = "United States Dollar"});
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        public static async Task CreateRubAsync(this HttpClient client)
        {
            var response = await client.PostAsJsonAsync("/currencies", new CreateCurrencyDto { Id = "Rub", Name = "Russian Ruble"});
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        public static async Task<T?> CreateCurrencyAsync<T>(this HttpClient client, CreateCurrencyDto dto, HttpStatusCode expectedCode = HttpStatusCode.OK)
        {
            var response = await client.PostAsJsonAsync("/currencies", dto);
            Assert.Equal(expectedCode, response.StatusCode);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        //public static Task<HttpResponseMessage> CreateCurrencyAsync(this HttpClient client, CreateCurrencyDto dto)
        //{
        //    return client.PostAsJsonAsync("/currencies", dto);
        //}


        public static async Task<CurrencyResponseDto> CreateRandomCurrencyAsync(this HttpClient client)
        {
            var name = Guid.NewGuid().ToString();

            for(var i = 0; i < 10; i++)
            {
                var id = Utils.RandomString(3);
                var dto = await client.CreateCurrencyAsync<CurrencyResponseDto>(new CreateCurrencyDto { Id = id, Name = name });

                Assert.NotNull(dto);

                if (dto.Name == name)
                {
                    return dto;
                }

                // collision, try create again with new random id
            }

            throw new Exception("Can't create random currency. Too many collisions, try to cleanup currency table");

        }
        
        public static async Task<T?> GetCurrencyAsync<T>(this HttpClient client, string id, HttpStatusCode expectedCode = HttpStatusCode.OK)
        {
            var response = await client.GetAsync($"/currencies/{id}");
            Assert.Equal(expectedCode, response.StatusCode);
            return await response.Content.ReadFromJsonAsync<T>();
        }
        
        public static Task<HttpResponseMessage> GetCurrencyAsync(this HttpClient client, string id)
        {
            return client.GetAsync($"/currencies/{id}");
        }
    }
   
}
