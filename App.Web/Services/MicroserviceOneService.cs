using Duende.IdentityModel.Client;

namespace App.Web.Services
{

    public class MicroserviceOneService(HttpClient client, IConfiguration configuration)
    {

        public async Task<ExchangeResponse> GetExchange()
        {

            var clientId = configuration.GetSection("Client")["Id"]!;

            var clientSecret = configuration.GetSection("Client")["Secret"]!;

            var Authority = configuration.GetSection("Client")["Authority"]!;

            var discoveryResult = await  client.GetDiscoveryDocumentAsync($"{Authority}/.well-known/openid-configuration");


            if (discoveryResult.IsError)
            {
                // logging
                // reuturn model
            }


            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryResult.TokenEndpoint,
                ClientId = clientId,
                ClientSecret = clientSecret
               
            });

            if (tokenResponse.IsError)
            {
                // logging
                // reuturn model
            }

            



            client.SetBearerToken(tokenResponse.AccessToken!);



            var response = await client.GetAsync("/exchange");

            if (response.IsSuccessStatusCode)
            {
            }


            var responseContent = await  response.Content.ReadFromJsonAsync<ExchangeResponse>();

            return responseContent!;

        }
    }
    

}
