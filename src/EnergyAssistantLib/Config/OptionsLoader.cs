using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.TypeInspectors;

namespace UlfenDk.EnergyAssistant.Config;

public class CircularTypeInspector<T> : TypeInspectorSkeleton
{
    private readonly ITypeInspector _innerTypeDescriptor;

    public CircularTypeInspector(ITypeInspector innerTypeDescriptor)
    {
        _innerTypeDescriptor = innerTypeDescriptor;
    }

    public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container)
    {
        var props = _innerTypeDescriptor.GetProperties(type, container)
            .Where(p => p.Type != typeof(T));
        
        return props;
    }
}

public class OptionsLoader<T>
    where T : IOptionsWithDefaults<T>, new()
{
    private readonly ILogger<OptionsLoader<T>> _logger;
    private readonly Lazy<ISerializer> _serializer;
    private readonly Lazy<IDeserializer> _deserializer;

    private readonly string _fileName;
    
    public OptionsLoader(IOptions<OptionsFileOptions<T>> options, ILogger<OptionsLoader<T>> logger)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
            .WithTypeInspector(inspector => new CircularTypeInspector<T>(inspector))
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
            _logger.LogInformation("Saving default configuration to {fileName}", _fileName);
            Save(new T().Default);
        }

        string yaml = File.ReadAllText(_fileName);
        
        var options = _deserializer.Value.Deserialize<T>(yaml);

        return options;
    }

    public void Save(T options)
    {
        _logger.LogInformation("Saving configuration to {fileName}", _fileName);

        var yaml = _serializer.Value.Serialize(options);
        File.WriteAllText(_fileName, yaml);
    }
}