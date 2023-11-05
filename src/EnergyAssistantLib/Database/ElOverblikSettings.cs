namespace UlfenDk.EnergyAssistant.Database;

public class ElOverblikSettings
{
    public long Id { get; set; }

    public bool IsEnabled { get; set; }

    public string MeteringPointId { get; set; }

    public string ElectricHeatingMeteringPointId { get; set; }

    public string ApiToken { get; set; }
}