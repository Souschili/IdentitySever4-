using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BankOfDotNet.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources =>
            new List<ApiResource>
            {
                new ApiResource("BankOfDotNetApi","Customer API of BankOfDotNet")
            };


        public static IEnumerable<Client> GetClients =>
            new List<Client>
            {
                new Client
                {
                    ClientId="client",
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    ClientSecrets=
                    {
                        new Secret("secret".Sha256()),
                    },
                    AllowedScopes= { "BankOfDotNetApi" }
                }
            };


    }
}
