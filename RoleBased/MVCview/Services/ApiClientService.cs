using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MVCview.Services
{
    public class ApiClientService( IHttpContextAccessor httpContextAccessor, TokenService tokenService)
    {
        private readonly string _baseUrl = "https://localhost:7050";

        public async Task<(bool Success, T? Data, string Error)> GetAsync<T>(string url)
        {
            return await SendAsync<T>(HttpMethod.Get, url, null);
        }

        public async Task<(bool Success, T? Data, string Error)> PostAsync<T>(string url, object data)
        {
            return await SendAsync<T>(HttpMethod.Post, url, data);
        }

        public async Task<(bool Success, T? Data, string Error)> PutAsync<T>(string url, object data)
        {
            return await SendAsync<T>(HttpMethod.Put, url, data);
        }

        public async Task<(bool Success, T? Data, string Error)> DeleteAsync<T>(string url)
        {
            return await SendAsync<T>(HttpMethod.Delete, url, null);
        }

        public async Task<(bool Success, string Error)> UploadFileAsync(
    string url,
    IFormFile file,
    string formFileName = "file")
        {
            try
            {
                using var client = CreateClient();

                using var form = new MultipartFormDataContent();

                using var stream = file.OpenReadStream();

                var fileContent = new StreamContent(stream);

                fileContent.Headers.ContentType =
                    new MediaTypeHeaderValue(file.ContentType);

                form.Add(fileContent, formFileName, file.FileName);

                var response = await client.PostAsync(
                    $"{_baseUrl}{url}",
                    form);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var refreshed = await tokenService.RefreshAccessToken();

                    if (refreshed)
                    {
                        using var retryClient = CreateClient();

                        using var retryForm = new MultipartFormDataContent();

                        using var retryStream = file.OpenReadStream();

                        var retryFileContent = new StreamContent(retryStream);

                        retryFileContent.Headers.ContentType =
                            new MediaTypeHeaderValue(file.ContentType);

                        retryForm.Add(
                            retryFileContent,
                            formFileName,
                            file.FileName);

                        response = await retryClient.PostAsync(
                            $"{_baseUrl}{url}",
                            retryForm);
                    }
                }

                var responseText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return (false, responseText);
                }

                return (true, "");
            }
            catch (HttpRequestException)
            {
                return (false, "API server is not running. Please start AdoApi2.");
            }
            catch (Exception)
            {
                return (false, "Something went wrong while uploading file.");
            }
        }

        private async Task<(bool Success, T? Data, string Error)> SendAsync<T>(
            HttpMethod method,
            string url,
            object? data)
        {
            try
            {
                using var client = CreateClient();

                var request = new HttpRequestMessage(
                    method,
                    $"{_baseUrl}{url}");

                if (data != null)
                {
                    var json = JsonSerializer.Serialize(data);

                    request.Content = new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json");
                }

                var response = await client.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var refreshed = await tokenService.RefreshAccessToken();

                    if (refreshed)
                    {
                        using var retryClient = CreateClient();

                        var retryRequest = new HttpRequestMessage(
                            method,
                            $"{_baseUrl}{url}");

                        if (data != null)
                        {
                            var json = JsonSerializer.Serialize(data);

                            retryRequest.Content = new StringContent(
                                json,
                                Encoding.UTF8,
                                "application/json");
                        }

                        response = await retryClient.SendAsync(retryRequest);
                    }
                }

                var responseText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return (false, default, responseText);
                }

                if (typeof(T) == typeof(string))
                {
                    return (true, (T)(object)responseText, "");
                }

                var result = JsonSerializer.Deserialize<T>(
                    responseText,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                return (true, result, "");
            }
            catch (HttpRequestException)
            {
                return (false, default, "API server is not running. Please start AdoApi2.");
            }
            catch (Exception)
            {
                return (false, default, "Something went wrong while calling API.");
            }
        }

        private HttpClient CreateClient()
        {
            var client = new HttpClient();

            var token = httpContextAccessor.HttpContext?
                .Session.GetString("token");

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }
    }
}