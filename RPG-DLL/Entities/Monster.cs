using RPG_DLL.Economy;
using RPG_DLL.Entities;
using RPG_DLL.Systems;
using System;
using System.Collections.Generic;
using System.Text.Json;

public class Monster
{
    public Stats MonsterStats { get; set; }
    public LootTable LootTable { get; set; }
    public string Name { get; set; }
    public int Tier { get; set; }
    public string Symbole { get; set; }
    public int ExperienceValue { get; set; } // Nouvelle propriété


    public Monster() { }

    static List<ItemTemplate> LoadItemTemplates()
    {
        string filePath = "C:\\Users\\pierr\\source\\repos\\RPG\\GameEditor\\bin\\Debug\\net8.0\\itemTemplates.json";

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Le fichier itemTemplates.json n'existe pas.");
            return new List<ItemTemplate>();
        }

        string jsonString = File.ReadAllText(filePath);

        if (string.IsNullOrEmpty(jsonString))
        {
            Console.WriteLine("Le fichier itemTemplates.json est vide.");
            return new List<ItemTemplate>();
        }

        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            List<ItemTemplate> itemTemplates = JsonSerializer.Deserialize<List<ItemTemplate>>(jsonString, options);

            if (itemTemplates == null)
            {
                Console.WriteLine("La désérialisation a renvoyé une valeur nulle.");
                return new List<ItemTemplate>();
            }

            return itemTemplates;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la désérialisation : {ex.Message}");
            return new List<ItemTemplate>();
        }
    }


    public void TakeDamage(int damage)
    {
        int netDamage = damage - MonsterStats.Defense;
        if (netDamage > 0)
        {
            MonsterStats.Health -= netDamage;
        }
        else
        {
            // Si la défense est supérieure aux dégâts, les dégâts nets sont mis à zéro
            netDamage = 0;
        }
    }


    public int getDamage()
    {
        return MonsterStats.Damage;
    }

    public static Monster CreateGolem(GolemTemplate template, int level, int RarityBoost)
    {
        // Charger les templates d'items
        List<ItemTemplate> itemTemplates = LoadItemTemplates();

        // Créer une nouvelle liste de possibleDrops et ajouter "MineStone"
        var possibleDrops = new List<Item>
    {
        new MineStone(level, StatTypes.Damage),
        new MineStone(level, StatTypes.Health),
        new MineStone(level, StatTypes.Defense),
        new MineStone(level, StatTypes.LootRarityBonus)
    };

        // Ajouter une chance % d'obtenir une arme supplémentaire des templates
        Random random = new Random();
        if (random.Next(100) < 5 + RarityBoost && itemTemplates.Count > 0)
        {
            var itemTemplatesFiltered = itemTemplates.Where(t => t.Type == ItemType.Weapon || t.Type == ItemType.Armor || t.Type == ItemType.Consumable).ToList();
            if (itemTemplatesFiltered.Count > 0)
            {
                int index = random.Next(itemTemplatesFiltered.Count);
                var selectedTemplate = itemTemplatesFiltered[index];

                if (selectedTemplate != null && !string.IsNullOrEmpty(selectedTemplate.Name))
                {
                    Item extraItem;
                    if (selectedTemplate.Type == ItemType.Weapon || selectedTemplate.Type == ItemType.Armor)
                    {
                        extraItem = new Equipement
                        {
                            Name = selectedTemplate.Name,
                            Type = selectedTemplate.Type,
                            Stats = selectedTemplate.Stats,
                            Skill = selectedTemplate.Skill
                        };
                    }
                    else if (selectedTemplate.Type == ItemType.Consumable)
                    {
                        extraItem = new Item
                        {
                            Name = selectedTemplate.Name,
                            Type = selectedTemplate.Type,
                            Stats = selectedTemplate.Stats,
                            Skill = selectedTemplate.Skill
                        };
                        Console.WriteLine($"Item a drop : {extraItem.Name}, Stats : {extraItem.Stats.Health} santé, {extraItem.Stats.Damage} dégâts, {extraItem.Stats.Defense} défense, {extraItem.Stats.LootRarityBonus} bonus de rareté de butin.");
                        possibleDrops.Add(extraItem);
                    }
                    else
                    {
                        extraItem = new Item
                        {
                            Name = selectedTemplate.Name,
                            Type = selectedTemplate.Type,
                            Stats = selectedTemplate.Stats,
                            Skill = selectedTemplate.Skill
                        };
                    }
                    possibleDrops.Add(extraItem);
                }
            }
        }

        return new Monster
        {
            MonsterStats = new Stats
            {
                Health = template.BaseHealth * level,
                Damage = template.BaseDamage * level,
                Defense = template.BaseDefense * level,
                LootRarityBonus = 0
            },
            LootTable = new LootTable { possibleDrops = possibleDrops },
            Name = $"{template.Type} Golem",
            Tier = level,
            Symbole = template.Type[0].ToString(),
            ExperienceValue = level * 50
        };
    }



}
