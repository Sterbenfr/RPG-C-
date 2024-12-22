using RPG_DLL.Economy;
using RPG_DLL.Entities;
using RPG_DLL.Systems;
using System.Text.Json;

public class GameEditor
{
public void AddItemTemplate()
{
    Console.WriteLine("Entrez le nom de l'équipement :");
    string name = Console.ReadLine();

    Console.WriteLine("Entrez le type de l'équipement (1: Weapon, 2: Armor, 3: Consumable) :");
    int itemType = int.Parse(Console.ReadLine());
    ItemType type = (ItemType)(itemType - 1);

    Console.WriteLine("Entrez les statistiques de l'équipement :");
    Console.WriteLine("Dégâts :");
    int damage = int.Parse(Console.ReadLine());
    Console.WriteLine("Défense :");
    int defense = int.Parse(Console.ReadLine());
    Console.WriteLine("Santé :");
    int health = int.Parse(Console.ReadLine());
    Console.WriteLine("Bonus de rareté de loot :");
    int lootRarityBonus = int.Parse(Console.ReadLine());

    var stats = new Stats
    {
        Damage = damage,
        Defense = defense,
        Health = health,
        LootRarityBonus = lootRarityBonus
    };

    Skill skill = null;
    if (type != ItemType.Consumable)
    {
        Console.WriteLine("Entrez le nom de la compétence :");
        string skillName = Console.ReadLine();
        Console.WriteLine("Entrez les dégâts de la compétence :");
        int skillDamage = int.Parse(Console.ReadLine());
        skill = new Skill(skillName, new Stats { Damage = skillDamage }, SkillEffectType.Damage);
    }

    var itemTemplate = new ItemTemplate
    {
        Name = name,
        Type = type,
        Stats = stats,
        Skill = skill
    };

    // Charger les templates d'items existants
    List<ItemTemplate> itemTemplates = LoadItemTemplates();

    // Ajouter le nouveau template
    itemTemplates.Add(itemTemplate);

    // Enregistrer les templates dans le fichier JSON
    SaveItemTemplates(itemTemplates);

    Console.WriteLine("Template d'équipement ajouté avec succès !");
}

    public void ListItemTemplates()
    {
        List<ItemTemplate> itemTemplates = LoadItemTemplates();
        if (itemTemplates.Count == 0)
        {
            Console.WriteLine("Aucun template d'item trouvé.");
            return;
        }

        Console.WriteLine("Liste des templates d'items :");
        for (int i = 0; i < itemTemplates.Count; i++)
        {
            Console.WriteLine($"{i + 1}. Nom: {itemTemplates[i].Name}, Type: {itemTemplates[i].Type}, Dégâts: {itemTemplates[i].Stats.Damage}, Défense: {itemTemplates[i].Stats.Defense}, Santé: {itemTemplates[i].Stats.Health}, Bonus de rareté de loot: {itemTemplates[i].Stats.LootRarityBonus}");
        }
    }

    public void DeleteItemTemplate()
    {
        List<ItemTemplate> itemTemplates = LoadItemTemplates();
        if (itemTemplates.Count == 0)
        {
            Console.WriteLine("Aucun template d'item à supprimer.");
            return;
        }

        ListItemTemplates();
        Console.WriteLine("Entrez le numéro du template d'item à supprimer :");
        int index = int.Parse(Console.ReadLine()) - 1;

        if (index < 0 || index >= itemTemplates.Count)
        {
            Console.WriteLine("Numéro invalide.");
            return;
        }

        itemTemplates.RemoveAt(index);
        SaveItemTemplates(itemTemplates);
        Console.WriteLine("Template d'item supprimé avec succès !");
    }

    private List<ItemTemplate> LoadItemTemplates()
    {
        if (!File.Exists("itemTemplates.json"))
        {
            return new List<ItemTemplate>();
        }

        string jsonString = File.ReadAllText("itemTemplates.json");
        return JsonSerializer.Deserialize<List<ItemTemplate>>(jsonString);
    }

