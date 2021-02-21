using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace ApiTwo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            // 1 - retrieve access token
            var serverClient = _httpClientFactory.CreateClient();

            // data from .well-known/open_configuration endpoint
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44385/"); // jwt bearer authority <- identity server url


            var tokenResponse = await serverClient.RequestClientCredentialsTokenAsync( // specific to client_credential
                new ClientCredentialsTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,

                    ClientId = "client_id",
                    ClientSecret = "client_secret",

                    Scope = "ApiOne"
                });// returned token contains access token

            // 2 - retrieve secret data
            var apiClient = _httpClientFactory.CreateClient();

            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = await apiClient.GetAsync("https://localhost:44306/secret");

            var content = await response.Content.ReadAsStringAsync();

            return Ok(new
            {
                access_token = tokenResponse.AccessToken,
                message = content
            });
        }
    }
}
