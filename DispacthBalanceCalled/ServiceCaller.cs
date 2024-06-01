using DispacthBalanceCalled;
using DispatchBalance;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILogger = Microsoft.Extensions.Logging.ILogger;
public class ServiceCaller
{
    private static readonly ILogger _log = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<DispatchBalanceFunction>();
    public async Task<List<string>> CallServices(OrchestratorInput input)
    {
        var parallelTasks = new List<Task<string>>();
        string formatSaleDate = input.SaleDate.ToString("yyyy-MM-dd");

        parallelTasks.Add(GetRouteDistributionClient.Run(input.ServiceCode, input.CeveCode, input.SaleDate, _log));
        parallelTasks.Add(GetServiceSaleDateClient.Run(input.ServiceCode, input.CeveCode, formatSaleDate, _log));
        parallelTasks.Add(GetServiceDetailClient.Run(input.ServiceCode, input.CeveCode, input.SaleDate, _log));
        parallelTasks.Add(GetRouteUnloadClient.Run(input.ServiceCode, input.CeveCode, input.SaleDate, _log));
        parallelTasks.Add(GetServiceFunctionAsnClient.Run(input.ServiceCode, input.CeveCode, input.SaleDate, _log));

        await Task.WhenAll(parallelTasks);

        List<string> results = new List<string>();
        foreach (var task in parallelTasks)
        {
            results.Add(await task);
        }

        _log.LogWarning($"All Activity functions completed for orchestration");

        return results;
    }
}

