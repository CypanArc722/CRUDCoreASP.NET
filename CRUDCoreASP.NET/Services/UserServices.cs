using CRUDCoreASP.NET.Database;
using CRUDCoreASP.NET.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CRUDCoreASP.NET.Services
{
    public class UserServices : IUserServices
    {
        #region Private Fields
        private readonly UserDatabaseContext _context;
        private readonly API _api;
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        string AccessToken = string.Empty;
        #endregion

        #region Constructor
        public UserServices(UserDatabaseContext _context, IOptions<API> apiOptions, HttpClient _httpClient, IMemoryCache _cache) { 
            this._context = _context;
            _api = apiOptions.Value;
            this._httpClient = _httpClient;
            this._cache = _cache;
        }
        #endregion

        #region Methods
        public async Task<string> GetToken()
        {
            if (_cache.TryGetValue("ExpressApiToken", out string? cachedToken))
            {
                return cachedToken;
            }

            var credentials = new { _api.Username, _api.Password };
            var response = await _httpClient.PostAsJsonAsync($"{_api.ApiLink?.TrimEnd('/')}/GenerateToken", credentials);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
            string newToken = result?.AccessToken ?? string.Empty;

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(9));

            _cache.Set("ExpressApiToken", newToken, cacheOptions);

            return newToken;
        }

        public async Task<List<Users>> CreateUser(Users user)
        {
            string token = await GetToken();
            var request = new HttpRequestMessage(HttpMethod.Post, _api.ApiLink + "adduser");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(user);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await GetAllUsers();

        }

        public async Task<bool> DeleteUser(int id)
        {
            string token = await GetToken();
            var request = new HttpRequestMessage(HttpMethod.Delete, _api.ApiLink + $"deleteuser/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;
        }

        public async Task<List<Users>> GetAllUsers()
        {
            string token = await GetToken();
            var request = new HttpRequestMessage(HttpMethod.Get, _api.ApiLink + "viewusers");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<Users>>() ?? new List<Users>();
        }

        public async Task<List<Users>> UpdateUser(Users user)
        {
            string token = await GetToken();
            var request = new HttpRequestMessage(HttpMethod.Put, _api.ApiLink + $"updateuser/{user.Id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(user);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await GetAllUsers();
        }

        #endregion
    }
}
