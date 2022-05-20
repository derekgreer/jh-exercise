using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JHExercise.Specs.Infrastructure.Clients
{
    public abstract class WebServiceClientBase
    {
        protected abstract HttpClient GetClient();
        protected abstract string GetBearerTokenHeader();

        public async Task<WebServiceClientResponse<TResponse>> GetRequest<TResponse>(string uri)
        {
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
                AddBearerTokenHeader(requestMessage);

                var httpResponse = await GetClient().SendAsync(requestMessage);
                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<TResponse>(responseContent,
                    new JsonSerializerSettings
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects
                    });

                return new WebServiceClientResponse<TResponse>
                {
                    HttpStatusCode = httpResponse.StatusCode,
                    Response = response
                };
            }
            catch (Exception e)
            {
                return new WebServiceClientResponse<TResponse>
                {
                    Exception = e
                };
            }
        }

        void AddBearerTokenHeader(HttpRequestMessage requestMessage)
        {
            var token = GetBearerTokenHeader();

            if (!string.IsNullOrEmpty(token))
                requestMessage.Headers.Add("Authorization", $"Bearer {token}");
        }

        public WebServiceClientResponse PostRequest(string uri, object request)
        {
            return PostRequest<string>(uri, request).Result;
        }

        public async Task<WebServiceClientResponse<TResponse>> PostRequest<TResponse>(string uri, object request)
        {
            return await Call<TResponse>(HttpMethod.Post, uri, request);
        }

        public WebServiceClientResponse PutRequest(string uri, object request)
        {
            return PutRequest<string>(uri, request).Result;
        }

        public async Task<WebServiceClientResponse<TResponse>> PutRequest<TResponse>(string uri, object request)
        {
            return await Call<TResponse>(HttpMethod.Put, uri, request);
        }

        public WebServiceClientResponse PatchRequest(string uri, object request)
        {
            return PatchRequest<string>(uri, request).Result;
        }

        public async Task<WebServiceClientResponse<TResponse>> PatchRequest<TResponse>(string uri, object request)
        {
            return await Call<TResponse>(new HttpMethod("PATCH"), uri, request);
        }

        public WebServiceClientResponse DeleteRequest(string uri, object request)
        {
            return DeleteRequest<string>(uri, request).Result;
        }

        public async Task<WebServiceClientResponse<TResponse>> DeleteRequest<TResponse>(string uri, object request)
        {
            return await Call<TResponse>(HttpMethod.Delete, uri, request);
        }

        public async Task<WebServiceClientResponse<TResponse>> Call<TResponse>(HttpMethod method, string uri,
            object request)
        {
            try
            {
                var serializedRequest = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(request));
                Console.WriteLine("Request:" + serializedRequest);
                HttpContent requestContent = new StringContent(serializedRequest);
                requestContent.Headers.Remove("Content-Type");
                requestContent.Headers.TryAddWithoutValidation("Content-Type", "application/json");
                var requestMessage = new HttpRequestMessage(method, uri) {Content = requestContent};
                AddBearerTokenHeader(requestMessage);

                var httpResponse = await GetClient().SendAsync(requestMessage, CancellationToken.None);

                Console.WriteLine("HttpResponse Status Code: " + httpResponse.StatusCode);
                var responseContent = await httpResponse.Content.ReadAsStringAsync();

                Console.WriteLine("Response: " + responseContent);

                var settings = new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                };

                var response = JsonConvert.DeserializeObject<TResponse>(responseContent, settings);

                return new WebServiceClientResponse<TResponse>
                {
                    HttpStatusCode = httpResponse.StatusCode,
                    Response = response
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}