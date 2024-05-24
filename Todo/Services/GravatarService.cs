using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Todo.ServiceContracts;
using System.Linq;
using Todo.Constants;
using Microsoft.Extensions.Caching.Memory;

namespace Todo.Services
{
    public class GravatarService : IGravatarService
    {
        IHttpClientFactory _httpClientFactory;
        ILogger<GravatarService> _logger;
        IMemoryCache _cache;

        private const string _username= "_username";
        private const string _avatar = "_avatar";
        public GravatarService(IHttpClientFactory httpClientFactory, ILogger<GravatarService> logger, IMemoryCache cache)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _cache = cache;
        }

        public async Task<Tuple<string, string>> GetProfileInformation(string emailAddress)
        {
            Tuple<string,string> cachedProfileInfo = new Tuple<string, string>(string.Empty, string.Empty);
            bool cacheMiss = false;
            try
            {
                cachedProfileInfo = GetProfileInfoFromCache(emailAddress);

                if (string.IsNullOrEmpty(cachedProfileInfo.Item2))
                {
                    cacheMiss = true;
                    using (var httpClient = _httpClientFactory.CreateClient("gravatar"))
                    {
                        var httpResponseMessage = await httpClient.GetAsync($"{Gravatar.GetHash(emailAddress)}.json");

                        if (httpResponseMessage.IsSuccessStatusCode)
                        {
                            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                            var respone = JsonConvert.DeserializeObject<JObject>(responseString);
                            if (respone != null && respone["entry"].Any())
                            {
                                var entity = respone["entry"][0] as JObject;
                                cachedProfileInfo = new Tuple<string, string>(Convert.ToString(entity["displayName"]),
                                                                 Convert.ToString(entity["thumbnailUrl"]));                              
                            }
                        }
                        else
                        {                          
                            cachedProfileInfo = new Tuple<string, string>(string.Empty, ApplicationConstants.UnknownUserAvatarPath);
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured while fecthing gravatar profile for user {emailAddress}. Exception is {ex}");
            }

            if (cacheMiss) 
              AddProfileInfoInCache(emailAddress, cachedProfileInfo.Item1, cachedProfileInfo.Item2);
            
            return cachedProfileInfo;
        }

        private Tuple<string, string> GetProfileInfoFromCache(string emailAddress)
        {
            var cachedUserName = _cache.Get<string>(emailAddress + _username);
            var avatarUrl = _cache.Get<string>(emailAddress + _avatar);
            return new Tuple<string, string>(cachedUserName, avatarUrl);
        }

        private void AddProfileInfoInCache(string key, string name, string avatarUrl)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                                                .SetSlidingExpiration(TimeSpan.FromMinutes(30));
            _cache.Set(key + _username, name, cacheEntryOptions);
            _cache.Set(key + _avatar, avatarUrl, cacheEntryOptions);
        }
    }
}
