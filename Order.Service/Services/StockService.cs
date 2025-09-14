using Polly;
using System.Net.Http.Json;
using System.Text.Json;

namespace Order.Service.Services
{

    public record CheckStockResponse(bool Status);

    public class StockService(HttpClient client)
    {
        public async Task<bool> CheckStockAsync(int productId, int quantity)
        {
            // Define a fallback policy to return a default response in case of failure
            // This ensures that if the stock service is down, we can still proceed with a default assumption
            // Here, we assume that if the stock service is unreachable, we consider the stock to be available (true)
            // This is a business decision and can be adjusted based on requirements
            // Kullanıcıya stok durumu hakkında bilgi verilebilir veya farklı bir işlem yapılabilir. Kullanıcıya "Stok durumu şu anda kontrol edilemiyor, lütfen daha sonra tekrar deneyin" gibi bir mesaj gösterilebilir.
            // Ancak burada basitlik adına true döndürüyoruz. Ayrıca bu fallback politikası loglama için de kullanılabilir. Burada loglama yaparak stok servisi hatalarının izlenmesini sağlayabiliriz.
            // Örneğin, bir log servisine hata bilgisi gönderilebilir veya bir monitoring aracı ile entegre edilebilir.
            var fallbackPolicy = Policy<HttpResponseMessage>
                .Handle<Exception>()
                .FallbackAsync(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonSerializer.Serialize(new CheckStockResponse(true)))
                });

            var response = await fallbackPolicy.ExecuteAsync(() =>  client.GetAsync($"/api/stock/{productId}/{quantity}"));

            

            if (!response.IsSuccessStatusCode)
            {

                //Logging
                return false;
            }

            var content = await response.Content.ReadFromJsonAsync<CheckStockResponse>();

            return content!.Status;

        }

    }
}
