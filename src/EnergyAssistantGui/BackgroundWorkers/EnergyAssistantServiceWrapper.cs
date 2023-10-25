using UlfenDk.EnergyAssistant;

namespace EnergyAssistant.BackgroundWorkers;

public class EnergyAssistantServiceWrapper : IHostedService
{
    private readonly EnergyAssistantService _service;

    public EnergyAssistantServiceWrapper(EnergyAssistantService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
//        throw new NotImplementedException();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // throw new NotImplementedException();

        return Task.CompletedTask;
    }
}