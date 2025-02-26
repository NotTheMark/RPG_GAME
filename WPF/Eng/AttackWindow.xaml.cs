using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RPG_nagymarci_WPF_eng
{
    public partial class AttackWindow : Window
    {
        public Character SelectedAttacker { get; private set; }
        public Character SelectedTarget { get; private set; }

        private List<Character> heroes;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public AttackWindow(List<Character> heroList)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
            InitializeComponent();
            heroes = heroList.Where(h => !h.IsDead).ToList();

            cmbAttacker.ItemsSource = heroes;
            cmbTarget.ItemsSource = heroes;

            cmbAttacker.DisplayMemberPath = "CharacterName";
            cmbTarget.DisplayMemberPath = "CharacterName";
        }

        private void AttackButton_Click(object sender, RoutedEventArgs e)
        {
            if (cmbAttacker.SelectedItem is Character attacker && cmbTarget.SelectedItem is Character target)
            {
                if (attacker == target)
                {
                    MessageBox.Show("A character cannot attack itself!", "Error", MessageBoxButton.OK);
                    return;
                }

                SelectedAttacker = attacker;
                SelectedTarget = target;

                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Select an attacker and a target!", "Warning", MessageBoxButton.OK);
            }
        }
    }
}