using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ViteFest.AspNetCore;

internal class ViteHostedService(IViteState state) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        state.Initialize();
        return Task.CompletedTask;
    }
}
