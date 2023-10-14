// Generated using https://app.quicktype.io/

namespace UlfenDk.EnergyAssistant.Nordpool;

#pragma warning disable CS8618
#pragma warning disable CS8601
#pragma warning disable CS8603

using System;

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

public class Welcome
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("data")]
    public Data Data { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("cacheKey")]
    public string CacheKey { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("conf")]
    public Conf Conf { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("header")]
    public Header Header { get; set; }

    [JsonPropertyName("endDate")]
    public object EndDate { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("pageId")]
    public long? PageId { get; set; }
}

public  class Conf
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Id")]
    public Guid? Id { get; set; }

    [JsonPropertyName("Name")]
    public object Name { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Published")]
    public string? Published { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ShowGraph")]
    public bool? ShowGraph { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ResolutionPeriod")]
    public ResolutionPeriod ResolutionPeriod { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ResolutionPeriodY")]
    public ResolutionPeriod ResolutionPeriodY { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Entities")]
    public Entity[] Entities { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TableType")]
    public long? TableType { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ExtraRows")]
    public ExtraRow[] ExtraRows { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Filters")]
    public Filter[] Filters { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("IsDrillDownEnabled")]
    public bool? IsDrillDownEnabled { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DrillDownMode")]
    public long? DrillDownMode { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("IsMinValueEnabled")]
    public bool? IsMinValueEnabled { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("IsMaxValueEnabled")]
    public bool? IsMaxValueEnabled { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ValidYearsBack")]
    public long? ValidYearsBack { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TimeScaleUnit")]
    public string TimeScaleUnit { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("IsNtcEnabled")]
    public bool? IsNtcEnabled { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NtcProductType")]
    public ProductType NtcProductType { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NtcHeader")]
    public string NtcHeader { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ShowTimelineGraph")]
    public long? ShowTimelineGraph { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ExchangeMode")]
    public long? ExchangeMode { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("IsPivotTable")]
    public long? IsPivotTable { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("IsCombinedHeadersEnabled")]
    public long? IsCombinedHeadersEnabled { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("NtcFormat")]
    public long? NtcFormat { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DisplayHourAlsoInUKTime")]
    public bool? DisplayHourAlsoInUkTime { get; set; }
}

public class Entity
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ProductType")]
    public ProductType ProductType { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("SecondaryProductType")]
    public ProductType SecondaryProductType { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("SecondaryProductBehavior")]
    public long? SecondaryProductBehavior { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Id")]
    public Guid? Id { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Name")]
    public Name? Name { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("GroupHeader")]
    public string GroupHeader { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DataUpdated")]
    public string? DataUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Attributes")]
    public Attribute[] Attributes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Drillable")]
    public bool? Drillable { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DateRanges")]
    public object[] DateRanges { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Index")]
    public long? Index { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("IndexForColumn")]
    public long? IndexForColumn { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("MinMaxDisabled")]
    public bool? MinMaxDisabled { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DisableNumberGroupSeparator")]
    public long? DisableNumberGroupSeparator { get; set; }

    [JsonPropertyName("TimeserieID")]
    public object TimeserieId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("SecondaryTimeserieID")]
    public Guid? SecondaryTimeserieId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("HasPreliminary")]
    public bool? HasPreliminary { get; set; }

    [JsonPropertyName("TimeseriePreliminaryID")]
    public object TimeseriePreliminaryId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Scale")]
    public long? Scale { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("SecondaryScale")]
    public long? SecondaryScale { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DataType")]
    public long? DataType { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("SecondaryDataType")]
    public long? SecondaryDataType { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("LastUpdate")]
    public string? LastUpdate { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Unit")]
    public string Unit { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("IsDominatingDirection")]
    public bool? IsDominatingDirection { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DisplayAsSeparatedColumn")]
    public bool? DisplayAsSeparatedColumn { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("EnableInChart")]
    public bool? EnableInChart { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("BlueNegativeValues")]
    public bool? BlueNegativeValues { get; set; }
}

public class Attribute
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Id")]
    public Guid? Id { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Name")]
    public string Name { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("HasRoles")]
    public bool? HasRoles { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Role")]
    public string Role { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Value")]
    public string Value { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Values")]
    public string[] Values { get; set; }
}

public class ProductType
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Id")]
    public Guid? Id { get; set; }

    [JsonPropertyName("Attributes")]
    public Attribute[] Attributes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Name")]
    public string Name { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DisplayName")]
    public string DisplayName { get; set; }
}

public class ExtraRow
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Id")]
    public Guid? Id { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Header")]
    public string Header { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ColumnProducts")]
    public string[] ColumnProducts { get; set; }
}

public class Filter
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Id")]
    public Guid? Id { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AttributeName")]
    public string AttributeName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Values")]
    public string[] Values { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DefaultValue")]
    public string DefaultValue { get; set; }
}

public class ResolutionPeriod
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Id")]
    public Guid? Id { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Resolution")]
    public long? Resolution { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Unit")]
    public long? Unit { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("PeriodNumber")]
    public long? PeriodNumber { get; set; }
}

public class Data
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Rows")]
    public Row[] Rows { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("IsDivided")]
    public bool? IsDivided { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("SectionNames")]
    public string[] SectionNames { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("EntityIDs")]
    public Guid[] EntityIDs { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DataStartdate")]
    public string? DataStartdate { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DataEnddate")]
    public string? DataEnddate { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("MinDateForTimeScale")]
    public string? MinDateForTimeScale { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("AreaChanges")]
    public object[] AreaChanges { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Units")]
    public string[] Units { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("LatestResultDate")]
    public string? LatestResultDate { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ContainsPreliminaryValues")]
    public bool? ContainsPreliminaryValues { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ContainsExchangeRates")]
    public bool? ContainsExchangeRates { get; set; }

    [JsonPropertyName("ExchangeRateOfficial")]
    public object ExchangeRateOfficial { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ExchangeRatePreliminary")]
    public string ExchangeRatePreliminary { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("ExchangeUnit")]
    public string ExchangeUnit { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DateUpdated")]
    public string? DateUpdated { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("CombinedHeadersEnabled")]
    public bool? CombinedHeadersEnabled { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DataType")]
    public long? DataType { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("TimeZoneInformation")]
    public long? TimeZoneInformation { get; set; }
}

public  class Row
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Columns")]
    public Column[] Columns { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Name")]
    public string Name { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("StartTime")]
    public string? StartTime { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("EndTime")]
    public string? EndTime { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DateTimeForData")]
    public string? DateTimeForData { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DayNumber")]
    public long? DayNumber { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("StartTimeDate")]
    public string? StartTimeDate { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("IsExtraRow")]
    public bool? IsExtraRow { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("IsNtcRow")]
    public bool? IsNtcRow { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("EmptyValue")]
    public string EmptyValue { get; set; }

    [JsonPropertyName("Parent")]
    public object Parent { get; set; }
}

public class Column
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Index")]
    public long? Index { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Scale")]
    public long? Scale { get; set; }

    [JsonPropertyName("SecondaryValue")]
    public object SecondaryValue { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("IsDominatingDirection")]
    public bool? IsDominatingDirection { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("IsValid")]
    public bool? IsValid { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("IsAdditionalData")]
    public bool? IsAdditionalData { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Behavior")]
    public long? Behavior { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Name")]
    public Name? Name { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("Value")]
    public string Value { get; set; }

    [JsonPropertyName("GroupHeader")]
    public string GroupHeader { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DisplayNegativeValueInBlue")]
    public bool? DisplayNegativeValueInBlue { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("CombinedName")]
    public Name? CombinedName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DateTimeForData")]
    public string? DateTimeForData { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DisplayName")]
    public string DisplayName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("DisplayNameOrDominatingDirection")]
    public string DisplayNameOrDominatingDirection { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("IsOfficial")]
    public bool? IsOfficial { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("UseDashDisplayStyle")]
    public bool? UseDashDisplayStyle { get; set; }
}

public class Header
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("questionMarkInfo")]
    public string QuestionMarkInfo { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("hideDownloadButton")]
    public string HideDownloadButton { get; set; }
}

public enum Name { Dk1, Dk2 };

internal static class Converter
{
    public static readonly JsonSerializerOptions Settings = new(JsonSerializerDefaults.General)
    {
        Converters =
        {
            NameConverter.Singleton,
            // new DateOnlyConverter(),
            // new TimeOnlyConverter(),
            // IsoDateTimeOffsetConverter.Singleton
        },
    };
}

internal class NameConverter : JsonConverter<Name>
{
    public override bool CanConvert(Type t) => t == typeof(Name);

    public override Name Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        switch (value)
        {
            case "DK1":
                return Name.Dk1;
            case "DK2":
                return Name.Dk2;
        }
        throw new Exception("Cannot unmarshal type Name");
    }

    public override void Write(Utf8JsonWriter writer, Name value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case Name.Dk1:
                JsonSerializer.Serialize(writer, "DK1", options);
                return;
            case Name.Dk2:
                JsonSerializer.Serialize(writer, "DK2", options);
                return;
        }
        throw new Exception("Cannot marshal type Name");
    }

    public static readonly NameConverter Singleton = new NameConverter();
}

#pragma warning restore CS8618
#pragma warning restore CS8601
#pragma warning restore CS8603
