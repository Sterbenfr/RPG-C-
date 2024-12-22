using System;

namespace RPG_DLL.Systems
{
    // Déclaration de l'énumération en dehors de la classe Stats
    public enum StatTypes
    {
        Health,
        Damage,
        Defense,
        LootRarityBonus
    }

    public class Stats
    {
        public int Id { get; set; } // Primary key
        public int Health { get; set; }
        public int Damage { get; set; }
        public int Defense { get; set; }
        public int LootRarityBonus { get; set; }

        public Stats() { }

        public Stats(int health, int damage, int defense, int lootRarityBonus)
        {
            Health = health;
            Damage = damage;
            Defense = defense;
            LootRarityBonus = lootRarityBonus;
        }

        public void AddStats(Stats stats)
        {
            if (stats == null)
            {
                throw new ArgumentNullException(nameof(stats), "Stats cannot be null");
            }

            Health += stats.Health;
            Damage += stats.Damage;
            Defense += stats.Defense;
            LootRarityBonus += stats.LootRarityBonus;
        }


        public void RemoveStats(Stats stats)
        {
            Health -= stats.Health;
            Damage -= stats.Damage;
            Defense -= stats.Defense;
            LootRarityBonus -= stats.LootRarityBonus;
        }

        public void AddHealth(int health)
        {
            Health += health;
        }
        public void RemoveHealth(int health)
        {
            Health -= health;
        }
        public void AddDamage(int damage)
        {
            Damage += damage;
        }
        public void RemoveDamage(int damage)
        {
            Damage -= damage;
        }
        public void AddDefense(int defense)
        {
            Defense += defense;
        }
        public void RemoveDefense(int defense)
        {
            Defense -= defense;
        }
        public void AddLootRarityBonus(int lootRarityBonus)
        {
            LootRarityBonus += lootRarityBonus;
        }
        public void RemoveLootRarityBonus(int lootRarityBonus)
        {
            LootRarityBonus -= lootRarityBonus;
        }
    }
}
