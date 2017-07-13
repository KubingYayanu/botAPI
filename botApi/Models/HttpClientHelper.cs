using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;

namespace botApi.Models
{
    public class HttpClientHelper
    {
        /// Http Get
        /// <summary>
        /// Http Get
        /// </summary>
        /// <param name="client">HttpClient</param>
        /// <param name="url">URL</param>
        /// <returns></returns>
        public static async Task<string> GetResult(HttpClient client, string url)
        {
            var response = await client.GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var body = response.Content.ReadAsStringAsync().Result;
            return body;
        }

        /// Http Post
        /// <summary>
        /// Http Post 
        /// </summary>
        /// <param name="type">JSON;XML;OTHER,Need HttpContent</param>
        /// <param name="url">URL</param>
        /// <param name="client">HttpClient</param>
        /// <param name="requestBody">RequestBodyContent</param>
        /// <param name="httpContent">ContentType...etc.</param>
        /// <returns></returns>
        public async static Task<string> PostResult(string type,
                                                    string url,
                                                    HttpClient client,
                                                    string requestBody, HttpContent httpContent = null)
        {
            HttpResponseMessage response;
            if (type.ToUpper() == "JSON")
            {
                response = await client.PostAsJsonAsync(url, requestBody);
            }
            else if (type.ToUpper() == "XML")
            {
                response = await client.PostAsJsonAsync(url, requestBody);
            }
            else
            {
                if (httpContent == null)
                {
                    throw new ArgumentException();
                }
                response = await client.PostAsync(url, httpContent);
            }

            response.EnsureSuccessStatusCode();
            var body = response.Content.ReadAsStringAsync().Result;
            return body;
        }
    }
}