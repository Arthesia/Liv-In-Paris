using TESTLivINParis;
/// LeonardOzanTimothe2ndRendu
class Program
{
    static void Main()
    {
        /// Définir le titre de la console
        Console.Title = " Liv'in Paris - Console App";

        /// Boucle principale de l'application
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Bienvenue sur Liv'in Paris ===\n");
            Console.WriteLine("1. Se connecter");
            Console.WriteLine("2. Créer un compte");
            Console.WriteLine("3. Connexion administrateur");
            Console.WriteLine("4. Graphe");
            Console.WriteLine("5. Statistiques");
            Console.WriteLine("0. Quitter\n");
            Console.Write("Votre choix : ");
            string choix = Console.ReadLine();

            /// Gestion du menu principal selon le choix de l'utilisateur
            switch (choix)
            {
                case "1":
                    /// Connexion d'un utilisateur standard
                    Utilisateur.Connexion();
                    break;
                case "2":
                    /// Création d'un nouveau compte utilisateur
                    Utilisateur.CreerCompte();
                    break;
                case "3":
                    /// Connexion en tant qu'administrateur
                    AdminService.ConnexionAdmin();
                    break;
                case "4":
                    /// Lancer l'affichage et les calculs du graphe de métro
                    MainGraphe.LancerSimulationGraphe();
                    break;
                case "5":
                    /// Accéder au menu des statistiques
                    Statistiques.MenuStatistiques();
                    break;
                case "0":
                    /// Quitter l'application
                    Console.WriteLine("À bientôt !");
                    return;
                default:
                    Console.WriteLine("Choix invalide !");
                    break;
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }
}


