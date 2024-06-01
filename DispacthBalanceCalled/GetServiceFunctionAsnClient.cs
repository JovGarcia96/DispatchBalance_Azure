using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;


namespace DispatchBalance
{
    public static class GetServiceFunctionAsnClient
    {
        public static async Task<string> Run(string ServiceCode, int CeveCode, DateOnly SaleDate, ILogger log)
        {

            using var httpClient = new HttpClient();
            string formattedSaleDate = SaleDate.ToString("yyyy-MM-dd");
            string serviceFunctionASNUrl = $"https://localhost:7171/api/ServiceFunction/ServiceFunctionGetASN?ServiceCode={ServiceCode}&CeveCode={CeveCode}&SaleDate={formattedSaleDate}";
            HttpResponseMessage response = await httpClient.GetAsync(serviceFunctionASNUrl);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                log.LogWarning($"GetServiceFunctionASN completed.");
                return result;
            }
            else
            {
                log.LogError($"Error calling URL: {serviceFunctionASNUrl}, StatusCode: {response.StatusCode}");
                return null;
            }
        }
    }
}
