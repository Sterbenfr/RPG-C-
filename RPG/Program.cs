using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using RPG_DLL.Entities;
using RPG_DLL.Systems;
using RPG_DLL.Economy;

namespace RPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Définir le répertoire de base de l'application car Sqlite ne fonctionne pas, need to be changed to make the app work on your machine
            List<GolemTemplate> monsterTemplates = LoadTemplates<GolemTemplate>("C:\\Users\\pierr\\source\\repos\\RPG\\GameEditor\\bin\\Debug\\net8.0\\golemTemplates.json");

            // Charger les templates d'items pour la boutique
            List<ItemTemplate> itemTemplates = LoadTemplates<ItemTemplate>("C:\\Users\\pierr\\source\\repos\\RPG\\GameEditor\\bin\\Debug\\net8.0\\itemTemplates.json");
            
            // Initialiser les compétences actives
            List<Skill> activeSkills = new List<Skill>
            {
                new Skill("Coup de base", new Stats { Damage = 10 }, SkillEffectType.Damage),
                new Skill("Soin mineur", new Stats { Health = 20 }, SkillEffectType.Heal),
                new Skill("Coup puissant", new Stats { Damage = 25 }, SkillEffectType.Damage)
            };

            // Initialiser l'inventaire
            Inventory inventory = new Inventory
            {
                Items = new Dictionary<Item, int>(),
                MineStones = new Dictionary<MineStone, int>(),
                Equipement = new Dictionary<Equipement, int>()
            };

            // Créer le joueur
            Player player = new Player(
                name: "test",
                level: 1,
                maxHealth: 2000,
                activeSkills: activeSkills,
                inventory: inventory,
                currentAscenscion: 0,
                playerStats: new Stats { Health = 2000, Damage = 10, Defense = 5, LootRarityBonus = 0 }
            );

            // Créer la boutique
            Shop shop = new Shop(itemTemplates);

            // Créer le système de combat
            CombatSystem combatSystem = new CombatSystem();

            // Hub principal
            while (true)
            {
                Console.WriteLine("Hub principal:");
                Console.WriteLine("1. Combattre un golem");
                Console.WriteLine("2. Se soigner");
                Console.WriteLine("3. Regarder l'inventaire");
                Console.WriteLine("4. S'équiper");
                Console.WriteLine("5. Sauvegarder le jeu");
                Console.WriteLine("6. Charger le jeu");
                Console.WriteLine("7. Aller à la boutique");
                Console.WriteLine("8. Sortir");
                string input = Console.ReadLine();

                if (input == "1")
                {
                    // Sélectionner le niveau du golem
                    Console.WriteLine("Sélectionnez le niveau du golem à affronter (1-70):");
                    for (int i = 0; i < monsterTemplates.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {monsterTemplates[i].Type}");
                    }
                    int level;
                    while (true)
                    {
                        string levelInput = Console.ReadLine();
                        if (int.TryParse(levelInput, out level) && level >= 1 && level <= monsterTemplates.Count)
                        {
                            level--; // Ajuster pour l'index de la liste
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Niveau invalide. Veuillez entrer un nombre entre 1 et 70.");
                        }
                    }

                    if (level >= 0 && level < monsterTemplates.Count)
                    {
                        // Créer un golem à partir du modèle
                        GolemTemplate template = monsterTemplates[level];
                        Monster monster = Monster.CreateGolem(template, player.Level, player.PlayerStats.LootRarityBonus);

                        // Boucle de combat
                        while (true)
                        {
                            // Afficher les informations du joueur et du monstre
                            Console.WriteLine($"Joueur: {player.Name}, Santé: {player.PlayerStats.Health}/{player.MaxHealth}");
                            Console.WriteLine($"Monstre: {monster.Name}, Santé: {monster.MonsterStats.Health}");

                            // Demander au joueur de choisir une action
                            Console.WriteLine("Choisissez une action:");
                            Console.WriteLine("1. Attaquer");
                            Console.WriteLine("2. Fuir");
                            string combatInput = Console.ReadLine();

                            if (combatInput == "1")
                            {
                                // Attaquer le monstre
                                combatSystem.AttackMonster(player, monster);

                                // Vérifier si le monstre est mort
                                if (monster.MonsterStats.Health <= 0)
                                {
                                    Console.WriteLine($"{monster.Name} est vaincu!");
                                    player.Inventory.Add(monster.LootTable.GenerateLoot(), player);
                                    player.AddExperience(monster.ExperienceValue); // Ajouter de l'expérience au joueur
                                    break;
                                }

                                // Le monstre attaque le joueur
                                combatSystem.AttackPlayer(player, monster);

                                // Vérifier si le joueur est mort
                                if (player.PlayerStats.Health <= 0)
                                {
                                    Console.WriteLine("Vous êtes mort!");
                                    return;
                                }
                            }
                            else if (combatInput == "2")
                            {
                                // Fuir le combat
                                Console.WriteLine("Vous avez fui le combat.");
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Choix invalide. Veuillez réessayer.");
                            }
                        }

                    }
                    else
                    {
                        Console.WriteLine("Niveau invalide. Veuillez réessayer.");
                    }
                }
                else if (input == "2")
                {
                    // Se soigner
                    Console.WriteLine("Combien de points de vie voulez-vous récupérer ?");
                    int healAmount = Convert.ToInt32(Console.ReadLine());
                    player.Heal(healAmount);
                    Console.WriteLine($"Vous avez récupéré {healAmount} points de vie.");
                }
                else if (input == "3")
                {
                    // Regarder l'inventaire
                    player.Inventory.DisplayInventory();
                }
                else if (input == "4")
                {
                    // S'équiper
                    player.Hold();
                }
                else if (input == "5")
                {
                    // Sauvegarder le jeu
                    SaveGame(player);
                    Console.WriteLine("Jeu sauvegardé.");
                }
                else if (input == "6")
                {
                    // Charger le jeu
                    List<int> availableIds = GetAvailableSaveIds();
                    Console.WriteLine("IDs disponibles pour charger le jeu :");
                    foreach (int id in availableIds)
                    {
                        Console.WriteLine(id);
                    }
                    Console.WriteLine("Entrez l'ID du joueur à charger :");
                    int playerId = Convert.ToInt32(Console.ReadLine());
                    player = LoadGameFromJson(playerId);
                    Console.WriteLine("Jeu chargé.");
                }
                else if (input == "7")
                {
                    // Aller à la boutique
                    while (true)
                    {
                        Console.WriteLine("Boutique:");
                        shop.DisplayItems();
                        Console.WriteLine("Entrez le nom de l'article que vous souhaitez acheter ou 'retour' pour revenir au menu principal:");
                        string itemName = Console.ReadLine();
                        if (itemName.ToLower() == "retour")
                        {
                            break;
                        }
                        shop.BuyItem(player, itemName);
                    }
                }
                else if (input == "8")
                {
                    // Sortir du jeu
                    Console.WriteLine("Vous avez quitté le jeu.");
                    break;
                }
                else
                {
                    Console.WriteLine("Choix invalide. Veuillez réessayer.");
                }
                player.DisplayStats();
            }
        }

        public static void SaveGame(Player player)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            options.Converters.Add(new ItemConverter());
            options.Converters.Add(new MineStoneConverter());
            options.Converters.Add(new EquipementConverter());
            string jsonString = JsonSerializer.Serialize(player, options);
            File.WriteAllText($"player_{player.Id}.json", jsonString);
        }

        public static List<int> GetAvailableSaveIds()
        {
            List<int> ids = new List<int>();
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "player_*.json");
            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string idString = fileName.Replace("player_", "");
                if (int.TryParse(idString, out int id))
                {
                    ids.Add(id);
                }
            }
            return ids;
        }

        public static Player LoadGameFromJson(int playerId)
        {
            string jsonString = File.ReadAllText($"player_{playerId}.json");

            var options = new JsonSerializerOptions();
            options.Converters.Add(new ItemConverter());
            options.Converters.Add(new MineStoneConverter());
            options.Converters.Add(new EquipementConverter());

            return JsonSerializer.Deserialize<Player>(jsonString, options);
        }

        public static List<T> LoadTemplates<T>(string fileName)
        {
            string jsonString = File.ReadAllText(fileName);

            var options = new JsonSerializerOptions();
            options.Converters.Add(new EquipementConverter());

            return JsonSerializer.Deserialize<List<T>>(jsonString, options);
        }
    }
}

