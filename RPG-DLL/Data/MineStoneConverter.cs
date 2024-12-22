using System.Text.Json;
using System.Text.Json.Serialization;
using RPG_DLL.Entities;
using RPG_DLL.Systems;

public class MineStoneConverter : JsonConverter<MineStone>
{
    public override MineStone Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        int id = 0;
        string name = null;
        int tier = 0;
        int value = 0;
        StatTypes statTypes = StatTypes.Health;
        ItemType itemType = ItemType.Rune;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                string propertyName = reader.GetString();
                reader.Read();

                switch (propertyName)
                {
                    case "Id":
                        id = reader.GetInt32();
                        break;
                    case "Name":
                        name = reader.GetString();
                        break;
                    case "Tier":
                        tier = reader.GetInt32();
                        break;
                    case "Value":
                        value = reader.GetInt32();
                        break;
                    case "StatTypes":
                        statTypes = (StatTypes)reader.GetInt32();
                        break;
                    case "ItemType":
                        itemType = (ItemType)reader.GetInt32();
                        break;
                }
            }
        }

        var mineStone = new MineStone(tier, statTypes)
        {
            Id = id,
            Name = name,
            Value = value,
            ItemType = itemType
        };

        return mineStone;
    }

    public override void Write(Utf8JsonWriter writer, MineStone value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("Id", value.Id);
        writer.WriteString("Name", value.Name);
        writer.WriteNumber("Tier", value.Tier);
        writer.WriteNumber("Value", value.Value);
        writer.WriteNumber("StatTypes", (int)value.StatTypes);
        writer.WriteNumber("ItemType", (int)value.ItemType);
        writer.WriteEndObject();
    }

    public override MineStone ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString();
        return new MineStone { Name = value };
    }

    public override void WriteAsPropertyName(Utf8JsonWriter writer, MineStone value, JsonSerializerOptions options)
    {
        writer.WritePropertyName(value.Name);
    }
}
