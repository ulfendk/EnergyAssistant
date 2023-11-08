using UlfenDk.EnergyAssistant.Database;

namespace EnergyAssistant.ViewModels;

public class FeeViewModel
{
    public Fee Data { get; }

    public FeeViewModel(Fee? data)
    {
        Data = data ?? new Fee();
    }

    public string Name
    {
        get => Data.Name;
        set => Data.Name = value;
    }

    public decimal Amount
    {
        get => Data.Amount;
        set => Data.Amount = value;
    }
}