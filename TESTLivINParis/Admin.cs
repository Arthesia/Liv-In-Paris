using MySql.Data.MySqlClient;
/// LeonardOzanTimothe2ndRendu

public static class AdminService
{

    public static string ConnectionString = "server=localhost;port=3306;user=root;password=1738;database=livinparis;";


    /// Identifiants définis en dur

    static string adminUsername = "admin";
    static string adminPassword = "admin";

    public static void ConnexionAdmin() /// Gère la connexion d’un administrateur via identifiants en dur avec les identifiants définis au dessus.
    {
        Console.WriteLine("\n--- Connexion Admin ---");

        Console.Write("Nom d'utilisateur : ");
        string username = Console.ReadLine();

        Console.Write("Mot de passe : ");
        string password = Console.ReadLine();

        if (username == adminUsername && password == adminPassword)
        {
            Console.WriteLine("\n Connexion administrateur réussie !");
            MenuAdmin();
        }
        else
        {
            Console.WriteLine("\n Identifiants administrateur incorrects.");
        }
    }

    public static void MenuAdmin() /// Affiche le menu d'administration avec plusieurs options de gestion des utilisateurs.
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Menu Administrateur ===\n");
            Console.WriteLine("1. Lister tous les utilisateurs");
            Console.WriteLine("2. Ajouter un utilisateur");
            Console.WriteLine("3. Modifier un utilisateur");
            Console.WriteLine("4. Supprimer un utilisateur");
            Console.WriteLine("5. Afficher les utilisateurs triés");
            Console.WriteLine("0. Retour\n");
            Console.Write("Votre choix : ");
            string choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    ListerUtilisateurs();
                    break;
                case "2":
                    AjouterUtilisateurAvecRoles();
                    break;
                case "3":
                    ModifierUtilisateur();
                    break;
                case "4":
                    SupprimerUtilisateur();
                    break;
                case "5":
                    AfficherUtilisateursTries();
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

