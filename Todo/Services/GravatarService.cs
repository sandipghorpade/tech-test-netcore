using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Todo.ServiceContracts;
using System.Linq;

namespace Todo.Services
{
    public class GravatarService : IGravatarService
    {
        IHttpClientFactory _httpClientFactory;
        ILogger<GravatarService> _logger;

        public GravatarService(IHttpClientFactory httpClientFactory, ILogger<GravatarService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<Tuple<string, string>> GetProfileInformation(string emailAddress)
        {
            try
            {
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
                            return new Tuple<string, string>(Convert.ToString(entity["displayName"]),
                                                            Convert.ToString(entity["thumbnailUrl"]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occured while fecthing gravatar profile for user {emailAddress}. Exception is {ex}");
            }

            return new Tuple<string, string>("", "");
        }
    }
}
