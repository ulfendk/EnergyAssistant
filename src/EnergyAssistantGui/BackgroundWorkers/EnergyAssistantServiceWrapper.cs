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
        _service.Start();

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _service.StopAsync(cancellationToken);
    }
}