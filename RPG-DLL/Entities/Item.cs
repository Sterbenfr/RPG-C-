using System;
using RPG_DLL.Systems;

namespace RPG_DLL.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ItemType Type { get; set; } 
        public Stats Stats { get; set; }
        public Skill Skill { get; set; }

        public void Consume(Player player)
        {
            if (Stats == null)
            {
                Console.WriteLine("Stats are null, cannot consume item.");
                return;
            }
            player.ApplyStats(Stats);
            Console.WriteLine($"{Name} a été consommé. Vous avez gagné {Stats.Health} santé, {Stats.Damage} dégâts, {Stats.Defense}, et {Stats.LootRarityBonus} bonus de rareté de butin.");
        }

    }
}
