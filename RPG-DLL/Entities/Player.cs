using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using RPG_DLL.Systems;

namespace RPG_DLL.Entities
{
    public class Player
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Level { get; set; }
        public int MaxHealth { get; set; }
        public List<Skill> ActiveSkills { get; set; } = new List<Skill>(3); // Limiter à 3 compétences
        public Inventory Inventory { get; set; }
        public int CurrentAscenscion { get; set; }
        public Stats PlayerStats { get; set; }
        public int Currency { get; set; } // Ajout de la propriété Currency

        public Item EquippedWeapon { get; set; }
        public Item EquippedArmor { get; set; }
        public Skill EquippedSkill { get; set; } // Ajout de la propriété pour la compétence équipée
        public int Experience { get; set; }

        public Player() { }
        public Player(string name, int level, int maxHealth, List<Skill> activeSkills, Inventory inventory, int currentAscenscion, Stats playerStats)
        {
            Name = name;
            Level = level;
            MaxHealth = maxHealth;
            ActiveSkills = activeSkills.Take(3).ToList(); // Limiter à 3 compétences
            Inventory = inventory;
            CurrentAscenscion = currentAscenscion;
            PlayerStats = playerStats;
            Experience = 0;
            Currency = 0;
        }

        public void TakeDamage(int damage)
        {
            int netDamage = damage - PlayerStats.Defense;
            if (netDamage > 0)
            {
                PlayerStats.Health -= netDamage;
            }
            else
            {
                // Si la défense est supérieure aux dégâts, les dégâts nets sont mis à zéro
                netDamage = 0;
            }
        }

        public void Update()
        {
            if (PlayerStats.Health > MaxHealth)
            {
                PlayerStats.Health = MaxHealth;
            }
        }
        public void Heal(int amount)
        {
            PlayerStats.Health += amount * Level;
            if (PlayerStats.Health > MaxHealth)
            {
                PlayerStats.Health = MaxHealth;
            }
        }

        public void DisplayStats()
        {
            Console.WriteLine($"Nom: {Name}");
            Console.WriteLine($"Niveau: {Level}");
            Console.WriteLine($"Santé Max: {MaxHealth}");
            Console.WriteLine($"Santé Actuelle: {PlayerStats.Health}");
            Console.WriteLine($"Dégâts: {PlayerStats.Damage}");
            Console.WriteLine($"Défense: {PlayerStats.Defense}");
            Console.WriteLine($"Bonus de Rareté de Butin: {PlayerStats.LootRarityBonus}");
        }

        public void ApplyStats(Stats stats)
        {
            Console.WriteLine(stats);
            if (stats == null)
            {
                Console.WriteLine("Stats are null, cannot apply stats.");
                return;
            }
            PlayerStats.AddStats(stats);
            Console.WriteLine($"Statistiques appliquées : Santé +{stats.Health}, Dégâts +{stats.Damage}, Défense +{stats.Defense}, Bonus de rareté de butin +{stats.LootRarityBonus}");
        }


