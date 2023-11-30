using Api.Controllers;
using BussinesServices.Dto;
using Dal.Entities;
using E2E.Tests.Helpers;
using System.Net;
using System.Net.Http.Json;

namespace E2E.Tests.Extensions
{
    internal static class UserExtensions
    {
        public static async Task<T?> CreateUserAsync<T>(this HttpClient client, CreateUserRequestDto dto, HttpStatusCode expectedCode = HttpStatusCode.OK)
        {
            var response = await client.PostAsJsonAsync("/users", dto);
            Assert.Equal(expectedCode, response.StatusCode);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public static Task<UserResponseDto?> CreateRandomUserAsync(this HttpClient client)
        {
            return client.CreateUserAsync<UserResponseDto>(new CreateUserRequestDto { Id = Guid.NewGuid(), Name = Utils.RandomString(10) });
        }

        public static async Task<T?> GetUserAsync<T>(this HttpClient client, Guid id, HttpStatusCode expectedCode = HttpStatusCode.OK)
        {
            var response = await client.GetAsync($"/users/{id}");
            Assert.Equal(expectedCode, response.StatusCode);
            return await response.Content.ReadFromJsonAsync<T>();
        }
        
        public static Task<HttpResponseMessage> GetUserAsync(this HttpClient client, string id)
        {
            return client.GetAsync($"/users/{id}");
        }
        
        public static Task<BalanceResponseDto?> SetUserBalanceAsync(this HttpClient client, string userId, string currencyId, decimal balance)
        {
            return client.SetUserBalanceAsync<BalanceResponseDto>(userId, currencyId, balance, HttpStatusCode.OK);
        }

        public static async Task<T?> SetUserBalanceAsync<T>(this HttpClient client, string userId, string currencyId, decimal balance, HttpStatusCode expectedCode = HttpStatusCode.OK)
        {
            var response = await client.PutAsJsonAsync($"/users/{userId}/balance/{currencyId}", new CreateBalanceDto { Balance = balance});
            Assert.Equal(expectedCode, response.StatusCode);
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public static async Task<T?> GetUserBalanceAsync<T>(this HttpClient client, string userId, string currencyId, HttpStatusCode expectedCode = HttpStatusCode.OK)
        {
            var response = await client.GetAsync($"/users/{userId}/balance/{currencyId}");
            Assert.Equal(expectedCode, response.StatusCode);
            return await response.Content.ReadFromJsonAsync<T>();
        }
        
        public static Task<HttpResponseMessage> GetUserBalanceAsync(this HttpClient client, string userId, string currencyId)
        {
            return client.GetAsync($"/users/{userId}/balance/{currencyId}");
        }
    }
   
}
