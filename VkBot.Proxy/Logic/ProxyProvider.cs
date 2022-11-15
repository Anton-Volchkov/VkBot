using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using VkBot.Proxy.Models;

namespace VkBot.Proxy.Logic
{
    public class ProxyProvider
    {
        private readonly string EndPoint = "http://pubproxy.com/api/proxy";
        public async Task<string> GetRandomProxy()
        {
            var response = await EndPoint.AllowAnyHttpStatus().GetAsync();

            if(!response.IsSuccessStatusCode)
            {
                return "";
            }

            var proxy = JsonConvert.DeserializeObject<ProxyModel>(await response.Content.ReadAsStringAsync());

            return proxy.Data.First().IpPort;
        }
    }
}
