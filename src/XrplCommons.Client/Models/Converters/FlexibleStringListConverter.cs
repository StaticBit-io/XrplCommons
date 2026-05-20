using System.Text.Json;
using System.Text.Json.Serialization;

namespace XrplCommons.Client.Models.Converters;

/// <summary>
/// Handles JSON fields that can be: a string, an array of strings, an array with nested arrays, or null.
/// Flattens everything into a List&lt;string&gt;.
/// </summary>
internal sealed class FlexibleStringListConverter : JsonConverter<List<string>>
{
    public override List<string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        List<string> result = [];

        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return result;

            case JsonTokenType.String:
                string? value = reader.GetString();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    result.Add(value);
                }
                return result;

            case JsonTokenType.StartArray:
                ReadArray(ref reader, result);
                return result;

            default:
                reader.Skip();
                return result;
        }
    }

    private static void ReadArray(ref Utf8JsonReader reader, List<string> result)
    {
        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.EndArray:
                    return;

                case JsonTokenType.String:
                    string? item = reader.GetString();
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        result.Add(item);
                    }
                    break;

                case JsonTokenType.StartArray:
                    ReadArray(ref reader, result);
                    break;

                default:
                    reader.Skip();
                    break;
            }
        }
    }

    public override void Write(Utf8JsonWriter writer, List<string> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (string item in value)
        {
            writer.WriteStringValue(item);
        }
        writer.WriteEndArray();
    }
}
