using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.XPath;

namespace RPG_nagymarci_WPF_eng
{
    public class Character
    {
        public string CharacterName { get; private set; }
        public int Level { get; private set; }
        public int Health { get; private set; }
        public int CombatSkill { get; private set; }
        public string Equipment { get; private set; }
        public int Xp { get; private set; }
        public bool IsDead => Health <= 0;

        public Character(string name, int level = 1, int health = 100, int combatSkill = 10, string equipment = "Wooden Sword", int xp = 0)
        {
            CharacterName = name;
            Level = level;
            Health = health;
            CombatSkill = combatSkill;
            Equipment = equipment;
            Xp = xp;
        }

        public void GainExperience()
        {
            Xp++;
        }

        public void ResetExperience()
        {
            Xp = 0;
        }

        public void LevelUp()
        {
            if (Level > 14)
            {
                Level = 14;
            }
            else if (Level < 14)
            {
                Level++;
                Health += 5;
                CombatSkill += 5;
            }
        }

        public void Attack(Character target)
        {
            target.TakeDamage(CombatSkill);
        }

        public void Defend()
        {
            Console.WriteLine($"{CharacterName} is defending, reducing damage from the next attack.");
            Health += 10;
        }

        public void Heal()
        {
            Random random = new Random();
            int chance = random.Next(1, 7);

            if (Health > 300)
            {
                Health = 300;
            }

            if (chance == 1)
            {
                Health += 10;
            }
            else if (chance == 2)
            {
                Health += 15;
            }
            else if (chance == 3)
            {
                Health += 20;
            }
            else if (chance == 4)
            {
                Health += 30;
            }
            else if (chance == 5)
            {
                Health += 40;
            }
            else
            {
                Health += 50;
            }
        }

        public void UpgradeEquipment()
        {
            if (Equipment == "Magic Sword")
            {
                Equipment = "Odin's Legendary Sword";
                CombatSkill += 15;
            }
            else if (Equipment == "Wooden Sword")
            {
                Equipment = "Magic Sword";
                CombatSkill += 30;
            }
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Health = 0;
            }
        }

        public void SaveToFile()
        {
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt");
            foreach (var file in files)
            {
                string[] lines = File.ReadAllLines(file);
                if (lines.Length >= 6)
                {
                    if (int.Parse(lines[2].Split(':')[1]) == 0)
                    {
                        File.Delete(file);
                    }
                }
            }
            string fileName = CharacterName + ".txt";
            string content = $"Name: {CharacterName}\nLevel: {Level}\nHealth: {Health}\nCombat Skill: {CombatSkill}\nEquipment: {Equipment}\nXp: {Xp}";
            File.WriteAllText(fileName, content);
        }

        public static List<Character> LoadFromFile()
        {
            List<Character> loadedHeroes = new List<Character>();
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt");
            foreach (var file in files)
            {
                string[] lines = File.ReadAllLines(file);
                if (lines.Length >= 6)
                {
                    if (int.Parse(lines[2].Split(':')[1]) != 0)
                    {
                        string name = lines[0].Split(':')[1].Trim();
                        int level = int.Parse(lines[1].Split(':')[1]);
                        int health = int.Parse(lines[2].Split(':')[1]);
                        int combatSkill = int.Parse(lines[3].Split(':')[1]);
                        string equipment = lines[4].Split(':')[1].Trim();
                        int xp = int.Parse(lines[5].Split(':')[1]);

                        Character newHero = new Character(name, level, health, combatSkill, equipment, xp);
                        loadedHeroes.Add(newHero);
                    }
                    else
                    {
                        File.Delete(file);
                    }
                }
            }
            return loadedHeroes;
        }
    }
}
