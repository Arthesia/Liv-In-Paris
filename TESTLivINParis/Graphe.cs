namespace LeonardOzanTimothe2ndRenduGRAPHE
{
    public class Graphe<T>
    {
        /// Représente la liste d’adjacence du graphe, chaque nœud est lié à une liste de voisins.
        public Dictionary<Noeud<T>, List<Lien<T>>> ListeAdjacence { get; set; } = new();

        /// Permet d’ajouter un lien (arc) entre deux stations avec un certain poids (durée ou distance).
        public void AjouterArc(Noeud<T> source, Noeud<T> destination, int poids)
        {
            if (!ListeAdjacence.ContainsKey(source))
                ListeAdjacence[source] = new List<Lien<T>>();

            ListeAdjacence[source].Add(new Lien<T>(destination, poids));
        }

        /// Affiche le graphe dans la console : les stations et leurs correspondances ou trajets directs.
        public void Afficher()
        {
            foreach (var element in ListeAdjacence)
            {
                string nomSource = element.Key.Id.ToString();
                string nomDeBase = nomSource.Split('_')[0];
                Console.WriteLine($"\nStation : {nomSource}");

                List<Lien<T>> correspondances = new List<Lien<T>>();
                List<Lien<T>> trajets = new List<Lien<T>>();

                foreach (var lien in element.Value)
                {
                    string nomDestination = lien.Destination.Id.ToString();
                    string baseDestination = nomDestination.Split('_')[0];

                    if (baseDestination == nomDeBase)
                        correspondances.Add(lien);
                    else
                        trajets.Add(lien);
                }

                if (trajets.Count > 0)
                {
                    Console.WriteLine("  - Liaisons directes :");
                    foreach (var lien in trajets)
                        Console.WriteLine($"     - {lien.Destination.Id} (temps : {lien.Poids} min)");
                }

                if (correspondances.Count > 0)
                {
                    Console.WriteLine("  - Correspondances :");
                    foreach (var lien in correspondances)
                        Console.WriteLine($"     - {lien.Destination.Id} (changement : {lien.Poids} min)");
                }
            }
        }

        /// Cette méthode lit les deux fichiers CSV (stations et liaisons) et construit le graphe à partir de ça.
        public void ChargerDepuisCSV(string cheminStations, string cheminLiaisons)
        {
            var stations = new Dictionary<string, string>();
            var correspondances = new Dictionary<string, List<string>>();

            /// Liste des liaisons spéciales qui sont à sens unique (ligne 7bis)
            var liaisonsSensUnique = new HashSet<(string, string)>
            {
                ("Botzaris_7bis", "Place des Fêtes_7bis"),
                ("Place des Fêtes_7bis", "Pré-Saint-Gervais_7bis"),
                ("Pré-Saint-Gervais_7bis", "Danube_7bis"),
                ("Danube_7bis", "Botzaris_7bis"),
                ("Botzaris_7bis", "Buttes Chaumont_7bis")
            };

            /// Lecture du fichier des stations
            string[] lignesStations = File.ReadAllLines(cheminStations);
            for (int i = 1; i < lignesStations.Length; i++)
            {
                var champs = lignesStations[i].Split(',');
                string id = champs[0].Trim();
                string ligne = champs[1].Trim();
                string nom = champs[2].Trim();
                string nomComplet = $"{nom}_{ligne}";

                stations[id] = nomComplet;

                if (!correspondances.ContainsKey(nom))
                    correspondances[nom] = new List<string>();

                if (!correspondances[nom].Contains(nomComplet))
                    correspondances[nom].Add(nomComplet);
            }

            /// Lecture du fichier des liaisons
            string[] lignesLiaisons = File.ReadAllLines(cheminLiaisons);
            for (int i = 1; i < lignesLiaisons.Length; i++)
            {
                var champs = lignesLiaisons[i].Split(',');
                string idStation = champs[0].Trim();
                string stationSuivante = champs[3].Trim();
                string poidsChaine = champs[4].Trim();
                string changementChaine = champs.Length > 5 ? champs[5].Trim() : "";

                if (!stations.ContainsKey(idStation))
                    continue;

                string stationSource = stations[idStation];
                var noeudSource = new Noeud<T>((T)(object)stationSource);

                if (!string.IsNullOrWhiteSpace(stationSuivante) && stations.ContainsKey(stationSuivante))
                {
                    string stationDestination = stations[stationSuivante];
                    var noeudDestination = new Noeud<T>((T)(object)stationDestination);
                    int poids = int.TryParse(poidsChaine, out int valeur) ? valeur : 0;

                    AjouterArc(noeudSource, noeudDestination, poids);

                    var arc = (stationSource, stationDestination);
                    if (!liaisonsSensUnique.Contains(arc))
                        AjouterArc(noeudDestination, noeudSource, poids);
                }

                /// Si changement de ligne à la même station, on ajoute aussi les correspondances internes
                if (!string.IsNullOrWhiteSpace(changementChaine) && int.TryParse(changementChaine, out int tempsChangement))
                {
                    string stationPure = stationSource.Split('_')[0];
                    foreach (var autre in correspondances[stationPure])
                    {
                        if (autre != stationSource)
                        {
                            var autreNoeud = new Noeud<T>((T)(object)autre);
                            AjouterArc(noeudSource, autreNoeud, tempsChangement);
                            AjouterArc(autreNoeud, noeudSource, tempsChangement);
                        }
                    }
                }
            }
        }

        /// Implémentation simple de l’algorithme de Dijkstra pour trouver le chemin le plus court entre 2 stations.
        public List<Noeud<T>> Dijkstra(Noeud<T> depart, Noeud<T> arrivee)
        {
            var distance = new Dictionary<Noeud<T>, int>();
            var precedent = new Dictionary<Noeud<T>, Noeud<T>>();
            var nonVisites = new List<Noeud<T>>();

            foreach (var noeud in ListeAdjacence.Keys)
            {
                distance[noeud] = int.MaxValue;
                precedent[noeud] = null;
                nonVisites.Add(noeud);
            }

            if (!distance.ContainsKey(depart))
                return null;
            distance[depart] = 0;

            while (nonVisites.Count > 0)
            {
                Noeud<T> courant = nonVisites.OrderBy(n => distance[n]).First();
                nonVisites.Remove(courant);

                if (courant.Equals(arrivee))
                    break;
                if (distance[courant] == int.MaxValue)
                    break;

                foreach (var arc in ListeAdjacence[courant])
                {
                    Noeud<T> voisin = arc.Destination;
                    int alt = distance[courant] + arc.Poids;
                    if (alt < distance[voisin])
                    {
                        distance[voisin] = alt;
                        precedent[voisin] = courant;
                    }
                }
            }

            var chemin = new List<Noeud<T>>();
            var temp = arrivee;
            while (temp != null)
            {
                chemin.Add(temp);
                temp = precedent[temp];
            }
            chemin.Reverse();

            if (chemin.Count == 0 || !chemin[0].Equals(depart))
                return null;
            return chemin;
        }

        /// Cette méthode retourne tous les nœuds du graphe (utile pour les algos qui ont besoin de la liste complète).
        public HashSet<Noeud<T>> RecupererTousLesNoeuds()
        {
            var ensemble = new HashSet<Noeud<T>>();
            foreach (var kvp in ListeAdjacence)
            {
                ensemble.Add(kvp.Key);
                foreach (var arc in kvp.Value)
                    ensemble.Add(arc.Destination);
            }
            return ensemble;
        }

        /// Version de Bellman-Ford : on cherche le plus court chemin même s'il y a des poids négatifs.
        public List<Noeud<T>> BellmanFord(Noeud<T> depart, Noeud<T> arrivee)
        {
            var tousNoeuds = RecupererTousLesNoeuds();
            var distance = tousNoeuds.ToDictionary(n => n, n => int.MaxValue);
            var precedent = tousNoeuds.ToDictionary(n => n, n => (Noeud<T>)null);
            distance[depart] = 0;

            for (int i = 0; i < tousNoeuds.Count - 1; i++)
            {
                foreach (var noeud in tousNoeuds)
                {
                    if (distance[noeud] == int.MaxValue || !ListeAdjacence.ContainsKey(noeud)) continue;

                    foreach (var arc in ListeAdjacence[noeud])
                    {
                        var voisin = arc.Destination;
                        int d = distance[noeud] + arc.Poids;
                        if (d < distance[voisin])
                        {
                            distance[voisin] = d;
                            precedent[voisin] = noeud;
                        }
                    }
                }
            }

            var chemin = new List<Noeud<T>>();
            var courant = arrivee;
            while (courant != null)
            {
                chemin.Add(courant);
                courant = precedent[courant];
            }
            chemin.Reverse();

            if (chemin.Count == 0 || !chemin[0].Equals(depart))
                return null;
            return chemin;
        }

        /// Implémentation de Floyd-Warshall : permet de connaître les plus courts chemins entre toutes les paires de nœuds.
        public List<Noeud<T>> FloydWarshall(Noeud<T> depart, Noeud<T> arrivee, out int coutTotal)
        {
            var noeuds = new List<Noeud<T>>(RecupererTousLesNoeuds());
            int n = noeuds.Count;
            var index = new Dictionary<Noeud<T>, int>();
            for (int i = 0; i < n; i++) index[noeuds[i]] = i;

            int[,] dist = new int[n, n];
            int?[,] suivant = new int?[n, n];
            int infini = int.MaxValue / 2;

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    dist[i, j] = (i == j) ? 0 : infini;
                    suivant[i, j] = (i == j) ? j : (int?)null;
                }

            foreach (var kvp in ListeAdjacence)
            {
                int i = index[kvp.Key];
                foreach (var arc in kvp.Value)
                {
                    int j = index[arc.Destination];
                    if (arc.Poids < dist[i, j])
                    {
                        dist[i, j] = arc.Poids;
                        suivant[i, j] = j;
                    }
                }
            }

            for (int k = 0; k < n; k++)
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        if (dist[i, k] + dist[k, j] < dist[i, j])
                        {
                            dist[i, j] = dist[i, k] + dist[k, j];
                            suivant[i, j] = suivant[i, k];
                        }

            int a = index[depart];
            int b = index[arrivee];
            if (suivant[a, b] == null)
            {
                coutTotal = infini;
                return null;
            }

            List<Noeud<T>> chemin = new();
            int actuel = a;
            while (actuel != b)
            {
                chemin.Add(noeuds[actuel]);
                actuel = suivant[actuel, b].Value;
            }
            chemin.Add(noeuds[b]);

            coutTotal = dist[a, b];
            return chemin;
        }

        /// Permet de récupérer la première station correspondant à un nom simple (ex : "Nation").
        public Noeud<T> ObtenirPremiereCorrespondance(string nomSimple)
        {
            return ListeAdjacence.Keys
                .FirstOrDefault(n => n.Id.ToString().StartsWith(nomSimple + "_"));
        }

        /// Calcule le coût total d’un chemin (en minutes), utile pour afficher à la fin.
        public int CalculerCoutTotal(List<Noeud<T>> chemin)
        {
            int total = 0;
            for (int i = 0; i < chemin.Count - 1; i++)
            {
                var actuel = chemin[i];
                var suivant = chemin[i + 1];

                if (ListeAdjacence.TryGetValue(actuel, out var voisins))
                {
                    var arc = voisins.FirstOrDefault(l => l.Destination.Equals(suivant));
                    if (arc != null)
                        total += arc.Poids;
                }
            }
            return total;
        }
    }
}
