using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;

namespace RPG_nagymarci_WPF_eng
{
    public partial class MainWindow : Window
    {
        public int adventures = 0;
        private List<Character> heroes = new List<Character>();

        public MainWindow()
        {
            InitializeComponent();
            heroes = Character.LoadFromFile();
            lstCharacters.ItemsSource = heroes;
        }

        private void RefreshList()
        {
            lstCharacters.Items.Refresh();
            lstCharacters.ItemsSource = heroes;
        }

        private void AddCharacter(object sender, RoutedEventArgs e)
        {
            string name = Microsoft.VisualBasic.Interaction.InputBox("Enter the character's name:", "New Character");
            if (!string.IsNullOrWhiteSpace(name))
            {
                Character newHero = new Character(name);
                heroes.Add(newHero);
                RefreshList();
                MessageBox.Show($"{name} has been added to the characters!", "Success");
            }
        }

        private void Attack(object sender, RoutedEventArgs e)
        {
            if (heroes.Count < 2)
            {
                MessageBox.Show("At least two characters are required to attack!", "Attack Failed!", MessageBoxButton.OK);
                return;
            }
            AttackWindow attackWindow = new AttackWindow(heroes);
            if (attackWindow.ShowDialog() == true)
            {
                Character attacker = attackWindow.SelectedAttacker;
                Character target = attackWindow.SelectedTarget;

                if (attacker == null || target == null)
                    return;

                attacker.Attack(target);
                MessageBox.Show($"{attacker.CharacterName} attacked {target.CharacterName}!{ target.CharacterName}'s new health: {target.Health}", "Successful Attack", MessageBoxButton.OK);
                RefreshList();
                CheckDeath(target);
            }
        }

        private void Heal(object sender, RoutedEventArgs e)
        {
            if (lstCharacters.SelectedItem is Character character)
            {
                character.Heal();
                RefreshList();
                MessageBox.Show($"{character.CharacterName} healed themselves! New health: { character.Health}", "Healing", MessageBoxButton.OK);
            }
        }

        private void LevelUp(object sender, RoutedEventArgs e)
        {
            if (lstCharacters.SelectedItem is Character character)
            {
                character.LevelUp();
                RefreshList();
                MessageBox.Show($"{character.CharacterName} leveled up! New level: { character.Level}", "Level Up", MessageBoxButton.OK);
                levelUpButton.Content = $"Level Up ({adventures}/5)";
                levelUpButton.IsEnabled = false;
            }
        }

        private void UpgradeEquipment(object sender, RoutedEventArgs e)
        {
            if (lstCharacters.SelectedItem is Character character)
            {
                Random random = new Random();
                int chance = random.Next(1, 7);

                if (chance == 6)
                {
                    character.UpgradeEquipment();
                    MessageBox.Show($"{character.CharacterName} received new equipment: {character.Equipment}", "Successful Adventure!", MessageBoxButton.OK);
                    character.GainExperience();
                    adventures += 1;
                    levelUpButton.Content = $"Level Up ({adventures}/5)";
                }
                else
                {
                    MessageBox.Show($"{character.CharacterName} went on an adventure but found nothing.", "Unsuccessful Adventure!", MessageBoxButton.OK);
                    character.GainExperience();
                    adventures += 1;
                    levelUpButton.Content = $"Level Up ({adventures}/5)";
                }
                if (adventures == 5)
                {
                    levelUpButton.Content = $"Level Up ({adventures}/5)";
                    levelUpButton.IsEnabled = true;
                    adventures = 0;
                    character.ResetExperience();
                }
                RefreshList();
            }
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            foreach (var character in heroes)
            {
                character.SaveToFile();
                heroes = Character.LoadFromFile();
                lstCharacters.ItemsSource = heroes;
            }
            MessageBox.Show("All characters have been saved to file!", "Save Successful", MessageBoxButton.OK);
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CheckDeath(Character target)
        {
            if (target.Health <= 0)
            {
                MessageBox.Show($"{target.CharacterName} has fallen in battle!", "Death", MessageBoxButton.OK);
                foreach (var character in heroes)
                {
                    character.SaveToFile();
                    heroes = Character.LoadFromFile();
                    lstCharacters.ItemsSource = heroes;
                }
                RefreshList();
            }
        }
    }
}