    private void SaveItemTemplates(List<ItemTemplate> itemTemplates)
    {
        string jsonString = JsonSerializer.Serialize(itemTemplates, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("itemTemplates.json", jsonString);
    }

    public void AddGolemTemplate()
    {
        Console.WriteLine("Entrez le type de golem :");
        string type = Console.ReadLine();

        Console.WriteLine("Entrez la santé de base :");
        int baseHealth = int.Parse(Console.ReadLine());

        Console.WriteLine("Entrez les dégâts de base :");
        int baseDamage = int.Parse(Console.ReadLine());

        Console.WriteLine("Entrez la défense de base :");
        int baseDefense = int.Parse(Console.ReadLine());

        var possibleDrops = new List<Item>();

        // Ajouter des MineStones aux possibleDrops
        Console.WriteLine("Combien de runes (MineStones) possibles ?");
        int runeCount = int.Parse(Console.ReadLine());

        for (int i = 0; i < runeCount; i++)
        {
            Console.WriteLine($"Entrez le tier de la rune {i + 1} :");
            int tier = int.Parse(Console.ReadLine());

            Console.WriteLine($"Entrez l'affinité de la rune {i + 1} (0: Health, 1: Damage, 2: Defense, 3: LootRarityBonus) :");
            StatTypes affinity = (StatTypes)int.Parse(Console.ReadLine());

            var rune = new MineStone(tier, affinity);
            possibleDrops.Add(rune);
        }

        var golemTemplate = new GolemTemplate
        {
            Type = type,
            BaseHealth = baseHealth,
            BaseDamage = baseDamage,
            BaseDefense = baseDefense,
        };

        // Charger les modèles existants
        List<GolemTemplate> golemTemplates = LoadGolemTemplates();

        // Ajouter le nouveau modèle
        golemTemplates.Add(golemTemplate);

        // Enregistrer les modèles dans le fichier JSON
        SaveGolemTemplates(golemTemplates);

        Console.WriteLine("Modèle de golem ajouté avec succès !");
    }

    public void ListGolemTemplates()
    {
        List<GolemTemplate> golemTemplates = LoadGolemTemplates();
        if (golemTemplates.Count == 0)
        {
            Console.WriteLine("Aucun modèle de golem trouvé.");
            return;
        }

        Console.WriteLine("Liste des modèles de golems :");
        for (int i = 0; i < golemTemplates.Count; i++)
        {
            Console.WriteLine($"{i + 1}. Type: {golemTemplates[i].Type}, Santé: {golemTemplates[i].BaseHealth}, Dégâts: {golemTemplates[i].BaseDamage}, Défense: {golemTemplates[i].BaseDefense}");
        }
    }

    public void DeleteGolemTemplate()
    {
        List<GolemTemplate> golemTemplates = LoadGolemTemplates();
        if (golemTemplates.Count == 0)
        {
            Console.WriteLine("Aucun modèle de golem à supprimer.");
            return;
        }

        ListGolemTemplates();
        Console.WriteLine("Entrez le numéro du modèle de golem à supprimer :");
        int index = int.Parse(Console.ReadLine()) - 1;

        if (index < 0 || index >= golemTemplates.Count)
        {
            Console.WriteLine("Numéro invalide.");
            return;
        }

        golemTemplates.RemoveAt(index);
        SaveGolemTemplates(golemTemplates);
        Console.WriteLine("Modèle de golem supprimé avec succès !");
    }

    private List<GolemTemplate> LoadGolemTemplates()
    {
        if (!File.Exists("golemTemplates.json"))
        {
            return new List<GolemTemplate>();
        }

        string jsonString = File.ReadAllText("golemTemplates.json");
        var options = new JsonSerializerOptions();
        options.Converters.Add(new MineStoneConverter());
        return JsonSerializer.Deserialize<List<GolemTemplate>>(jsonString, options);
    }

    private void SaveGolemTemplates(List<GolemTemplate> golemTemplates)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        options.Converters.Add(new MineStoneConverter());
        string jsonString = JsonSerializer.Serialize(golemTemplates, options);
        File.WriteAllText("golemTemplates.json", jsonString);
    }

}

class Program
{
    static void Main(string[] args)
    {
        GameEditor editor = new GameEditor();
        while (true)
        {
            Console.WriteLine("1. Ajouter un modèle de golem");
            Console.WriteLine("2. Lister les modèles de golems");
            Console.WriteLine("3. Supprimer un modèle de golem");
            Console.WriteLine("4. Ajouter un template d'équipement");
            Console.WriteLine("5. Lister les templates d'équipement");
            Console.WriteLine("6. Supprimer un template d'équipement");
            Console.WriteLine("7. Quitter");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    editor.AddGolemTemplate();
                    break;
                case "2":
                    editor.ListGolemTemplates();
                    break;
                case "3":
                    editor.DeleteGolemTemplate();
                    break;
                case "4":
                    editor.AddItemTemplate();
                    break;
                case "5":
                    editor.ListItemTemplates();
                    break;
                case "6":
                    editor.DeleteItemTemplate();
                    break;
                case "7":
                    return;
                default:
                    Console.WriteLine("Choix invalide. Veuillez réessayer.");
                    break;
            }
        }
    }
}
