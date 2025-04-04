using System.Diagnostics;
// ...

namespace LeonardOzanTimothe2ndRenduGRAPHE
{
    public class Program
    {
        public static void Main()
        {
            string cheminStations = "MetroParis.csv";
            string cheminLiaisons = "MetroParisLiaison.csv";

            var graphe = new Graphe<string>();
            graphe.ChargerDepuisCSV(cheminStations, cheminLiaisons);

            Console.WriteLine("Liste d'adjacence générée :\n");
            graphe.Afficher();

            while (true)
            {
                // Option pour visualiser le graphe
                Console.WriteLine("\nTapez 'visu' pour visualiser le graphe ou appuyez sur Entrée pour continuer avec le calcul de chemin :");
                string option = Console.ReadLine();
                if (option.ToLower() == "visu")
                {
                    try
                    {
                        string cheminImage = "graphe.png";
                        // Supprimer l'ancien fichier s'il existe
                        if (File.Exists(cheminImage))
                        {
                            File.Delete(cheminImage);
                        }
                        // Créer et sauvegarder le nouveau graphe
                        var visualiseur = new VisualiseurGraphe<string>(graphe);

                        visualiseur.DessinerGraphe(cheminImage);
                        Console.WriteLine("Le graphe a été sauvegardé dans '{0}'.", cheminImage);
                        // Ouvrir automatiquement le fichier image avec l'application par défaut
                        var infoProcess = new ProcessStartInfo(cheminImage)
                        {
                            UseShellExecute = true
                        };
                        Process.Start(infoProcess);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erreur lors de la visualisation : " + ex.Message);
                    }
                    continue;
                }

                Console.WriteLine("\nEntrez le nom simple de la station de départ (ex : Nation) ou 'exit' :");
                string stationDepartInput = Console.ReadLine();
                if (stationDepartInput.ToLower() == "exit") break;

                Console.WriteLine("Entrez le nom simple de la station d'arrivée (ex : Bercy) ou 'exit' :");
                string stationArriveeInput = Console.ReadLine();
                if (stationArriveeInput.ToLower() == "exit") break;

                var noeudDepart = graphe.ObtenirPremiereCorrespondance(stationDepartInput);
                var noeudArrivee = graphe.ObtenirPremiereCorrespondance(stationArriveeInput);

                if (noeudDepart == null || noeudArrivee == null)
                {
                    Console.WriteLine("Station de départ ou d'arrivée introuvable.");
                    continue;
                }

                Console.WriteLine("Choisissez l'algorithme à utiliser :");
                Console.WriteLine("1 - Dijkstra");
                Console.WriteLine("2 - Bellman-Ford");
                Console.WriteLine("3 - Floyd-Warshall");
                string choixAlgo = Console.ReadLine();

                List<Noeud<string>> cheminOptimal = null;
                int coutTotal = 0;

                switch (choixAlgo)
                {
                    case "1":
                        cheminOptimal = graphe.Dijkstra(noeudDepart, noeudArrivee);
                        if (cheminOptimal != null)
                            coutTotal = graphe.CalculerCoutTotal(cheminOptimal);
                        break;
                    case "2":
                        cheminOptimal = graphe.BellmanFord(noeudDepart, noeudArrivee);
                        if (cheminOptimal != null)
                            coutTotal = graphe.CalculerCoutTotal(cheminOptimal);
                        break;
                    case "3":
                        cheminOptimal = graphe.FloydWarshall(noeudDepart, noeudArrivee, out coutTotal);
                        break;
                    default:
                        Console.WriteLine("Choix invalide.");
                        continue;
                }

                if (cheminOptimal != null)
                {
                    Console.WriteLine("\nChemin le plus court trouvé :");
                    foreach (var noeud in cheminOptimal)
                        Console.WriteLine(noeud.Id);

                    Console.WriteLine($"\nTemps total du trajet : {coutTotal} min");
                }
                else
                {
                    Console.WriteLine("\nAucun chemin trouvé entre les deux stations.");
                }
            }
        }
    }
}
