using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ETLAppInternal.Contracts.Repository;
using ETLAppInternal.Contracts.Services.General;
using ETLAppInternal.Exceptions;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Polly;

namespace ETLAppInternal.Services.Repository
{
    public class GenericRepository : IGenericRepository
    {
        private readonly ISettingsService _settingsService;

        public GenericRepository(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public async Task<T> GetAsync<T>(string uri, string authToken = "")
        {
            try
            {
                HttpClient httpClient = CreateHttpClient(uri);
                string jsonResult = string.Empty;

                var responseMessage = await Policy
                    .Handle<WebException>(ex =>
                    {
                        Crashes.TrackError(ex, new Dictionary<string, string> {
                            { "GetAsync", DateTime.Now.ToString("G") },
                            { $"{ex.GetType().Name}" , ex.Message }
                        });
                        return true;
                    })
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () =>
                    {
                        return await httpClient.GetAsync(uri);
                    });

                if (responseMessage.IsSuccessStatusCode)
                {
                    jsonResult =
                        await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var json = JsonConvert.DeserializeObject<T>(jsonResult);
                    return json;
                } else if (responseMessage.StatusCode == HttpStatusCode.Forbidden ||
                           responseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ServiceAuthenticationException(jsonResult);
                }
                else
                {
                    var ex = new HttpRequestExceptionEx(responseMessage.StatusCode, jsonResult);
                    Crashes.TrackError(ex, new Dictionary<string, string> {
                        { "GetAsync", DateTime.Now.ToString("G") },
                        { responseMessage.StatusCode.ToString(), jsonResult }
                    });
                    throw ex;
                }

                
                

            }
            catch (Exception e)
            {
                Crashes.TrackError(e, new Dictionary<string, string> {
                    { $"{e.GetType().Name}" , e.Message }
                });
                throw;
            }
        }

        public async Task<T> PostAsync<T>(string uri, T data, string authToken = "")
        {
            try
            {
                HttpClient httpClient = CreateHttpClient(uri);

                var content = new StringContent(JsonConvert.SerializeObject(data));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                string jsonResult = string.Empty;

                var responseMessage = await Policy
                    .Handle<WebException>(e =>
                    {
                        Crashes.TrackError(e);
                        return true;
                    })
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await httpClient.PostAsync(uri, content));

                if (responseMessage.IsSuccessStatusCode)
                {
                    jsonResult = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var json = JsonConvert.DeserializeObject<T>(jsonResult);
                    return json;
                }



                if (responseMessage.StatusCode == HttpStatusCode.Forbidden ||
                    responseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ServiceAuthenticationException(jsonResult);
                }

                var ex = new HttpRequestExceptionEx(responseMessage.StatusCode, jsonResult);
                Crashes.TrackError(ex, new Dictionary<string, string> {
                    { "PostAsync", DateTime.Now.ToString("G") },
                    { responseMessage.StatusCode.ToString(), jsonResult }
                });
                throw ex;

            }
            catch (Exception e)
            {
                Crashes.TrackError(e, new Dictionary<string, string> {
                    { $"{e.GetType().Name}" , e.Message }
                });
                throw;
            }
        }

        public async Task<TR> PostAsync<T, TR>(string uri, T data, string authToken = "")
        {
            try
            {
                HttpClient httpClient = CreateHttpClient(uri);

                var content = new StringContent(JsonConvert.SerializeObject(data));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                string jsonResult = string.Empty;

                var responseMessage = await Policy
                    .Handle<WebException>(ex =>
                    {
                        Crashes.TrackError(ex, new Dictionary<string, string> {
                            { "PostAsync", DateTime.Now.ToString("G") },
                            { $"{ex.GetType().Name}" , ex.Message }
                        });
                        return true;
                    })
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await httpClient.PostAsync(uri, content));

                if (responseMessage.IsSuccessStatusCode)
                {
                    jsonResult = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var json = JsonConvert.DeserializeObject<TR>(jsonResult);
                    return json;
                }

                if (responseMessage.StatusCode == HttpStatusCode.Forbidden ||
                    responseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ServiceAuthenticationException(jsonResult);
                }

                throw new HttpRequestExceptionEx(responseMessage.StatusCode, jsonResult);

            }
            catch (Exception e)
            {
                Crashes.TrackError(e, new Dictionary<string, string> {
                    { $"{e.GetType().Name}" , e.Message }
                });
                throw;
            }
        }

        public async Task<T> PutAsync<T>(string uri, T data, string authToken = "")
        {
            try
            {
                HttpClient httpClient = CreateHttpClient(uri);

                var content = new StringContent(JsonConvert.SerializeObject(data));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                string jsonResult = string.Empty;

                var responseMessage = await Policy
                    .Handle<WebException>(ex =>
                    {
                        Crashes.TrackError(ex, new Dictionary<string, string> {
                            { "PutAsync", DateTime.Now.ToString("G") },
                            { $"{ex.GetType().Name}" , ex.Message }
                        });
                        return true;
                    })
                    .WaitAndRetryAsync
                    (
                        5,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    )
                    .ExecuteAsync(async () => await httpClient.PutAsync(uri, content));

                if (responseMessage.IsSuccessStatusCode)
                {
                    jsonResult = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var json = JsonConvert.DeserializeObject<T>(jsonResult);
                    return json;
                }

                if (responseMessage.StatusCode == HttpStatusCode.Forbidden ||
                    responseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ServiceAuthenticationException(jsonResult);
                }

                var err = new HttpRequestExceptionEx(responseMessage.StatusCode, jsonResult);
                Crashes.TrackError(err, new Dictionary<string, string> {
                    { "PutAsync", DateTime.Now.ToString("G") },
                    { responseMessage.StatusCode.ToString(), jsonResult }
                });
                throw err;

            }
            catch (Exception e)
            {
                Crashes.TrackError(e, new Dictionary<string, string> {
                    { $"{e.GetType().Name}" , e.Message }
                });
                throw;
            }
        }

        public async Task DeleteAsync(string uri, string authToken = "")
        {
            HttpClient httpClient = CreateHttpClient(authToken);
            await httpClient.DeleteAsync(uri);
        }

        private HttpClient CreateHttpClient(string authToken)
        {
            const string token = "0DD08291D759";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("auth", token);
            httpClient.DefaultRequestHeaders.Add("userId", _settingsService.UserIdSetting);
            httpClient.DefaultRequestHeaders.Add("userName", _settingsService.UserNameSetting);
            httpClient.Timeout = TimeSpan.FromSeconds(180);

            if (!string.IsNullOrEmpty(authToken))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            }
            return httpClient;
        }


    }
}
