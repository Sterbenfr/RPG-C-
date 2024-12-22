using RPG_DLL.Entities;
using System;
using System.Collections.Generic;

public class Inventory
{
    public Dictionary<Item, int> Items { get; set; }
    public Dictionary<MineStone, int> MineStones { get; set; }
    public Dictionary<Equipement, int> Equipement { get; set; }

    public Inventory()
    {
        Items = new Dictionary<Item, int>();
        MineStones = new Dictionary<MineStone, int>();
        Equipement = new Dictionary<Equipement, int>();
    }

    public void Add(List<Item> items, Player player)
    {
        foreach (var item in items)
        {
            AddItem(item, player);
        }
    }

    public void AddItem(Item item, Player player)
    {
        if (item is MineStone mineStone)
        {
            mineStone.AddToPlayer(player); // Appeler AddToPlayer pour mettre à jour les statistiques du joueur
        }
        else if (item is Equipement equipement)
        {
            AddEquipement(equipement);
        }
        else
        {
            if (Items.ContainsKey(item))
            {
                Items[item]++;
            }
            else
            {
                Items[item] = 1;
            }
        }
    }

    public void RemoveItem(Item item)
    {
        if (Items.ContainsKey(item))
        {
            Items[item]--;
            if (Items[item] <= 0)
            {
                Items.Remove(item);
            }
        }
    }

    public void AddMineStone(MineStone mineStone)
    {
        if (MineStones.ContainsKey(mineStone))
        {
            MineStones[mineStone]++;
        }
        else
        {
            MineStones[mineStone] = 1;
        }
    }

    public void RemoveMineStone(MineStone mineStone)
    {
        if (MineStones.ContainsKey(mineStone))
        {
            MineStones[mineStone]--;
            if (MineStones[mineStone] <= 0)
            {
                MineStones.Remove(mineStone);
            }
        }
    }

    public void AddEquipement(Equipement equipement)
    {
        if (Equipement.ContainsKey(equipement))
        {
            Equipement[equipement]++;
        }
        else
        {
            Equipement[equipement] = 1;
        }
    }

    public void RemoveEquipement(Equipement equipement)
    {
        if (Equipement.ContainsKey(equipement))
        {
            Equipement[equipement]--;
            if (Equipement[equipement] <= 0)
            {
                Equipement.Remove(equipement);
            }
        }
    }

    public void DisplayInventory()
    {
        Console.WriteLine("Inventory:");
        foreach (var item in Items)
        {
            Console.WriteLine($"{item.Key.Name} x{item.Value}");
        }
        DisplayMineStones();
        DisplayEquipement();
    }

    public void DisplayEquipement()
    {
        Console.WriteLine("Equipement:");
        foreach (var equipement in Equipement)
        {
            Console.WriteLine($"{equipement.Key.Name} x{equipement.Value}");
        }
    }

    public void DisplayMineStones()
    {
        Console.WriteLine("Mine Stones:");
        foreach (var mineStone in MineStones)
        {
            Console.WriteLine($"{mineStone.Key.Name} x{mineStone.Value}");
        }
    }



    public void EquipItem(Item item, Player player)
    {
        if (item.Type == ItemType.Weapon && item is Equipement equipement)
        {
            if (player.EquippedWeapon != null)
            {
                // Retirer les buffs de l'arme précédente
                player.PlayerStats.RemoveStats(player.EquippedWeapon.Stats);
            }
            player.EquippedWeapon = item;
            // Ajouter les buffs de la nouvelle arme
            player.PlayerStats.AddStats(item.Stats);
            // Équiper la compétence de l'arme sur le slot 4
            player.EquipWeaponSkill(equipement);
        }
        else if (item.Type == ItemType.Armor)
        {
            if (player.EquippedArmor != null)
            {
                // Retirer les buffs de l'armure précédente
                player.PlayerStats.RemoveStats(player.EquippedArmor.Stats);
            }
            player.EquippedArmor = item;
            // Ajouter les buffs de la nouvelle armure
            player.PlayerStats.AddStats(item.Stats);
        }
    }

}
