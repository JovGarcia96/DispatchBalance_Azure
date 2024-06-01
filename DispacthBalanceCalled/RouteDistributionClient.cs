using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;


namespace DispatchBalance
{
    public static class RouteDistributionClient
    {
        public static async Task<string> Run(string ServiceCode, int CeveCode, DateOnly SaleDate, ILogger log)
        {
            using var httpClient = new HttpClient();
            string formattedSaleDate = SaleDate.ToString("yyyy-MM-dd");
            string routeDistributionUrl = $"https://localhost:7171/api/RouteDistribution/GetRouteDistribution?ServiceCode={ServiceCode}&CeveCode={CeveCode}&SaleDate={formattedSaleDate}";
            HttpResponseMessage response = await httpClient.GetAsync(routeDistributionUrl);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                log.LogWarning($"FunctionRouteDistribution completed.");
                return result;
            }
            else
            {
                log.LogError($"Error calling URL: {routeDistributionUrl}, StatusCode: {response.StatusCode}");
                return null;
            }
        }
    }
}
