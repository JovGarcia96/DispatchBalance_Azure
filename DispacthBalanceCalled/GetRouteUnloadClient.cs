using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;


namespace DispatchBalance
{
    public static class GetRouteUnloadClient
    {
        public static async Task<string> Run(string ServiceCode, int CeveCode, DateOnly SaleDate, ILogger log)
        {
            using var httpClient = new HttpClient();
            string formattedSaleDate = SaleDate.ToString("yyyy-MM-dd");
            string routeUnloadUrl = $"https://localhost:7171/api/RouteUnload/GetRouteDistribution?ServiceCode={ServiceCode}&CeveCode={CeveCode}&SaleDate={formattedSaleDate}";
            HttpResponseMessage response = await httpClient.GetAsync(routeUnloadUrl);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                log.LogWarning($"FunctionRouteUnload completed.");
                return result;
            }
            else
            {
                log.LogError($"Error calling URL: {routeUnloadUrl}, StatusCode: {response.StatusCode}");
                return null;
            }
        }
    }
}
//variables de entorno y despues solo configurarlo.