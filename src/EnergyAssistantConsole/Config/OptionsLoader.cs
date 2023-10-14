using System.Globalization;
using System.Text.Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.TypeInspectors;

namespace UlfenDk.EnergyAssistant.Config;

public static class OptionsLoader
{
    public static void WriteOptions(string fileName, Options options)
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTypeInspector
            (
                inner => inner,
                s => s.InsteadOf<YamlAttributesTypeInspector>()
            )
            .WithTypeInspector
            (
                inner => new YamlAttributesTypeInspector(inner),
                s => s.Before<NamingConventionTypeInspector>()
            )
            .WithIndentedSequences()
            .WithTypeConverter(new DateOnlyConverter(CultureInfo.InvariantCulture, false, "yyyy-MM-dd"))
            .WithTypeConverter(new TimeOnlyConverter())
            .Build();
        var serialized = serializer.Serialize(options);
        File.WriteAllText(fileName, serialized);
    }

    public static Options? GetOptions(string fileName)
    {
        var serializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTypeInspector
            (
                inner => inner,
                s => s.InsteadOf<YamlAttributesTypeInspector>()
            )
            .WithTypeInspector
            (
                inner => new YamlAttributesTypeInspector(inner),
                s => s.Before<NamingConventionTypeInspector>()
            )
            .WithTypeConverter(new DateOnlyConverter(CultureInfo.InvariantCulture, false, "yyyy-MM-dd"))
            .WithTypeConverter(new TimeOnlyConverter())
            .Build();

        string optionsJson = File.ReadAllText(fileName);

        return serializer.Deserialize<Options>(optionsJson);
        // return JsonSerializer.Deserialize<Options>(optionsJson, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase,   });
    }
}