using Newtonsoft.Json;
using Pivotal.Discovery.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PeopleUI.Services
{
    public abstract class BaseDiscoveryService
    {

        protected DiscoveryHttpClientHandler _handler;

        public BaseDiscoveryService(IDiscoveryClient client)
        {
            _handler = new DiscoveryHttpClientHandler(client);
        }

        public virtual async Task<T> Invoke<T>(HttpRequestMessage request, object content = null)
        {
            var client = GetClient();
            try
            {
                if (content != null)
                {
                    request.Content = Serialize(content);
                }
                using (HttpResponseMessage response = await client.SendAsync(request))
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    return Deserialize<T>(stream);
                }
            }
            catch (Exception e)
            {
                //log error
            }

            return default(T);
        }

        public virtual T Deserialize<T>(Stream stream)
        {
            using (JsonReader reader = new JsonTextReader(new StreamReader(stream)))
            {
                JsonSerializer serializer = new JsonSerializer();
                return (T)serializer.Deserialize(reader, typeof(T));
            }

        }

        public virtual HttpContent Serialize(object toSerialize)
        {
            string json = JsonConvert.SerializeObject(toSerialize);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public virtual HttpClient GetClient()
        {
            var client = new HttpClient(_handler, false);
            return client;
        }
    }
}
