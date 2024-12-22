using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPG_DLL.Entities;

namespace RPG_DLL.Systems
{
    public class CombatSystem
    {
        public void AttackMonster(Player player, Monster monster)
        {
            int skillIndex = player.ChooseSkillForAttack();
            if (skillIndex != -1)
            {
                player.UseSkill(skillIndex, monster);

                if (monster.MonsterStats.Health <= 0)
                {
                    player.Inventory.Add(monster.LootTable.GenerateLoot(), player);
                    player.AddCurrency(monster.Tier * player.Level);
                }
            }
        }

        public void AttackPlayer(Player player, Monster monster)
        {
            int damage = monster.getDamage();
            player.TakeDamage(damage);
        }
    }
}
