using MySql.Data.MySqlClient;
/// LeonardOzanTimothe2ndRendu

public static class Utilisateur
{
    static string connectionString = AdminService.ConnectionString;

    public static void Connexion() /// Permet à un utilisateur de se connecter en donnant son ID et son mot de passe
    {
        Console.WriteLine("\n--- Connexion ---");
        Console.Write("ID utilisateur : ");
        string idInput = Console.ReadLine();
        Console.Write("Mot de passe : ");
        string mdpInput = Console.ReadLine();

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            string query = "SELECT * FROM Utilisateur WHERE id_utilisateur = @id AND mot_de_passe = @mdp";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", idInput);
            cmd.Parameters.AddWithValue("@mdp", mdpInput);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine("\n Connexion réussie !"); /// Connexion réussie demande à l'utilisateur s'il veut se connecter en tant que cuisinier ou client s'il a les deux rôles
                    int id = int.Parse(idInput);
                    GérerRôleUtilisateur(id);
                }
                else
                {
                    Console.WriteLine("\n Identifiants incorrects.");
                }
            }
        }
    }

    public static void CreerCompte() /// Permet de créer un compte et de donner ses informations et d'attribuer un rôles ou les deux...
    {
        Console.WriteLine("\n--- Création de compte ---");

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

        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            int newId = Convert.ToInt32(new MySqlCommand("SELECT IFNULL(MAX(id_utilisateur), 0) + 1 FROM Utilisateur", conn).ExecuteScalar()); /// On génère un nouvel ID utilisateur

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

            Console.WriteLine($"\n Compte créé avec ID {newId}");
            GérerCréationDeRôle(conn, newId);
        }
    }

    private static void GérerCréationDeRôle(MySqlConnection conn, int id) /// Associe les rôles (Client / Cuisinier) à un utilisateur après création de compte
    {
        Console.WriteLine("\nSouhaitez-vous être :");
        Console.WriteLine("1. Client");
        Console.WriteLine("2. Cuisinier");
        Console.WriteLine("3. Les deux");
        Console.Write("Votre choix : ");
        string role = Console.ReadLine();

        if (role == "1" || role == "3")
        {
            Console.Write("Type de client (Particulier / Entreprise) : ");
            string type = Console.ReadLine();
            Console.Write("Nom du référent (laisser vide si particulier) : ");
            string referent = Console.ReadLine();

            /// Insertion dans la table Client
            var cmdClient = new MySqlCommand("INSERT INTO Client VALUES (@id, @type, @ref)", conn);
            cmdClient.Parameters.AddWithValue("@id", id);
            cmdClient.Parameters.AddWithValue("@type", type);
            cmdClient.Parameters.AddWithValue("@ref", string.IsNullOrWhiteSpace(referent) ? null : referent);
            cmdClient.ExecuteNonQuery();
        }

        if (role == "2" || role == "3")
        {
            Console.Write("Email du cuisinier : ");
            string email = Console.ReadLine();

            /// Insertion dans la table Cuisinier
            var cmdCuisinier = new MySqlCommand("INSERT INTO Cuisinier VALUES (@id, @email)", conn);
            cmdCuisinier.Parameters.AddWithValue("@id", id);
            cmdCuisinier.Parameters.AddWithValue("@email", email);
            cmdCuisinier.ExecuteNonQuery();
        }
    }

    private static void GérerRôleUtilisateur(int id) /// Permet à l'utilisateur connecté d'accéder à son espace Client ou Cuisinier selon son rôle
    {
        using (var conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            bool estClient = false, estCuisinier = false;

            /// Vérifie si l'utilisateur est un client
            var cmd1 = new MySqlCommand("SELECT 1 FROM Client WHERE id_utilisateur = @id", conn);
            cmd1.Parameters.AddWithValue("@id", id);
            estClient = cmd1.ExecuteScalar() != null;

            /// Vérifie si l'utilisateur est un cuisinier
            var cmd2 = new MySqlCommand("SELECT 1 FROM Cuisinier WHERE id_utilisateur = @id", conn);
            cmd2.Parameters.AddWithValue("@id", id);
            estCuisinier = cmd2.ExecuteScalar() != null;

            Console.WriteLine("\n Vous avez accès aux rôles suivants :");
            if (estClient) Console.WriteLine(" - Client");
            if (estCuisinier) Console.WriteLine(" - Cuisinier");
            if (!estClient && !estCuisinier)
            {
                Console.WriteLine("Aucun rôle défini dans la BDD.");
                return;
            }

            if (estClient && estCuisinier) /// Si l'utilisateur a les deux rôles, il choisit entre Client ou Cuisinier
            {
                Console.WriteLine("\nSouhaitez-vous entrer en tant que :");
                Console.WriteLine("1. Client");
                Console.WriteLine("2. Cuisinier");
                Console.Write("Votre choix : ");
                string choix = Console.ReadLine();

                if (choix == "1") ClientService.MenuClient(id);
                else if (choix == "2") CuisinierService.MenuCuisinier(id);
            }
            else if (estClient)
            {
                ClientService.MenuClient(id);
            }
            else if (estCuisinier)
            {
                CuisinierService.MenuCuisinier(id);
            }
        }
    }


}
