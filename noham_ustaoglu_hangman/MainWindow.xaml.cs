
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
        // Liste des mots à deviner
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
            "DEVELOPPEUR",
            "SECRET",
            "ALGORITHME",
            "VARIABLE",
            "FONCTION"

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

        // Démarrer une nouvelle partie
        void NouvellePartie()
        {
            // Choisir un mot aléatoire
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
                    new SolidColorBrush(Color.FromRgb(23, 18, 20));
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

        // Afficher les vies
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

        // Faire disparaître les membres du personnage
        void ActualiserRobot()
        {
            int erreurs = 6 - vies;

            // 1 erreur : bras gauche
            BrasGauche.Visibility = erreurs >= 1
                ? Visibility.Hidden : Visibility.Visible;

            // 2 erreurs : bras droit
            BrasDroit.Visibility = erreurs >= 2
                ? Visibility.Hidden : Visibility.Visible;

            // 3 erreurs : jambe gauche
            JambeGauche.Visibility = erreurs >= 3
                ? Visibility.Hidden : Visibility.Visible;

            // 4 erreurs : jambe droite
            JambeDroite.Visibility = erreurs >= 4
                ? Visibility.Hidden : Visibility.Visible;

            // 5 erreurs : corps
            RobotCorps.Visibility = erreurs >= 5
                ? Visibility.Hidden : Visibility.Visible;

            // 6 erreurs : tête
            RobotTete.Visibility = erreurs >= 6
                ? Visibility.Hidden : Visibility.Visible;
        }

        // Quand le joueur choisit une lettre
        private void Lettre_Click(object sender, RoutedEventArgs e)
        {
            if (partieTerminee)
            {
                return;
            }

            Button bouton = (Button)sender;

            char lettre = bouton.Content.ToString()[0];

            // Désactiver la lettre choisie
            bouton.IsEnabled = false;

            // Vérifier si la lettre est correcte
            if (motSecret.Contains(lettre))
            {
                lettresTrouvees.Add(lettre);

                // Couleur verte
                bouton.Background =
                    new SolidColorBrush(Color.FromRgb(40, 130, 65));

                AfficherMot();

                VerifierVictoire();
            }
            else
            {
                // Couleur rouge
                bouton.Background =
                    new SolidColorBrush(Color.FromRgb(150, 20, 35));

                vies--;

                AfficherVies();

                ActualiserRobot();

                VerifierDefaite();
            }
        }

        // Vérifier la victoire
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
                    "VICTOIRE",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                DesactiverLettres();
            }
        }

        // Vérifier la défaite
        void VerifierDefaite()
        {
            if (vies == 0)
            {
                partieTerminee = true;

                // Afficher le mot secret
                TB_Mot.Text = string.Join(" ", motSecret.ToCharArray());

                MessageBox.Show(
                    "Tu as perdu !\n\nLe mot était : " + motSecret,
                    "DEFAITE",
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
                "Bienvenue dans HANGMAN !\n\n" +
                "Trouve le mot secret en choisissant les lettres.\n\n" +
                "Une mauvaise réponse fait disparaître un membre.\n" +
                "Tu disposes de 6 vies.\n\n" +
                "Bonne chance !",
                "AIDE",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        // Bouton Pause
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            if (!partieTerminee)
            {
                MessageBox.Show(
                    "La partie est en pause.\n\n" +
                    "Clique sur OK pour reprendre.",
                    "PAUSE",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
    }
}
