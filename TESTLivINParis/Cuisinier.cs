using MySql.Data.MySqlClient;
/// LeonardOzanTimothe2ndRendu

public static class CuisinierService
{
    private static string connectionString = AdminService.ConnectionString;

    public static void MenuCuisinier(int id) /// Menu qui est affiché lorsqu'un cuisinier se connecte
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Menu Cuisinier ===");
            Console.WriteLine("1. Voir mes clients servis");
            Console.WriteLine("2. Voir mes plats vendus");
            Console.WriteLine("3. Gérer mon compte");
            Console.WriteLine("0. Déconnexion\n");
            Console.Write("Votre choix : ");
            string choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    AfficherClientsServis(id);
                    break;
                case "2":
                    AfficherPlatsVendus(id);
                    break;
                case "3":
                    GérerMonCompte(id);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine(" Choix invalide !");
                    break;
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }


    private static void AfficherClientsServis(int idCuisinier) /// Affiche les clients qui ont été servis par un cuisinier donné
    {
        using var conn = new MySqlConnection(connectionString);
        conn.Open();

        var cmd = new MySqlCommand(@"
            SELECT DISTINCT u.id_utilisateur, u.nom, u.prenom
            FROM LigneCommande lc
            JOIN Commande c ON c.id_commande = lc.id_commande
            JOIN Utilisateur u ON c.id_client = u.id_utilisateur
            JOIN Plat p ON lc.id_plat = p.id_plat
            JOIN Propose pr ON pr.id_plat = p.id_plat
            WHERE pr.id_cuisinier = @id", conn);

        cmd.Parameters.AddWithValue("@id", idCuisinier);

        using var reader = cmd.ExecuteReader();

        Console.WriteLine("\n Clients que vous avez servis :\n");
        while (reader.Read())
        {
            Console.WriteLine($"- {reader["nom"]} {reader["prenom"]} (ID : {reader["id_utilisateur"]})");
        }
    }

    private static void AfficherPlatsVendus(int idCuisinier) /// Affiche les plats vendus par le cuisinier avec les quantités totales
    {
        using var conn = new MySqlConnection(connectionString);
        conn.Open();

        var cmd = new MySqlCommand(@"
            SELECT p.nom_plat, SUM(lc.quantite) AS total_vendu
            FROM Plat p
            JOIN Propose pr ON pr.id_plat = p.id_plat
            JOIN LigneCommande lc ON lc.id_plat = p.id_plat
            JOIN Commande c ON c.id_commande = lc.id_commande
            WHERE pr.id_cuisinier = @id
            GROUP BY p.nom_plat", conn);

        cmd.Parameters.AddWithValue("@id", idCuisinier);

        using var reader = cmd.ExecuteReader();

        Console.WriteLine("\n🍽 Plats que vous avez vendus :\n");
        while (reader.Read())
        {
            Console.WriteLine($"- {reader["nom_plat"]} | Quantité vendue : {reader["total_vendu"]}");
        }
    }

    private static void GérerMonCompte(int id) /// Permet au cuisinier de modifier les infos de son compte ou de le supprimer
    {
        Console.WriteLine("\n=== Gérer mon compte ===");
        Console.WriteLine("1. Modifier mes informations");
        Console.WriteLine("2. Supprimer mon compte");
        Console.Write("Votre choix : ");
        string choix = Console.ReadLine();

        if (choix == "1")
        {
            AdminService.ModifierUtilisateurParClient(id); /// Réutilise la méthode admin pour modifier les infos d’un utilisateur
        }
        else if (choix == "2")
        {
            Console.Write("❗ Confirmez-vous la suppression de votre compte ? (oui/non) : ");  /// Demande une confirmation avant suppression
            string confirm = Console.ReadLine().ToLower();
            if (confirm == "oui")
            {
                using var conn = new MySqlConnection(AdminService.ConnectionString);
                conn.Open();

                var del1 = new MySqlCommand("DELETE FROM Cuisinier WHERE id_utilisateur = @id", conn);
                var del2 = new MySqlCommand("DELETE FROM Utilisateur WHERE id_utilisateur = @id", conn);
                del1.Parameters.AddWithValue("@id", id);
                del2.Parameters.AddWithValue("@id", id);

                del1.ExecuteNonQuery();
                int res = del2.ExecuteNonQuery();

                Console.WriteLine(res > 0 ? " Compte supprimé avec succès." : " Échec de la suppression.");
            }
            else
            {
                Console.WriteLine(" Suppression annulée.");
            }
        }
        else
        {
            Console.WriteLine(" Choix invalide.");
        }
    }
}
