using DispacthBalanceCalled;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DurableClientAttribute = Microsoft.Azure.Functions.Worker.DurableClientAttribute;
using ILogger = Microsoft.Extensions.Logging.ILogger;


namespace DispatchBalance
{

    public class DispatchBalanceFunction
    {
        private readonly HttpClient _httpClient;
        private static readonly ILogger _log = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<DispatchBalanceFunction>();

        public DispatchBalanceFunction(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [Function("GetDispatchBalance")]
        public async Task<HttpResponseData> HttpStart(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "v1/dispatchBalance/{serviceCode}/{ceveCode}/{saleDate}")] HttpRequestData req,
            int ceveCode,
            string saleDate,
            string serviceCode)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);

            try
            {
                var orchestratorInput = new OrchestratorInput
                {
                    ServiceCode = serviceCode,
                    CeveCode = ceveCode,
                    SaleDate = DateOnly.ParseExact(saleDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)
                };

                var serviceCaller = new ServiceCaller();
                List<string> apiResults = await serviceCaller.CallServices(orchestratorInput);
                await response.WriteAsJsonAsync(new { Results = apiResults });
            }
            catch (Exception ex)
            {
                _log.LogError($"An error occurred: {ex.Message}");
                response.StatusCode = HttpStatusCode.InternalServerError;
                await response.WriteStringAsync("An error occurred while processing the request.");
            }

            return response;
        }
    }
}