    public static void ListerUtilisateurs() /// Affiche la liste de tous les utilisateurs présents dans la base.
    {
        using (var conn = new MySqlConnection(ConnectionString))
        {
            conn.Open();

            string query = "SELECT id_utilisateur, nom, prenom, telephone FROM Utilisateur";

            using (var cmd = new MySqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                Console.WriteLine("\n Liste des utilisateurs :\n");

                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["id_utilisateur"]} | {reader["nom"]} {reader["prenom"]} - {reader["telephone"]}");
                }
            }
        }
    }

    public static void AjouterUtilisateurAvecRoles() /// Permet d'ajouter un nouvel utilisateur avec touttes les données nécessaires et avec le rôle donc client, cuisinier ou les deux
    {
        Console.WriteLine("\n--- Ajouter un utilisateur ---");

        Console.Write("Nom : ");
        string nom = Console.ReadLine();
        Console.Write("Prénom : ");
        string prenom = Console.ReadLine();
        Console.Write("Adresse : ");
        string adresse = Console.ReadLine();
        Console.Write("Station de métro : ");
        string station = Console.ReadLine();
        Console.Write("Téléphone : ");
        string tel = Console.ReadLine();
        Console.Write("Mot de passe : ");
        string mdp = Console.ReadLine();

        using (var conn = new MySqlConnection(ConnectionString))
        {
            conn.Open();

            int newId = Convert.ToInt32(new MySqlCommand("SELECT IFNULL(MAX(id_utilisateur), 0) + 1 FROM Utilisateur", conn).ExecuteScalar());

            /// Insère l'utilisateur dans la table principale
            var insertUser = new MySqlCommand(
                @"INSERT INTO Utilisateur (id_utilisateur, nom, prenom, adresse, station_metro, telephone, est_radie, mot_de_passe)
                  VALUES (@id, @nom, @prenom, @adresse, @station, @tel, false, @mdp)", conn);
            insertUser.Parameters.AddWithValue("@id", newId);
            insertUser.Parameters.AddWithValue("@nom", nom);
            insertUser.Parameters.AddWithValue("@prenom", prenom);
            insertUser.Parameters.AddWithValue("@adresse", adresse);
            insertUser.Parameters.AddWithValue("@station", station);
            insertUser.Parameters.AddWithValue("@tel", tel);
            insertUser.Parameters.AddWithValue("@mdp", mdp);
            insertUser.ExecuteNonQuery();

            Console.WriteLine("\nRôles à assigner :");
            Console.WriteLine("1. Client");
            Console.WriteLine("2. Cuisinier");
            Console.WriteLine("3. Les deux");
            Console.Write("Votre choix : ");
            string role = Console.ReadLine();

            /// Ajout du rôle Client
            if (role == "1" || role == "3")
            {
                Console.Write("Type de client : ");
                string type = Console.ReadLine();
                Console.Write("Nom du référent (optionnel) : ");
                string referent = Console.ReadLine();

                var cmdClient = new MySqlCommand("INSERT INTO Client VALUES (@id, @type, @ref)", conn);
                cmdClient.Parameters.AddWithValue("@id", newId);
                cmdClient.Parameters.AddWithValue("@type", type);
                cmdClient.Parameters.AddWithValue("@ref", string.IsNullOrWhiteSpace(referent) ? null : referent);
                cmdClient.ExecuteNonQuery();
            }

            /// Ajout du rôle Cuisinier
            if (role == "2" || role == "3")
            {
                Console.Write("Email du cuisinier : ");
                string email = Console.ReadLine();

                var cmdCuisinier = new MySqlCommand("INSERT INTO Cuisinier VALUES (@id, @email)", conn);
                cmdCuisinier.Parameters.AddWithValue("@id", newId);
                cmdCuisinier.Parameters.AddWithValue("@email", email);
                cmdCuisinier.ExecuteNonQuery();
            }

            Console.WriteLine($" Utilisateur ajouté avec ID {newId}.");
        }
    }

    public static void SupprimerUtilisateur() /// Supprime un utilisateur de la base de données grâce à son ID.
    {
        Console.Write("\nID de l'utilisateur à supprimer : ");
        string id = Console.ReadLine();

        using (var conn = new MySqlConnection(ConnectionString))
        {
            conn.Open();

            var del1 = new MySqlCommand("DELETE FROM Client WHERE id_utilisateur = @id", conn);
            var del2 = new MySqlCommand("DELETE FROM Cuisinier WHERE id_utilisateur = @id", conn);
            var del3 = new MySqlCommand("DELETE FROM Utilisateur WHERE id_utilisateur = @id", conn);

            del1.Parameters.AddWithValue("@id", id);
            del2.Parameters.AddWithValue("@id", id);
            del3.Parameters.AddWithValue("@id", id);

            del1.ExecuteNonQuery();
            del2.ExecuteNonQuery();
            int res = del3.ExecuteNonQuery();

            Console.WriteLine(res > 0 ? " Utilisateur supprimé." : " Utilisateur introuvable.");
        }
    }

    public static void ModifierUtilisateur()  /// Permet à l’administrateur de modifier toutes les informations d'un utilisateur
    {
        Console.Write("\nID de l'utilisateur à modifier : ");
        string id = Console.ReadLine();

        Console.Write("Nouveau nom : ");
        string nom = Console.ReadLine();

        Console.Write("Nouveau prénom : ");
        string prenom = Console.ReadLine();

        Console.Write("Nouvelle adresse : ");
        string adresse = Console.ReadLine();

        Console.Write("Nouvelle station de métro : ");
        string station = Console.ReadLine();

        Console.Write("Nouveau téléphone : ");
        string tel = Console.ReadLine();

        Console.Write("Nouveau mot de passe : ");
        string mdp = Console.ReadLine();

        Console.Write("Est-ce que l'utilisateur est radié ? (true / false) : ");
        string radieInput = Console.ReadLine();
        bool estRadie = radieInput.Trim().ToLower() == "true";

        using (var conn = new MySqlConnection(ConnectionString))
        {
            conn.Open();

            string update = @"
            UPDATE Utilisateur SET 
                nom = @nom,
                prenom = @prenom,
                adresse = @adresse,
                station_metro = @station,
                telephone = @tel,
                mot_de_passe = @mdp,
                est_radie = @radie
            WHERE id_utilisateur = @id";

            var cmd = new MySqlCommand(update, conn);
            cmd.Parameters.AddWithValue("@nom", nom);
            cmd.Parameters.AddWithValue("@prenom", prenom);
            cmd.Parameters.AddWithValue("@adresse", adresse);
            cmd.Parameters.AddWithValue("@station", station);
            cmd.Parameters.AddWithValue("@tel", tel);
            cmd.Parameters.AddWithValue("@mdp", mdp);
            cmd.Parameters.AddWithValue("@radie", estRadie);
            cmd.Parameters.AddWithValue("@id", id);

            int res = cmd.ExecuteNonQuery();
            Console.WriteLine(res > 0 ? " Utilisateur modifié avec succès." : " Aucun utilisateur trouvé avec cet ID.");
        }
    }


    public static void ModifierUtilisateurParClient(int id) /// Permet à un utilisateur de modifier ses propres infos...
    {
        Console.WriteLine("\n=== Modifier mes informations ===");

        Console.Write("Nouveau nom : ");
        string nom = Console.ReadLine();

        Console.Write("Nouveau prénom : ");
        string prenom = Console.ReadLine();

        Console.Write("Nouvelle adresse : ");
        string adresse = Console.ReadLine();

        Console.Write("Nouvelle station de métro : ");
        string station = Console.ReadLine();

        Console.Write("Nouveau téléphone : ");
        string tel = Console.ReadLine();

        Console.Write("Nouveau mot de passe : ");
        string mdp = Console.ReadLine();

        using (var conn = new MySqlConnection(ConnectionString))
        {
            conn.Open();

            string update = @"
            UPDATE Utilisateur SET 
                nom = @nom,
                prenom = @prenom,
                adresse = @adresse,
                station_metro = @station,
                telephone = @tel,
                mot_de_passe = @mdp
            WHERE id_utilisateur = @id";

            var cmd = new MySqlCommand(update, conn);
            cmd.Parameters.AddWithValue("@nom", nom);
            cmd.Parameters.AddWithValue("@prenom", prenom);
            cmd.Parameters.AddWithValue("@adresse", adresse);
            cmd.Parameters.AddWithValue("@station", station);
            cmd.Parameters.AddWithValue("@tel", tel);
            cmd.Parameters.AddWithValue("@mdp", mdp);
            cmd.Parameters.AddWithValue("@id", id);

            int res = cmd.ExecuteNonQuery();
            Console.WriteLine(res > 0 ? " Informations mises à jour." : " Échec de la mise à jour.");
        }
    }
    public static void AfficherUtilisateursTries() /// Affiche les utilisateurs triés selon un critère choisi (nom, adresse, ou total d’achats).
    {
        Console.WriteLine("\n=== Affichage Trié des Utilisateurs ===");
        Console.WriteLine("1. Trier par nom");
        Console.WriteLine("2. Trier par adresse (rue)");
        Console.WriteLine("3. Trier par montant cumulé des achats (clients uniquement)");
        Console.Write("Votre choix : ");
        string choix = Console.ReadLine();

        string query = "";

        if (choix == "1")
        {
            query = "SELECT id_utilisateur, nom, prenom, adresse, station_metro FROM Utilisateur ORDER BY nom ASC";
        }
        else if (choix == "2")
        {
            query = "SELECT id_utilisateur, nom, prenom, adresse, station_metro FROM Utilisateur ORDER BY adresse ASC";
        }
        else if (choix == "3")
        {
            query = @"
            SELECT u.id_utilisateur, u.nom, u.prenom, u.adresse, u.station_metro, 
                   IFNULL(SUM(c.montant_commande), 0) AS total_achats
            FROM Utilisateur u
            JOIN Client cl ON u.id_utilisateur = cl.id_utilisateur
            LEFT JOIN Commande c ON cl.id_utilisateur = c.id_client
            GROUP BY u.id_utilisateur, u.nom, u.prenom, u.adresse, u.station_metro
            ORDER BY total_achats DESC";
        }
        else
        {
            Console.WriteLine("Choix invalide.");
            return;
        }

        using (var conn = new MySqlConnection(ConnectionString))
        {
            conn.Open();

            using (var cmd = new MySqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                Console.WriteLine("\n Liste triée des utilisateurs :\n");

                while (reader.Read())
                {
                    string ligne = $"ID: {reader["id_utilisateur"]} | {reader["nom"]} {reader["prenom"]} | Adresse: {reader["adresse"]} | Station: {reader["station_metro"]}";

                    if (choix == "3")
                    {
                        ligne += $" |  Total achats : {reader["total_achats"]}€";
                    }

                    Console.WriteLine(ligne);
                }
            }
        }

        Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
        Console.ReadKey();
    }



}

