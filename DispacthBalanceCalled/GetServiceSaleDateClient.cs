using Microsoft.Extensions.Logging;
using System.Globalization;
using ILogger = Microsoft.Extensions.Logging.ILogger;


namespace DispatchBalance
{
    public static class GetServiceSaleDateClient
    {

        public static async Task<string> Run(string ServiceCode, int CeveCode, string SaleDateStr, ILogger log)
        {
            // Validar el formato de la fecha
            if (!DateTime.TryParseExact(SaleDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime saleDate))
            {
                // Si el formato de la fecha no es válido, registrar un mensaje de advertencia y retornar null
                log.LogWarning($"Invalid date format for saleDate: {SaleDateStr}. Use yyyy-MM-dd format.");
                return null;
            }

            // Reformatear la fecha a yyyy-MM-dd
            string formattedSaleDate = saleDate.ToString("yyyy-MM-dd");
            using var httpClient = new HttpClient();
            string saleDateUrl = $"https://localhost:7171/api/ServiceSalesDate/getSaleData?ServiceCode={ServiceCode}&CeveCode={CeveCode}&SaleDate={formattedSaleDate}";
            HttpResponseMessage response = await httpClient.GetAsync(saleDateUrl);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                log.LogWarning($"GetServiceSaleDate completed.");
                return result;
            }
            else
            {
                log.LogError($"Error calling URL: {saleDateUrl}, StatusCode: {response.StatusCode}");
                return null;
            }
        }
    }
}
