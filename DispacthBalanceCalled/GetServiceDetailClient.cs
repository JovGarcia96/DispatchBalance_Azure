using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;


namespace DispatchBalance
{
    public static class GetServiceDetailClient
    {
        public static async Task<string> Run(string ServiceCode, int CeveCode, DateOnly SaleDateStr, ILogger log)
        {
            using var httpClient = new HttpClient();
            string serviceDetailUrl = $"https://localhost:7171/api/ServiceDetail/GetSaleData?ServiceCode={ServiceCode}&CeveCode={CeveCode}&SaleDate={SaleDateStr}";
            HttpResponseMessage response = await httpClient.GetAsync(serviceDetailUrl);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                log.LogWarning($"FunctionServiceDetail completed.");
                return result;
            }
            else
            {
                log.LogError($"Error calling URL: {serviceDetailUrl}, StatusCode: {response.StatusCode}");
                return null;
            }
        }
    }
}
