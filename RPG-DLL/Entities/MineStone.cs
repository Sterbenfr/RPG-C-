using System;
using RPG_DLL.Systems;

namespace RPG_DLL.Entities
{
    public class MineStone : Item
    {
        // Propriétés de base
        public int Id { get; set; }
        public string Name { get; set; }
        public int Tier { get; set; }
        public int Value { get; set; }
        public StatTypes StatTypes { get; set; }
        public ItemType ItemType { get; set; } = ItemType.Rune;

        private static readonly Random Random = new Random();

        // Constructeur par défaut
        public MineStone()
        {
        }

        // Constructeur avec paramètres
        public MineStone(int tier, StatTypes statTypes)
        {
            Name = statTypes.ToString() + " rune";
            Tier = tier;
            StatTypes = statTypes;
            Value = CalculateValue(tier);
        }

        // Méthode pour afficher les informations de la pierre
        public void DisplayInfo()
        {
            Console.WriteLine(ToString());
        }

        // Méthode pour calculer la valeur de la pierre en fonction de sa dureté
        public int CalculateValue(int tier)
        {
            if (tier <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tier), "Tier must be greater than 0.");
            }
            return Random.Next(1, tier * 10);
        }


        // Méthode pour ajouter la pierre au joueur
        public void AddToPlayer(Player player)
        {
            switch (StatTypes)
            {
                case StatTypes.Health:
                    player.PlayerStats.AddHealth(Value);
                    break;
                case StatTypes.Damage:
                    player.PlayerStats.AddDamage(Value);
                    break;
                case StatTypes.Defense:
                    player.PlayerStats.AddDefense(Value);
                    break;
                case StatTypes.LootRarityBonus:
                    player.PlayerStats.AddLootRarityBonus(Value);
                    break;
            }
            player.Inventory.AddMineStone(this);
        }

        // Méthode ToString pour afficher les informations de la pierre
        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Tier: {Tier}, Value: {Value}, StatType: {StatTypes}";
        }
    }
}
