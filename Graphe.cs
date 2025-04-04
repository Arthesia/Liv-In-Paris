namespace LeonardOzanTimothe2ndRenduGRAPHE
{
    public class Graphe<T>
    {
        public Dictionary<Noeud<T>, List<Lien<T>>> ListeAdjacence { get; set; } = new();

        public void AjouterArc(Noeud<T> source, Noeud<T> destination, int poids)
        {
            if (!ListeAdjacence.ContainsKey(source))
                ListeAdjacence[source] = new List<Lien<T>>();

            ListeAdjacence[source].Add(new Lien<T>(destination, poids));
        }

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

        public void ChargerDepuisCSV(string cheminStations, string cheminLiaisons)
        {
            Dictionary<string, string> stations = new Dictionary<string, string>();
            Dictionary<string, List<string>> correspondances = new Dictionary<string, List<string>>();

            HashSet<(string, string)> liaisonsSensUnique = new HashSet<(string, string)>
            {
                ("Botzaris_7bis", "Place des Fêtes_7bis"),
                ("Place des Fêtes_7bis", "Pré-Saint-Gervais_7bis"),
                ("Pré-Saint-Gervais_7bis", "Danube_7bis"),
                ("Danube_7bis", "Botzaris_7bis"),
                ("Botzaris_7bis", "Buttes Chaumont_7bis")
            };

            string[] lignesStations = File.ReadAllLines(cheminStations);
            for (int i = 1; i < lignesStations.Length; i++)
            {
                string[] champs = lignesStations[i].Split(',');
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

            string[] lignesLiaisons = File.ReadAllLines(cheminLiaisons);
            for (int i = 1; i < lignesLiaisons.Length; i++)
            {
                string[] champs = lignesLiaisons[i].Split(',');
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

                    (string, string) arc = (stationSource, stationDestination);
                    if (!liaisonsSensUnique.Contains(arc))
                        AjouterArc(noeudDestination, noeudSource, poids);
                }

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

        // --- Algorithme de Dijkstra (simple) ---
        public List<Noeud<T>> Dijkstra(Noeud<T> depart, Noeud<T> arrivee)
        {
            Dictionary<Noeud<T>, int> distance = new Dictionary<Noeud<T>, int>();
            Dictionary<Noeud<T>, Noeud<T>> precedent = new Dictionary<Noeud<T>, Noeud<T>>();
            List<Noeud<T>> nonVisites = new List<Noeud<T>>();

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
                Noeud<T> courant = nonVisites[0];
                foreach (var noeud in nonVisites)
                {
                    if (distance[noeud] < distance[courant])
                        courant = noeud;
                }
                nonVisites.Remove(courant);

                if (courant.Equals(arrivee))
                    break;
                if (distance[courant] == int.MaxValue)
                    break;

                if (ListeAdjacence.ContainsKey(courant))
                {
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
            }

            List<Noeud<T>> chemin = new List<Noeud<T>>();
            Noeud<T> temp = arrivee;
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

        // --- Méthode utilitaire pour récupérer tous les nœuds du graphe ---
        public HashSet<Noeud<T>> RecupererTousLesNoeuds()
        {
            HashSet<Noeud<T>> ensembleNoeuds = new HashSet<Noeud<T>>();
            foreach (var element in ListeAdjacence)
            {
                ensembleNoeuds.Add(element.Key);
                foreach (var arc in element.Value)
                    ensembleNoeuds.Add(arc.Destination);
            }
            return ensembleNoeuds;
        }

        // --- Algorithme de Bellman–Ford (simple) ---
        public List<Noeud<T>> BellmanFord(Noeud<T> depart, Noeud<T> arrivee)
        {
            HashSet<Noeud<T>> tousNoeuds = RecupererTousLesNoeuds();
            Dictionary<Noeud<T>, int> distance = new Dictionary<Noeud<T>, int>();
            Dictionary<Noeud<T>, Noeud<T>> precedent = new Dictionary<Noeud<T>, Noeud<T>>();

            foreach (var noeud in tousNoeuds)
            {
                distance[noeud] = int.MaxValue;
                precedent[noeud] = null;
            }
            distance[depart] = 0;
            int nbNoeuds = tousNoeuds.Count;

            // Relaxer toutes les arêtes |V| - 1 fois
            for (int i = 0; i < nbNoeuds - 1; i++)
            {
                foreach (var noeud in tousNoeuds)
                {
                    if (distance[noeud] == int.MaxValue)
                        continue;
                    if (ListeAdjacence.ContainsKey(noeud))
                    {
                        foreach (var arc in ListeAdjacence[noeud])
                        {
                            Noeud<T> voisin = arc.Destination;
                            if (distance[noeud] + arc.Poids < distance[voisin])
                            {
                                distance[voisin] = distance[noeud] + arc.Poids;
                                precedent[voisin] = noeud;
                            }
                        }
                    }
                }
            }

            List<Noeud<T>> chemin = new List<Noeud<T>>();
            Noeud<T> courant = arrivee;
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

        // --- Algorithme de Floyd–Warshall (simple) ---
        // Renvoie le chemin le plus court entre 'depart' et 'arrivee'
        // et retourne le coût total dans 'coutTotal'
        public List<Noeud<T>> FloydWarshall(Noeud<T> depart, Noeud<T> arrivee, out int coutTotal)
        {
            HashSet<Noeud<T>> ensembleNoeuds = RecupererTousLesNoeuds();
            List<Noeud<T>> listeNoeuds = new List<Noeud<T>>(ensembleNoeuds);
            int n = listeNoeuds.Count;
            Dictionary<Noeud<T>, int> indice = new Dictionary<Noeud<T>, int>();
            for (int i = 0; i < n; i++)
            {
                indice[listeNoeuds[i]] = i;
            }

            int valeurInfinie = int.MaxValue / 2;
            int[,] matriceDistance = new int[n, n];
            int?[,] suivant = new int?[n, n];

            // Initialisation des matrices
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        matriceDistance[i, j] = 0;
                        suivant[i, j] = j;
                    }
                    else
                    {
                        matriceDistance[i, j] = valeurInfinie;
                        suivant[i, j] = null;
                    }
                }
            }

            // Remplissage avec les arcs directs
            foreach (var element in ListeAdjacence)
            {
                Noeud<T> noeudU = element.Key;
                int i = indice[noeudU];
                foreach (var arc in element.Value)
                {
                    Noeud<T> noeudV = arc.Destination;
                    int j = indice[noeudV];
                    if (arc.Poids < matriceDistance[i, j])
                    {
                        matriceDistance[i, j] = arc.Poids;
                        suivant[i, j] = j;
                    }
                }
            }

            // Exécution de Floyd–Warshall
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (matriceDistance[i, k] + matriceDistance[k, j] < matriceDistance[i, j])
                        {
                            matriceDistance[i, j] = matriceDistance[i, k] + matriceDistance[k, j];
                            suivant[i, j] = suivant[i, k];
                        }
                    }
                }
            }

            int indiceDepart = indice[depart];
            int indiceArrivee = indice[arrivee];
            if (suivant[indiceDepart, indiceArrivee] == null)
            {
                coutTotal = valeurInfinie;
                return null; // Aucun chemin
            }

            List<int> indicesChemin = new List<int>();
            int indiceCourant = indiceDepart;
            indicesChemin.Add(indiceCourant);
            while (indiceCourant != indiceArrivee)
            {
                indiceCourant = suivant[indiceCourant, indiceArrivee].Value;
                indicesChemin.Add(indiceCourant);
            }

            List<Noeud<T>> cheminFinal = new List<Noeud<T>>();
            foreach (int idx in indicesChemin)
            {
                cheminFinal.Add(listeNoeuds[idx]);
            }

            coutTotal = matriceDistance[indiceDepart, indiceArrivee];
            return cheminFinal;
        }

        public Noeud<T> ObtenirPremiereCorrespondance(string nomSimple)
        {
            return ListeAdjacence.Keys
                .FirstOrDefault(n => n.Id.ToString().StartsWith(nomSimple + "_"));
        }

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
