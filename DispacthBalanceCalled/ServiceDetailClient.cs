using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;


namespace DispatchBalance
{
    public static class ServiceDetailClient
    {
        public static async Task<string> Run(string ServiceCode, int CeveCode, string SaleDateStr, ILogger log)
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
//public class Function1
//{
//    private readonly HttpClient _httpClient;

//    public Function1(HttpClient httpClient)
//    {
//        _httpClient = httpClient;
//    }

//    [Function("HttpStart")]
//    public async Task<HttpResponseData> HttpStart(
//        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "v1/Dispacthbalance/{serviceCode}/{ceveCode}/{saleDate}")] HttpRequestData req,
//        [DurableClient] IDurableOrchestrationClient starter,
//        //"v1/Dispacthbalance/{serviceCode}/{ceveCode}/{saleDate}"
//        int ceveCode,
//        string saleDate,
//        string serviceCode,
//        ILogger log)
//    {
//        var response = req.CreateResponse(HttpStatusCode.OK);
//        // Aquí puedes utilizar los parámetros ceveCode, saleDate y serviceCode según sea necesario
//        string instanceId = await starter.StartNewAsync("DurableFunction1", null);
//        log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

//        // Escribir el objeto JSON en la respuesta
//        await response.WriteAsJsonAsync(new { InstanceId = instanceId });

//        return response;
//    }


//    [Function("DurableFunction1")]
//    public static async Task<List<string>> RunOrchestrator(OrchestratorInput data, ILogger log)
//    {
//        var httpClient = new HttpClient();

//        var parallelTasks = new List<Task<string>>();
//        //Convertir la fecha a string en el formato (yyyy-MM-dd)
//        string formatSaleDate = data.SaleDate.ToString("yyyy-MM-dd");

//        parallelTasks.Add(FunctionServiceDetail.Run(data.ServiceCode, httpClient, log));
//        parallelTasks.Add(FunctionServiceSaleDate.Run(data.ServiceCode, data.CeveCode, formatSaleDate, httpClient, log));
//        parallelTasks.Add(FunctionRouteDistribution.Run(data.ServiceCode, data.CeveCode, data.SaleDate, httpClient, log));
//        parallelTasks.Add(FunctionRouteUnload.Run(data.ServiceCode, data.CeveCode, data.SaleDate, httpClient, log));
//        parallelTasks.Add(FunctionServiceFunctionASN.Run(data.ServiceCode, data.CeveCode, data.SaleDate, httpClient, log));

//        await Task.WhenAll(parallelTasks);

//        List<string> results = new List<string>();
//        foreach (var task in parallelTasks)
//        {
//            results.Add(await task);
//        }

//        log.LogWarning($"All Activity functions completed for orchestration");

//        return results;
//    }
//}

//public static class FunctionServiceDetail
//{
//    public static async Task<string> Run(string ServiceCode, HttpClient httpClient, ILogger log)
//    {
//        string serviceDetailUrl = $"https://localhost:7171/api/ServiceDetail/GetSaleData?ServiceCode={ServiceCode}";
//        HttpResponseMessage response = await httpClient.GetAsync(serviceDetailUrl);
//        if (response.IsSuccessStatusCode)
//        {
//            string result = await response.Content.ReadAsStringAsync();
//            log.LogWarning($"FunctionServiceDetail completed.");
//            return result;
//        }
//        else
//        {
//            log.LogError($"Error calling URL: {serviceDetailUrl}, StatusCode: {response.StatusCode}");
//            return null;
//        }
//    }
//}

//public static class FunctionServiceSaleDate
//{
//    public static async Task<string> Run(string ServiceCode, int CeveCode, string SaleDateStr, HttpClient httpClient, ILogger log)
//    {
//        // Validar el formato de la fecha
//        if (!DateTime.TryParseExact(SaleDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime saleDate))
//        {
//            // Si el formato de la fecha no es válido, registrar un mensaje de advertencia y retornar null
//            log.LogWarning($"Invalid date format for saleDate: {SaleDateStr}. Use yyyy-MM-dd format.");
//            return null;
//        }

//        // Reformatear la fecha a yyyy-MM-dd
//        string formattedSaleDate = saleDate.ToString("yyyy-MM-dd");

//        string saleDateUrl = $"https://localhost:7171/api/ServiceSalesDate/getSaleData?ServiceCode={ServiceCode}&CeveCode={CeveCode}&SaleDate={formattedSaleDate}";
//        HttpResponseMessage response = await httpClient.GetAsync(saleDateUrl);
//        if (response.IsSuccessStatusCode)
//        {
//            string result = await response.Content.ReadAsStringAsync();
//            log.LogWarning($"FunctionServiceSaleDate completed.");
//            return result;
//        }
//        else
//        {
//            log.LogError($"Error calling URL: {saleDateUrl}, StatusCode: {response.StatusCode}");
//            return null;
//        }
//    }
//}
//public static class FunctionRouteDistribution
//{
//    public static async Task<string> Run(string ServiceCode, int CeveCode, DateOnly SaleDate, HttpClient httpClient, ILogger log)
//    {
//        string formattedSaleDate = SaleDate.ToString("yyyy-MM-dd");
//        string routeDistributionUrl = $"https://localhost:7171/api/RouteDistribution/GetRouteDistribution?ServiceCode={ServiceCode}&CeveCode={CeveCode}&SaleDate={formattedSaleDate}";
//        HttpResponseMessage response = await httpClient.GetAsync(routeDistributionUrl);
//        if (response.IsSuccessStatusCode)
//        {
//            string result = await response.Content.ReadAsStringAsync();
//            log.LogWarning($"FunctionRouteDistribution completed.");
//            return result;
//        }
//        else
//        {
//            log.LogError($"Error calling URL: {routeDistributionUrl}, StatusCode: {response.StatusCode}");
//            return null;
//        }
//    }
//}


//public static class FunctionRouteUnload
//{
//    public static async Task<string> Run(string ServiceCode, int CeveCode, DateOnly SaleDate, HttpClient httpClient, ILogger log)
//    {
//        string formattedSaleDate = SaleDate.ToString("yyyy-MM-dd");
//        string routeUnloadUrl = $"https://localhost:7171/api/RouteUnload/GetRouteDistribution?ServiceCode={ServiceCode}&CeveCode={CeveCode}&SaleDate={formattedSaleDate}";
//        HttpResponseMessage response = await httpClient.GetAsync(routeUnloadUrl);
//        if (response.IsSuccessStatusCode)
//        {
//            string result = await response.Content.ReadAsStringAsync();
//            log.LogWarning($"FunctionRouteUnload completed.");
//            return result;
//        }
//        else
//        {
//            log.LogError($"Error calling URL: {routeUnloadUrl}, StatusCode: {response.StatusCode}");
//            return null;
//        }
//    }
//}

//public static class FunctionServiceFunctionASN
//{
//    public static async Task<string> Run(string ServiceCode, int CeveCode, DateOnly SaleDate, HttpClient httpClient, ILogger log)
//    {
//        string formattedSaleDate = SaleDate.ToString("yyyy-MM-dd");
//        string serviceFunctionASNUrl = $"https://localhost:7171/api/ServiceFunction/ServiceFunctionGetASN?ServiceCode={ServiceCode}&CeveCode={CeveCode}&SaleDate={formattedSaleDate}";
//        HttpResponseMessage response = await httpClient.GetAsync(serviceFunctionASNUrl);
//        if (response.IsSuccessStatusCode)
//        {
//            string result = await response.Content.ReadAsStringAsync();
//            log.LogWarning($"FunctionServiceFunctionASN completed.");
//            return result;
//        }
//        else
//        {
//            log.LogError($"Error calling URL: {serviceFunctionASNUrl}, StatusCode: {response.StatusCode}");
//            return null;
//        }
//    }
//}

//public class OrchestratorInput
//{
//    public string ServiceCode { get; set; }
//    public int CeveCode { get; set; }
//    public DateOnly SaleDate { get; set; }
//}
