using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_DLL.Entities;
using RPG_DLL.Systems;

namespace RPG_DLL.Economy
{
    public class LootTable
    {
        public List<Item> possibleDrops { get; set; }

        public List<Item> GenerateLoot()
        {
            List<Item> Drops = new List<Item>();
            Random rand = new Random();
            int i = rand.Next(0, 5);
            for (int j = 0; j < i; j++)
            {
                Drops.Add(possibleDrops[rand.Next(0, possibleDrops.Count)]);
            }
            StatTypes affinity = (StatTypes)rand.Next(0, 5);
            int tier = rand.Next(1, 10); // Assurez-vous que le tier est toujours supérieur à 0

            MineStone rune = new MineStone(tier, affinity);
            Drops.Add(rune);

            return Drops;
        }

    }
}
