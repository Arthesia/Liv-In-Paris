using LeonardOzanTimothe2ndRenduGRAPHE;
using MySql.Data.MySqlClient;
/// LeonardOzanTimothe2ndRendu

public static class ClientService
{
    public static string ConnectionString = AdminService.ConnectionString;

    public static void MenuClient(int id) /// Menu principal lorsqu'on se connecte en tant que client
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Menu Client ===\n");
            Console.WriteLine("1. Gérer mon compte");
            Console.WriteLine("2. Voir les plats disponibles");
            Console.WriteLine("3. Commander des plats");
            Console.WriteLine("4. Historique des avis");
            Console.WriteLine("5. Historique des transactions");
            Console.WriteLine("0. Retour\n");
            Console.Write("Votre choix : ");
            string choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    GererMonCompte(id);
                    break;
                case "2":
                    AfficherPlatsDisponibles();
                    break;
                case "3":
                    PasserCommande(id);
                    break;
                case "4":
                    AfficherAvisClient(id);
                    break;
                case "5":
                    AfficherTransactionsClient(id);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Choix invalide !");
                    break;
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }

    private static void GererMonCompte(int id) /// Permet au client de modifier les informations de son compte ou supprimer son compte.
    {
        Console.WriteLine("\n1. Modifier mes informations");
        Console.WriteLine("2. Supprimer mon compte");
        Console.Write("Votre choix : ");
        string choix = Console.ReadLine();

        if (choix == "1")
        {
            AdminService.ModifierUtilisateurParClient(id);
        }
        else if (choix == "2")
        {
            using (var conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                var del1 = new MySqlCommand("DELETE FROM Client WHERE id_utilisateur = @id", conn);
                var del2 = new MySqlCommand("DELETE FROM Utilisateur WHERE id_utilisateur = @id", conn);
                del1.Parameters.AddWithValue("@id", id);
                del2.Parameters.AddWithValue("@id", id);
                del1.ExecuteNonQuery();
                del2.ExecuteNonQuery();
                Console.WriteLine(" Compte supprimé avec succès.");
            }
            Environment.Exit(0);
        }
    }

    private static void AfficherPlatsDisponibles() /// Affiche tous les plats disponibles dans la base avec le regime alimentaire et les portions disponibles
    {
        using (var conn = new MySqlConnection(ConnectionString))
        {
            conn.Open();
            var cmd = new MySqlCommand("SELECT nom_plat, regime_alimentaire, portions_disponibles FROM Plat", conn);
            using (var reader = cmd.ExecuteReader())
            {
                Console.WriteLine("\n Plats disponibles :\n");
                while (reader.Read())
                {
                    Console.WriteLine($"- {reader["nom_plat"]} | Régime : {reader["regime_alimentaire"]} | Portions : {reader["portions_disponibles"]}");
                }
            }
        }
    }

    private static void PasserCommande(int idClient) /// Permet au client de commander un plat et affiche le trajet le plus court en minutes
    {
        Console.Clear();
        Console.WriteLine("=== Liste des plats disponibles ===\n");

        using (var conn = new MySqlConnection(ConnectionString))
        {
            conn.Open();

            /// liste de plats disponibles avec l'ID du cuisinier qui le propose...
            var cmdPlats = new MySqlCommand(@"
            SELECT p.id_plat, p.nom_plat, p.prix_par_personne, p.portions_disponibles, pr.id_cuisinier
            FROM Plat p
            JOIN Propose pr ON p.id_plat = pr.id_plat", conn);

            using (var reader = cmdPlats.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["id_plat"]} | {reader["nom_plat"]} | Prix : {reader["prix_par_personne"]}€ | Portions dispo : {reader["portions_disponibles"]} | Cuisinier ID: {reader["id_cuisinier"]}");
                }
            }

            Console.Write("\nEntrez l'ID du plat à commander : ");
            int idPlat = int.Parse(Console.ReadLine());

            Console.Write("Quantité souhaitée : ");
            int quantite = int.Parse(Console.ReadLine());

            /// Récupération du prix unitaire
            var cmdPrix = new MySqlCommand("SELECT prix_par_personne FROM Plat WHERE id_plat = @id", conn);
            cmdPrix.Parameters.AddWithValue("@id", idPlat);
            object result = cmdPrix.ExecuteScalar();

            if (result == null)
            {
                Console.WriteLine("❌ Plat introuvable.");
                return;
            }

            decimal prixUnitaire = Convert.ToDecimal(result);
            decimal total = prixUnitaire * quantite;

            /// Récupération du cuisinier ayant proposé le plat
            var getCuisinier = new MySqlCommand("SELECT id_cuisinier FROM Propose WHERE id_plat = @id", conn);
            getCuisinier.Parameters.AddWithValue("@id", idPlat);
            int idCuisinier = Convert.ToInt32(getCuisinier.ExecuteScalar());

            /// Récupération des stations du client et du cuisinier
            var cmdStations = new MySqlCommand(@"
            SELECT 
                (SELECT station_metro FROM Utilisateur WHERE id_utilisateur = @clientId) AS client_station,
                (SELECT station_metro FROM Utilisateur WHERE id_utilisateur = @cuisinierId) AS cuisinier_station
        ", conn);

            cmdStations.Parameters.AddWithValue("@clientId", idClient);
            cmdStations.Parameters.AddWithValue("@cuisinierId", idCuisinier);

            string stationClient = null, stationCuisinier = null;
            using (var reader = cmdStations.ExecuteReader())
            {
                if (reader.Read())
                {
                    stationClient = reader["client_station"]?.ToString();
                    stationCuisinier = reader["cuisinier_station"]?.ToString();
                }
            }
            Console.WriteLine($"\n Station client : '{stationClient}' | Station cuisinier : '{stationCuisinier}'");


            if (string.IsNullOrWhiteSpace(stationClient) || string.IsNullOrWhiteSpace(stationCuisinier))
            {
                Console.WriteLine("Impossible de déterminer les stations pour le trajet.");
                return;
            }

            /// Calcul du trajet avec Dijkstra
            var graphe = new Graphe<string>();
            graphe.ChargerDepuisCSV("MetroParis.csv", "MetroParisLiaison.csv");

            var noeudDepart = graphe.ObtenirPremiereCorrespondance(stationCuisinier);
            var noeudArrivee = graphe.ObtenirPremiereCorrespondance(stationClient);

            int dureeTrajet = -1;
            if (noeudDepart != null && noeudArrivee != null)
            {
                var chemin = graphe.Dijkstra(noeudDepart, noeudArrivee);
                if (chemin != null)
                    dureeTrajet = graphe.CalculerCoutTotal(chemin);
            }

            Console.WriteLine($"\nCommande résumée : {quantite} x Plat #{idPlat} = {total}€");
            if (dureeTrajet >= 0)
                Console.WriteLine($"Temps estimé entre cuisinier et client : {dureeTrajet} minutes.");
            else
                Console.WriteLine("Temps de trajet non disponible.");

        }
    }


    private static int RecupererProchainId(MySqlConnection conn, string table, string colonneId) /// Recupere le prochain ID dans une table
    {
        var cmd = new MySqlCommand($"SELECT MAX({colonneId}) FROM {table}", conn);
        var result = cmd.ExecuteScalar();
        return (result != DBNull.Value) ? Convert.ToInt32(result) + 1 : 1;
    }

    private static string GenererNouvelIdLivraison(MySqlConnection conn) /// Génère un nouvel identifiant pour une livraison de type LIV###
    {
        var cmd = new MySqlCommand("SELECT id_livraison FROM livraison ORDER BY id_livraison DESC LIMIT 1", conn);
        var last = cmd.ExecuteScalar()?.ToString();
        int number = (last != null && last.StartsWith("LIV")) ? int.Parse(last[3..]) + 1 : 1;
        return $"LIV{number:D3}";
    }




    private static void AfficherAvisClient(int idClient)  /// Affiche tous les avis donnés par un client 
    {
        using (var conn = new MySqlConnection(ConnectionString))
        {
            conn.Open();
            var cmd = new MySqlCommand("SELECT note, commentaire, date_avis FROM Avis WHERE id_client = @id", conn);
            cmd.Parameters.AddWithValue("@id", idClient);
            using (var reader = cmd.ExecuteReader())
            {
                Console.WriteLine("\n Vos avis :\n");
                while (reader.Read())
                {
                    Console.WriteLine($"Note : {reader["note"]}/5 | Commentaire : {reader["commentaire"]} | Date : {reader["date_avis"]}");
                }
            }
        }
    }

    private static void AfficherTransactionsClient(int idClient) /// Affiche l’historique des transactions d’un client donné.
    {
        using (var conn = new MySqlConnection(ConnectionString))
        {
            conn.Open();
            var cmd = new MySqlCommand(@"
                SELECT t.id_transaction, t.mode_paiement, t.statut_paiement, c.montant_commande
                FROM Transaction t
                JOIN Commande c ON t.id_commande = c.id_commande
                WHERE c.id_client = @id", conn);
            cmd.Parameters.AddWithValue("@id", idClient);
            using (var reader = cmd.ExecuteReader())
            {
                Console.WriteLine("\n Historique des transactions :\n");
                while (reader.Read())
                {
                    Console.WriteLine($"Transaction ID : {reader["id_transaction"]} | Montant : {reader["montant_commande"]} | Paiement : {reader["mode_paiement"]} ({reader["statut_paiement"]})");
                }
            }
        }
    }
}

