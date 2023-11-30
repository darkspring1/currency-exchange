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

        public static async Task<T?> CreateUserBalanceAsync<T>(this HttpClient client, string userId, string currencyId, CreateBalanceDto dto, HttpStatusCode expectedCode = HttpStatusCode.OK)
        {
            var response = await client.PutAsJsonAsync($"/users/{userId}/balance/{currencyId}", dto);
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
