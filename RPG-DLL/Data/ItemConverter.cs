using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using RPG_DLL.Entities;

public class ItemConverter : JsonConverter<Item>
{
    public override Item Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Implement the logic to deserialize an Item from a string
        string itemName = reader.GetString();
        return new Item { Name = itemName };
    }

    public override void Write(Utf8JsonWriter writer, Item value, JsonSerializerOptions options)
    {
        // Implement the logic to serialize an Item to a string
        writer.WriteStringValue(value.Name);
    }

    public override void WriteAsPropertyName(Utf8JsonWriter writer, Item value, JsonSerializerOptions options)
    {
        writer.WritePropertyName(value.Name);
    }

    public override Item ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string itemName = reader.GetString();
        return new Item { Name = itemName };
    }
}
