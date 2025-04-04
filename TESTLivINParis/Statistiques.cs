using MySql.Data.MySqlClient;
/// LeonardOzanTimothe2ndRendu

namespace TESTLivINParis
{
    internal class Statistiques
    {
        public static string ConnectionString = AdminService.ConnectionString; /// Chaîne de connexion utilisée pour accéder à la base de données.

        public static void AfficherLivraisonsParCuisinier() /// Méthode qui affiche le nombre total de livraisons effectuées par chaque cuisinier triés par nombre de livraisons décroissants
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();

                var cmd = new MySqlCommand(@"
                SELECT c.id_utilisateur, u.nom, u.prenom, COUNT(l.id_livraison) AS nb_livraisons
                FROM Cuisinier c
                JOIN Utilisateur u ON c.id_utilisateur = u.id_utilisateur
                LEFT JOIN livraison l ON c.id_utilisateur = l.id_cuisinier
                GROUP BY c.id_utilisateur, u.nom, u.prenom
                ORDER BY nb_livraisons DESC", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("\nNombre de livraisons par cuisinier :\n");

                    while (reader.Read())
                    {
                        Console.WriteLine($"Cuisinier ID: {reader["id_utilisateur"]} | Nom: {reader["nom"]} {reader["prenom"]} | Livraisons effectuées : {reader["nb_livraisons"]}");
                    }
                }
            }

            Console.WriteLine("\nAppuyez sur une touche pour revenir...");
            Console.ReadKey();
        }
        private static void AfficherCommandesParPeriode()  /// Affiche toutes les commandes passées dans une période définie par l'utilisateur.
        {
            Console.Write("Entrez la date de début (format AAAA-MM-JJ) : ");
            string dateDebutStr = Console.ReadLine();

            Console.Write("Entrez la date de fin (format AAAA-MM-JJ) : ");
            string dateFinStr = Console.ReadLine();

            if (!DateTime.TryParse(dateDebutStr, out DateTime dateDebut) || !DateTime.TryParse(dateFinStr, out DateTime dateFin))
            {
                Console.WriteLine("❌ Format de date invalide.");
                return;
            }

            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();

                var cmd = new MySqlCommand(@"
            SELECT c.id_commande, c.date_commande, c.montant_commande, u.nom, u.prenom
            FROM Commande c
            JOIN Utilisateur u ON c.id_client = u.id_utilisateur
            WHERE c.date_commande BETWEEN @start AND @end
            ORDER BY c.date_commande", conn);

                cmd.Parameters.AddWithValue("@start", dateDebut);
                cmd.Parameters.AddWithValue("@end", dateFin);

                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine($"\nCommandes du {dateDebut:yyyy-MM-dd} au {dateFin:yyyy-MM-dd} :\n");

                    bool hasRows = false;
                    while (reader.Read())
                    {
                        hasRows = true;
                        Console.WriteLine($"Commande #{reader["id_commande"]} | Date : {reader["date_commande"]:d} | Montant : {reader["montant_commande"]} | Client : {reader["nom"]} {reader["prenom"]}");
                    }

                    if (!hasRows)
                        Console.WriteLine("Aucune commande trouvée sur cette période.");
                }
            }
        }
        private static void AfficherMoyennePrixCommandes() /// Calcule et affiche la moyenne des prix de toutes les commandes.
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();

                var cmd = new MySqlCommand("SELECT AVG(montant_commande) AS moyenne FROM Commande", conn);

                object result = cmd.ExecuteScalar();

                if (result != DBNull.Value && result != null)
                {
                    decimal moyenne = Convert.ToDecimal(result);
                    Console.WriteLine($"\nPrix moyen des commandes : {moyenne:F2}");
                }
                else
                {
                    Console.WriteLine("Aucune commande enregistrée pour calculer une moyenne.");
                }
            }
        }

        private static void AfficherCommandesClientParNationaliteEtPeriode() /// Affiche les commandes d’un client sur une période donnée, triées par nationalité de plat, l'utilisateur saisit l'ID du client et une plage de dates.
        {
            Console.Write("Entrez l'ID du client : ");
            int idClient = int.Parse(Console.ReadLine());

            Console.Write("Date de début (YYYY-MM-DD) : ");
            string dateDebut = Console.ReadLine();

            Console.Write("Date de fin (YYYY-MM-DD) : ");
            string dateFin = Console.ReadLine();

            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();

                var cmd = new MySqlCommand(@"
            SELECT c.id_commande, c.date_commande, p.nom_plat, p.nationalite_plat, p.prix_par_personne
            FROM Commande c
            JOIN LigneCommande lc ON lc.id_commande = c.id_commande
            JOIN Plat p ON p.id_plat = lc.id_plat
            WHERE c.id_client = @idClient
              AND c.date_commande BETWEEN @dateDebut AND @dateFin
            ORDER BY p.nationalite_plat, c.date_commande", conn);

                cmd.Parameters.AddWithValue("@idClient", idClient);
                cmd.Parameters.AddWithValue("@dateDebut", dateDebut);
                cmd.Parameters.AddWithValue("@dateFin", dateFin);

                using (var reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("\nCommandes du client (filtrées par nationalité et période) :\n");

                    if (!reader.HasRows)
                    {
                        Console.WriteLine("Aucune commande trouvée pour ce client durant cette période.");
                        return;
                    }

                    while (reader.Read())
                    {
                        Console.WriteLine($"Commande #{reader["id_commande"]} | Date : {Convert.ToDateTime(reader["date_commande"]).ToShortDateString()} | Plat : {reader["nom_plat"]} | 🇫🇷 Nationalité : {reader["nationalite_plat"]} | Prix : {reader["prix_par_personne"]}");
                    }
                }
            }
        }

        public static void MenuStatistiques()
        {
            while (true)
            {
                /// Menu principal permettant d'accéder aux statistiques diverses, 
                Console.Clear();
                Console.WriteLine("=== Menu des Statistiques ===\n");
                Console.WriteLine("1. Nombre de livraisons par cuisinier");
                Console.WriteLine("2. Afficher les commandes dans une période");
                Console.WriteLine("3. Moyenne des prix des commandes");
                Console.WriteLine("4. Commandes d’un client par nationalité et période");
                Console.WriteLine("0. Retour\n");
                Console.Write("Votre choix : ");
                string choix = Console.ReadLine();

                switch (choix)
                {
                    case "1":
                        AfficherLivraisonsParCuisinier(); ///Appelle la méthode pour compter les livraisons effectuées par chaque cuisinier
                        break;
                    case "2":
                        AfficherCommandesParPeriode(); /// Appelle la méthode pour afficher les commandes filtrées par une période de dates
                        break;
                    case "3":
                        AfficherMoyennePrixCommandes(); /// Affiche la moyenne des montants de toutes les commandes
                        break;
                    case "4":
                        AfficherCommandesClientParNationaliteEtPeriode(); /// Affiche les commandes pour un client selon nationalité des plats + période
                        break;
                    case "0":
                        return; /// Quitte le menu statistique
                    default:
                        Console.WriteLine("Choix invalide.");
                        break;
                }

                Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                Console.ReadKey();
            }
        }
    }

}
