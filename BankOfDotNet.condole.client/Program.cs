using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BankOfDotNet.condole.client
{
    class Program
    {
        public static void Main(string[] args) => Main().GetAwaiter().GetResult();

        private static async Task Main()
        {
            var client = HttpClientFactory.Create(); // фабрика подключений оптимальней 
            //var сlient = new HttpClient();         // можно , но лучше не надо в нормальном приложении

            // получаем все метаданные от сервера
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");

            // если была ошибка
            if (disco.IsError)
            {
                System.Console.WriteLine(disco.Error);
                return;
            }

            // созданм запрос данными на получение токена
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "secret",
                Scope = "BankOfDotNetApi"
            });

            // если возникла ошибка выводим
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            // выводим полученый токен
            Console.WriteLine(tokenResponse.Json);


            // вызываем апи и передаем токен в защишеный энд-поинт
            var apiClient = HttpClientFactory.Create();

            apiClient.SetToken("Bearer", tokenResponse.AccessToken); // вписываем токем в хедер

            var response = await apiClient.GetAsync("http://localhost:7158/api/customers");

            // постим новый обьект в защишеный апи 
            //object content = new { id = 1, FirstName = "Xaos", LastName = "Sauron" };
            //var customerAdd = await apiClient.PostAsJsonAsync("http://localhost:7158/api/customers", content);

            // если статус код 400 и тд. то выводим его 
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                // получаем данные парсим и выводим 
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }


            Console.ReadLine();

        }


    }
}
