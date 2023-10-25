using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Options;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.TypeInspectors;

namespace UlfenDk.EnergyAssistant.Config;

public class OptionsLoader<T>
    where T : IOptionsWithDefaults<T>, new()
{
    private readonly Lazy<ISerializer> _serializer;
    private readonly Lazy<IDeserializer> _deserializer;

    private readonly string _fileName;
    
    public OptionsLoader(IOptions<OptionsFileOptions<T>> options)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));
        _fileName = options.Value.FileName;
        
        _serializer = new Lazy<ISerializer>(() => new SerializerBuilder()
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
            .Build());

        _deserializer = new Lazy<IDeserializer>(() => new DeserializerBuilder()
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
            .Build());
    }

    public T Load()
    {
        if (!File.Exists(_fileName))
        {
            Save(new T().Default);
        }

        string yaml = File.ReadAllText(_fileName);
        
        var options = _deserializer.Value.Deserialize<T>(yaml);

        return options;
    }

    public void Save(T options)
    {
        var yaml = _serializer.Value.Serialize(options);
        File.WriteAllText(_fileName, yaml);
    }
}

// public static class OptionsLoader
// {
//     public static void WriteOptions(string fileName, Options options)
//     {
//         var serializer = new SerializerBuilder()
//             .WithNamingConvention(CamelCaseNamingConvention.Instance)
//             .WithTypeInspector
//             (
//                 inner => inner,
//                 s => s.InsteadOf<YamlAttributesTypeInspector>()
//             )
//             .WithTypeInspector
//             (
//                 inner => new YamlAttributesTypeInspector(inner),
//                 s => s.Before<NamingConventionTypeInspector>()
//             )
//             .WithIndentedSequences()
//             .WithTypeConverter(new DateOnlyConverter(CultureInfo.InvariantCulture, false, "yyyy-MM-dd"))
//             .WithTypeConverter(new TimeOnlyConverter())
//             .Build();
//         var serialized = serializer.Serialize(options);
//         File.WriteAllText(fileName, serialized);
//     }
//
//     public static Options? GetOptions(string fileName)
//     {
//         var serializer = new DeserializerBuilder()
//             .WithNamingConvention(CamelCaseNamingConvention.Instance)
//             .WithTypeInspector
//             (
//                 inner => inner,
//                 s => s.InsteadOf<YamlAttributesTypeInspector>()
//             )
//             .WithTypeInspector
//             (
//                 inner => new YamlAttributesTypeInspector(inner),
//                 s => s.Before<NamingConventionTypeInspector>()
//             )
//             .WithTypeConverter(new DateOnlyConverter(CultureInfo.InvariantCulture, false, "yyyy-MM-dd"))
//             .WithTypeConverter(new TimeOnlyConverter())
//             .Build();
//
//         string optionsJson = File.ReadAllText(fileName);
//
//         return serializer.Deserialize<Options>(optionsJson);
//         // return JsonSerializer.Deserialize<Options>(optionsJson, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase,   });
//     }
// }