        public void Hold()
        {
            Console.WriteLine("Inventaire:");
            int index = 0;
            var allItems = new List<Item>();

            foreach (var item in Inventory.Items)
            {
                Console.WriteLine($"{index + 1}. {item.Key.Name} x{item.Value}");
                allItems.Add(item.Key);
                index++;
            }

            foreach (var mineStone in Inventory.MineStones)
            {
                Console.WriteLine($"{index + 1}. {mineStone.Key.Name} x{mineStone.Value}");
                allItems.Add(mineStone.Key);
                index++;
            }

            foreach (var equipement in Inventory.Equipement)
            {
                Console.WriteLine($"{index + 1}. {equipement.Key.Name} x{equipement.Value}");
                allItems.Add(equipement.Key);
                index++;
            }

            Console.WriteLine("Quel objet voulez-vous équiper ou consommer ?");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int choice) && choice > 0 && choice <= allItems.Count)
            {
                var selectedItem = allItems[choice - 1];
                if (selectedItem is Equipement equipement)
                {
                    EquipWeaponSkill(equipement);
                }
                else
                {
                    selectedItem.Consume(this);
                    Inventory.RemoveItem(selectedItem);
                }
            }
            else
            {
                Console.WriteLine("Choix invalide.");
            }
        }





        public void EquipWeaponSkill(Equipement equipement)
        {
            if (equipement.Skill != null)
            {
                if (ActiveSkills.Count >= 4)
                {
                    ActiveSkills[3] = equipement.Skill;
                }
                else
                {
                    while (ActiveSkills.Count < 4)
                    {
                        ActiveSkills.Add(null); // Ajouter des slots vides si nécessaire
                    }
                    ActiveSkills[3] = equipement.Skill;
                }
                Console.WriteLine($"La compétence {equipement.Skill.Name} a été équipée sur le slot 4.");
            }
            else
            {
                Console.WriteLine("L'arme sélectionnée n'a pas de compétence.");
            }
        }


        public void EquipSkill(int skillIndex)
        {
            if (skillIndex >= 0 && skillIndex < ActiveSkills.Count)
            {
                var skill = ActiveSkills[skillIndex];
                if (EquippedSkill != null)
                {
                    // Retirer les statistiques de la compétence précédemment équipée
                    PlayerStats.RemoveStats(EquippedSkill.Stats);
                }
                EquippedSkill = skill;
                // Ajouter les statistiques de la nouvelle compétence équipée
                PlayerStats.AddStats(skill.Stats);
                Console.WriteLine($"Compétence {skill.Name} équipée.");
            }
            else
            {
                Console.WriteLine("Choix de compétence invalide.");
            }
        }

        public int CalculateDamage(int skillIndex)
        {
            if (skillIndex >= 0 && skillIndex < ActiveSkills.Count)
            {
                var skill = ActiveSkills[skillIndex];
                return PlayerStats.Damage + skill.Stats.Damage;
            }
            else
            {
                Console.WriteLine("Choix de compétence invalide.");
                return 0;
            }
        }

        public void DisplaySkillsMenu()
        {
            Console.WriteLine("Choisissez une compétence pour attaquer :");
            for (int i = 0; i < ActiveSkills.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {ActiveSkills[i].Name}");
            }
            if (EquippedWeapon != null && EquippedWeapon is Equipement equipement && equipement.Skill != null)
            {
                Console.WriteLine($"{ActiveSkills.Count + 1}. {equipement.Skill.Name} (Compétence de l'arme)");
            }
        }

        public int ChooseSkillForAttack()
        {
            DisplaySkillsMenu();
            string input = Console.ReadLine();

            if (int.TryParse(input, out int choice))
            {
                choice -= 1;
                if (choice >= 0 && choice < ActiveSkills.Count)
                {
                    return choice;
                }
                else if (choice == ActiveSkills.Count && EquippedWeapon != null && EquippedWeapon is Equipement equipement && equipement.Skill != null)
                {
                    return ActiveSkills.Count; // Indique que la compétence de l'arme est choisie
                }
            }

            Console.WriteLine("Choix invalide.");
            return -1; // Indique un choix invalide
        }


        public void UseSkill(int skillIndex, Monster monster)
        {
            if (skillIndex >= 0 && skillIndex < ActiveSkills.Count)
            {
                var skill = ActiveSkills[skillIndex];
                if (skill.EffectType == SkillEffectType.Damage)
                {
                    int damage = CalculateDamage(skillIndex);
                    monster.TakeDamage(damage);
                }
                else if (skill.EffectType == SkillEffectType.Heal)
                {
                    Heal(skill.Stats.Health);
                }
            }
            else if (skillIndex == ActiveSkills.Count && EquippedWeapon != null && EquippedWeapon is Equipement equipement && equipement.Skill != null)
            {
                var skill = equipement.Skill;
                if (skill.EffectType == SkillEffectType.Damage)
                {
                    int damage = equipement.CalculateDamage(this);
                    monster.TakeDamage(damage);
                }
                else if (skill.EffectType == SkillEffectType.Heal)
                {
                    Heal(skill.Stats.Health);
                }
            }
            else
            {
                Console.WriteLine("Choix de compétence invalide.");
            }
        }

        public void AddCurrency(int amount)
        {
            Currency += amount;
            Console.WriteLine($"{amount} pièces d'or gagnées. Total: {Currency}");
        }

        public void AddExperience(int amount)
        {
            Experience += amount;
            Console.WriteLine($"{amount} points d'expérience gagnés. Expérience totale: {Experience}");
            if (Experience >= 100)
            {
                levelUp();
                Experience -= 100;
            }
        }
        public void levelUp()
        {
            Level++;
            MaxHealth += 10;
            PlayerStats.Health = MaxHealth;
            PlayerStats.Damage += 5;
            PlayerStats.Defense += 2;
            Console.WriteLine("Niveau supérieur atteint!");
        }
    }
}
