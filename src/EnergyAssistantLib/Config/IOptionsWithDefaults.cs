using YamlDotNet.Serialization;

namespace UlfenDk.EnergyAssistant.Config;

public interface IOptionsWithDefaults<T>
    where T : new()
{
    [YamlIgnore]
    T Default { get; }
}