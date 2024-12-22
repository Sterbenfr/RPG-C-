using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using RPG_DLL.Systems;

namespace RPG_DLL.Entities
{
    public class Equipement : Item
    {
        public Stats Stats { get; set; }

        public Skill Skill { get; set; }

        public Equipement() { }

        public Equipement(Stats stats, Skill skill, ItemType itemType, string name)
        {
            Name = name;
            Type = itemType;
            Stats = stats;
            Skill = skill;
        }

        public int CalculateDamage(Player player)
        {
            return Stats.Damage + Skill.Stats.Damage + player.PlayerStats.Damage;
        }
    }
}
