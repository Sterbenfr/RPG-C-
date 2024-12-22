using RPG_DLL.Entities;
using RPG_DLL.Systems;
using System.Text.Json.Serialization;
using System.Text.Json;

public class EquipementConverter : JsonConverter<Equipement>
{
    public override Equipement Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Désérialiser l'objet JSON complet en un dictionnaire
        var equipementDict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(ref reader, options);

        // Créer un nouvel objet Equipement et initialiser ses propriétés
        var equipement = new Equipement
        {
            Name = equipementDict["Name"].GetString(),
            Type = (ItemType)equipementDict["Type"].GetInt32(),
            Stats = JsonSerializer.Deserialize<Stats>(equipementDict["Stats"].GetRawText(), options),
            Skill = equipementDict.ContainsKey("Skill") ? JsonSerializer.Deserialize<Skill>(equipementDict["Skill"].GetRawText(), options) : null
        };

        return equipement;
    }

    public override void Write(Utf8JsonWriter writer, Equipement value, JsonSerializerOptions options)
    {
        // Implémentez la logique pour sérialiser un Equipement en une chaîne
        writer.WriteStartObject();
        writer.WriteString("Name", value.Name);
        writer.WriteNumber("Type", (int)value.Type);
        writer.WritePropertyName("Stats");
        JsonSerializer.Serialize(writer, value.Stats, options);
        if (value.Skill != null)
        {
            writer.WritePropertyName("Skill");
            JsonSerializer.Serialize(writer, value.Skill, options);
        }
        writer.WriteEndObject();
    }

    public override void WriteAsPropertyName(Utf8JsonWriter writer, Equipement value, JsonSerializerOptions options)
    {
        writer.WritePropertyName(value.Name);
    }

    public override Equipement ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string equipementName = reader.GetString();
        return new Equipement { Name = equipementName };
    }
}
