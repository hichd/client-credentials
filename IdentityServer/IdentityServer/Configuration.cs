using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource> { new ApiResource("ApiOne") };

        public static IEnumerable<Client> GetClients() =>
            new List<Client> { new Client {
                    ClientId = "client_id",
                    ClientSecrets = { new Secret("client_secret".ToSha256()) },

                    // specify how is client going to retrieve access token
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // what resource this access token is used for
                    AllowedScopes = { "ApiOne" }
                }
            };
    }
}
