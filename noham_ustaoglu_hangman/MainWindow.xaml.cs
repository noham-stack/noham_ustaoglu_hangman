
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NohamCalculette
{
    public partial class MainWindow : Window
    {
        // Liste des mots
        string[] mots =
        {
            "ORDINATEUR",
            "PROGRAMME",
            "CLAVIER",
            "SOURIS",
            "VISUALSTUDIO",
            "WINDOWS",
            "ROBOT",
            "RESEAU",
            "INTERNET",
            "CALCULATRICE",
            "ELECTRONIQUE",
            "DEVELOPPEUR"
        };

        // Variables du jeu
        string motSecret;
        List<char> lettresTrouvees = new List<char>();

        int vies = 6;
        bool partieTerminee = false;

        Random hasard = new Random();

        public MainWindow()
        {
            InitializeComponent();

            NouvellePartie();
        }

        // Commencer une nouvelle partie
        void NouvellePartie()
        {
            // Choisir un mot au hasard
            motSecret = mots[hasard.Next(mots.Length)];

            lettresTrouvees.Clear();

            vies = 6;
            partieTerminee = false;

            // Réactiver toutes les lettres
            for (char lettre = 'A'; lettre <= 'Z'; lettre++)
            {
                Button bouton = (Button)FindName("BTN_" + lettre);

                bouton.IsEnabled = true;

                bouton.Background =
                    new SolidColorBrush(Color.FromRgb(221, 166, 93));
            }

            AfficherMot();
            AfficherVies();
            ActualiserRobot();
        }

        // Afficher le mot à deviner
        void AfficherMot()
        {
            string affichage = "";

            foreach (char lettre in motSecret)
            {
                if (lettresTrouvees.Contains(lettre))
                {
                    affichage += lettre + " ";
                }
                else
                {
                    affichage += "_ ";
                }
            }

            TB_Mot.Text = affichage.Trim();
        }

        // Afficher les vies restantes
        void AfficherVies()
        {
            string coeurs = "";

            for (int i = 0; i < vies; i++)
            {
                coeurs += "♥ ";
            }

            for (int i = vies; i < 6; i++)
            {
                coeurs += "♡ ";
            }

            TB_Vies.Text = coeurs;
        }

        // Faire disparaître les membres du robot
        void ActualiserRobot()
        {
            int erreurs = 6 - vies;

            // Première erreur : bras gauche
            BrasGauche.Visibility = erreurs >= 1
                ? Visibility.Hidden : Visibility.Visible;

            // Deuxième erreur : bras droit
            BrasDroit.Visibility = erreurs >= 2
                ? Visibility.Hidden : Visibility.Visible;

            // Troisième erreur : jambe gauche
            JambeGauche.Visibility = erreurs >= 3
                ? Visibility.Hidden : Visibility.Visible;

            // Quatrième erreur : jambe droite
            JambeDroite.Visibility = erreurs >= 4
                ? Visibility.Hidden : Visibility.Visible;

            // Cinquième erreur : corps
            RobotCorps.Visibility = erreurs >= 5
                ? Visibility.Hidden : Visibility.Visible;

            // Sixième erreur : tête
            RobotTete.Visibility = erreurs >= 6
                ? Visibility.Hidden : Visibility.Visible;

            OeilGauche.Visibility = erreurs >= 6
                ? Visibility.Hidden : Visibility.Visible;

            OeilDroit.Visibility = erreurs >= 6
                ? Visibility.Hidden : Visibility.Visible;

            Sourire.Visibility = erreurs >= 6
                ? Visibility.Hidden : Visibility.Visible;
        }

        // Quand le joueur clique sur une lettre
        private void Lettre_Click(object sender, RoutedEventArgs e)
        {
            if (partieTerminee)
            {
                return;
            }

            Button bouton = (Button)sender;

            char lettre = bouton.Content.ToString()[0];

            // Désactiver la lettre utilisée
            bouton.IsEnabled = false;

            // Vérifier si la lettre est dans le mot
            if (motSecret.Contains(lettre))
            {
                lettresTrouvees.Add(lettre);

                bouton.Background =
                    new SolidColorBrush(Color.FromRgb(105, 180, 65));

                AfficherMot();

                VerifierVictoire();
            }
            else
            {
                vies--;

                bouton.Background =
                    new SolidColorBrush(Color.FromRgb(220, 65, 55));

                AfficherVies();

                ActualiserRobot();

                VerifierDefaite();
            }
        }

        // Vérifier si le joueur a gagné
        void VerifierVictoire()
        {
            bool gagne = true;

            foreach (char lettre in motSecret)
            {
                if (!lettresTrouvees.Contains(lettre))
                {
                    gagne = false;
                    break;
                }
            }

            if (gagne)
            {
                partieTerminee = true;

                MessageBox.Show(
                    "Bravo ! Tu as trouvé le mot : " + motSecret,
                    "Victoire",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                DesactiverLettres();
            }
        }

        // Vérifier si le joueur a perdu
        void VerifierDefaite()
        {
            if (vies == 0)
            {
                partieTerminee = true;

                TB_Mot.Text = string.Join(" ", motSecret.ToCharArray());

                MessageBox.Show(
                    "Tu as perdu ! Le mot était : " + motSecret,
                    "Défaite",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                DesactiverLettres();
            }
        }

        // Désactiver toutes les lettres
        void DesactiverLettres()
        {
            for (char lettre = 'A'; lettre <= 'Z'; lettre++)
            {
                Button bouton = (Button)FindName("BTN_" + lettre);

                bouton.IsEnabled = false;
            }
        }

        // Bouton Rejouer
        private void Rejouer_Click(object sender, RoutedEventArgs e)
        {
            NouvellePartie();
        }

        // Bouton Aide
        private void Aide_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Bienvenue dans Hangman !\n\n" +
                "Le but est de trouver le mot secret.\n" +
                "Clique sur les lettres pour découvrir le mot.\n\n" +
                "Une mauvaise réponse fait disparaître un membre.\n" +
                "Tu disposes de 6 vies.\n\n" +
                "Bonne chance !",
                "Aide",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        // Bouton Pause
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            if (!partieTerminee)
            {
                MessageBox.Show(
                    "Partie en pause.\nClique sur OK pour reprendre.",
                    "Pause",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
    }
}
