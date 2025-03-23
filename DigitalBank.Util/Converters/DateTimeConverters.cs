using System.Text.Json;
using System.Text.Json.Serialization;

namespace DigitalBank.Util.Converters;

public class DateTimeConverter : JsonConverter<DateTime>
{
    private readonly string _format = "dd/MM/yyyy HH:mm";

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => DateTime.Parse(reader.GetString() ?? "");

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(_format));
}

public class NullableDateTimeConverter : JsonConverter<DateTime?>
{
    private readonly string _format = "dd/MM/yyyy HH:mm";

    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => DateTime.TryParse(reader.GetString(), out var date) ? date : null;

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        => writer.WriteStringValue(value?.ToString(_format));
}
