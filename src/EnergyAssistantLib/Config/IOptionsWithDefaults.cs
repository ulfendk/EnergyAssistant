namespace UlfenDk.EnergyAssistant.Config;

public interface IOptionsWithDefaults<T>
    where T : new()
{
    T Default { get; }
}