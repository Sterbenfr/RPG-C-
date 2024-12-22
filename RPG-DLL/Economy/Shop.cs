using System;
using System.Collections.Generic;
using System.Linq;
using RPG_DLL.Entities;

namespace RPG_DLL.Economy
{
    public class Shop
    {
        private List<Item> _items;

        public Shop(List<ItemTemplate> itemTemplates)
        {
            _items = new List<Item>();

            foreach (var template in itemTemplates)
            {
                Item item;
                if (template.Type == ItemType.Weapon || template.Type == ItemType.Armor)
                {
                    item = new Equipement
                    {
                        Name = template.Name,
                        Type = template.Type,
                        Stats = template.Stats,
                        Skill = template.Skill,
                    };
                }
                else
                {
                    item = new Item
                    {
                        Name = template.Name,
                        Type = template.Type,
                        Stats = template.Stats,
                        Skill = template.Skill,
                    };
                }
                _items.Add(item);
            }
        }

        public void DisplayItems()
        {
            Console.WriteLine("Items available in the shop:");
            foreach (var item in _items)
            {
                if (item == null)
                {
                    Console.WriteLine("- Invalid item: item is null");
                    continue;
                }

                if (item.Stats == null)
                {
                    Console.WriteLine($"- Invalid item: {item.Name} has null stats");
                    continue;
                }

                string skillName = item.Skill?.Name ?? "No skill";
                Console.WriteLine($"- {item.Name}: {item.Stats.Health} Health, {item.Stats.Damage} Damage, {item.Stats.Defense} Defense, {item.Stats.LootRarityBonus} Loot Rarity Bonus, skill: {skillName}");
            }
        }


        public void BuyItem(Player player, string itemName)
        {
            var item = _items.Find(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
            if (item != null)
            {
                int itemCost = 10 * player.Level; // Coût basé sur le niveau du joueur
                if (player.Currency >= itemCost)
                {
                    // Deduct the cost from the player's currency
                    player.Currency -= itemCost;

                    // Add the item to the player's inventory
                    player.Inventory.AddItem(item, player);

                    Console.WriteLine($"You have bought a {item.Name} for {itemCost} currency.");
                }
                else
                {
                    Console.WriteLine("You do not have enough currency to buy this item.");
                }
            }
            else
            {
                Console.WriteLine("Item not found.");
            }
        }
    }
}
