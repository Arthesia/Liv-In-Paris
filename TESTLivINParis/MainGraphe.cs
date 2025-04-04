using LeonardOzanTimothe2ndRenduGRAPHE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TESTLivINParis
{
    internal class MainGraphe
    {
        /// Lance la simulation interactive du graphe pour visualisation ou calcul de trajet
        public static void LancerSimulationGraphe()
        {
            /// On indique ou se trouvent les fichiers CSV des stations et des lisaisons
            string cheminStations = "MetroParis.csv";
            string cheminLiaisons = "MetroParisLiaison.csv";

            /// On crée le graphe et on le remplit avec les données des fichiers
            var graphe = new Graphe<string>();
            graphe.ChargerDepuisCSV(cheminStations, cheminLiaisons);

            /// Affichage de la liste d'adjacence (utile pour voir si le graphe est bien construit)
            Console.WriteLine("Liste d'adjacence générée :\n");
            graphe.Afficher();

            /// Boucle principale du programme
            while (true)
            {
                /// L'utilisateur peut choisir d’afficher le graphe ou de chercher un chemin
                Console.WriteLine("\nTapez 'visu' pour visualiser le graphe ou appuyez sur Entrée pour continuer avec le calcul de chemin :");
                string option = Console.ReadLine();
                if (option.ToLower() == "visu")
                {
                    try
                    {
                        string cheminImage = "graphe.png";
                        /// On supprime l’ancienne image si elle existe
                        if (File.Exists(cheminImage)) File.Delete(cheminImage);

                        /// On crée une nouvelle image du graphe
                        var visualiseur = new VisualiseurGraphe<string>(graphe);
                        visualiseur.DessinerGraphe(cheminImage);

                        /// On ouvre automatiquement l’image générée
                        Console.WriteLine($"Le graphe a été sauvegardé dans '{cheminImage}'.");
                        Process.Start(new ProcessStartInfo(cheminImage) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erreur lors de la visualisation : " + ex.Message);
                    }
                    continue;
                }

                /// Saisie des stations de départ et d'arrivée
                Console.WriteLine("\nEntrez le nom simple de la station de départ (ex : Nation) ou 'exit' :");
                string stationDepartInput = Console.ReadLine();
                if (stationDepartInput.ToLower() == "exit") break;

                Console.WriteLine("Entrez le nom simple de la station d'arrivée (ex : Bercy) ou 'exit' :");
                string stationArriveeInput = Console.ReadLine();
                if (stationArriveeInput.ToLower() == "exit") break;

                /// Récupération des nœuds correspondant aux stations
                var noeudDepart = graphe.ObtenirPremiereCorrespondance(stationDepartInput);
                var noeudArrivee = graphe.ObtenirPremiereCorrespondance(stationArriveeInput);

                /// Vérifie que les deux stations existent
                if (noeudDepart == null || noeudArrivee == null)
                {
                    Console.WriteLine("Station de départ ou d'arrivée introuvable.");
                    continue;
                }

                /// Choix de l’algorithme à utiliser
                Console.WriteLine("Choisissez l'algorithme à utiliser :");
                Console.WriteLine("1 - Dijkstra");
                Console.WriteLine("2 - Bellman-Ford");
                Console.WriteLine("3 - Floyd-Warshall");
                string choixAlgo = Console.ReadLine();

                /// Initialisation des variables de sortie
                List<Noeud<string>> cheminOptimal = null;
                int coutTotal = 0;

                /// Application de l’algorithme sélectionné
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

                /// Affichage du chemin trouvé et de sa durée
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
