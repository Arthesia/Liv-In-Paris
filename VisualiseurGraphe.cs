using SkiaSharp;

namespace LeonardOzanTimothe2ndRenduGRAPHE
{
    public class VisualiseurGraphe<T>
    {
        private Graphe<T> graphe;

        // On augmente la taille de l'image
        private int largeurImage = 1600;
        private int hauteurImage = 1200;

        // Positions calculées pour chaque nœud unifié (par station)
        private Dictionary<Noeud<T>, SKPoint> positionsNoeuds;

        // Liste de tous les nœuds unifiés
        private List<Noeud<T>> noeudsUnifies;
        // Liste des liens (paires de nœuds unifiés) pour le dessin
        private List<(Noeud<T> A, Noeud<T> B)> liensUnifies;

        public VisualiseurGraphe(Graphe<T> graphe)
        {
            this.graphe = graphe;
            this.positionsNoeuds = new Dictionary<Noeud<T>, SKPoint>();
            this.noeudsUnifies = new List<Noeud<T>>();
            this.liensUnifies = new List<(Noeud<T> A, Noeud<T> B)>();
        }

        public void DessinerGraphe(string cheminFichier)
        {
            // 1) Construire une version "unifiée" des nœuds : on fusionne Station_Ligne en Station
            ConstruireNoeudsUnifiesEtLiens();

            // 2) Calculer une disposition "force-directed" simple pour mieux espacer les nœuds
            CalculerDispositionForce();

            // 3) Dessiner dans un bitmap
            using (var imageBitmap = new SKBitmap(largeurImage, hauteurImage))
            using (var toile = new SKCanvas(imageBitmap))
            {
                toile.Clear(SKColors.White);

                // Pinceaux
                var pinceauLien = new SKPaint
                {
                    Color = SKColors.Gray,
                    StrokeWidth = 2,
                    IsAntialias = true
                };
                var pinceauNoeud = new SKPaint
                {
                    Color = SKColors.Blue,
                    IsAntialias = true
                };
                var pinceauTexte = new SKPaint
                {
                    Color = SKColors.Black,
                    TextSize = 16,
                    IsAntialias = true
                };

                // Dessiner les liens
                foreach (var (A, B) in liensUnifies)
                {
                    var pA = positionsNoeuds[A];
                    var pB = positionsNoeuds[B];
                    toile.DrawLine(pA, pB, pinceauLien);
                }

                // Dessiner les nœuds + étiquettes
                foreach (var noeud in noeudsUnifies)
                {
                    var pos = positionsNoeuds[noeud];
                    toile.DrawCircle(pos, 12, pinceauNoeud);

                    // Afficher le label (nom)
                    string etiquette = noeud.Id.ToString();
                    // Pour centrer un peu le texte
                    SKRect bounds = new SKRect();
                    pinceauTexte.MeasureText(etiquette, ref bounds);
                    float decalageX = bounds.MidX;
                    float decalageY = bounds.MidY;
                    toile.DrawText(etiquette, pos.X - decalageX, pos.Y - 20 - decalageY, pinceauTexte);
                }

                // Sauvegarder en PNG
                using (var image = SKImage.FromBitmap(imageBitmap))
                using (var donnees = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var flux = File.OpenWrite(cheminFichier))
                {
                    donnees.SaveTo(flux);
                }
            }
        }

        /// <summary>
        /// Unifie les nœuds : "Nation_6", "Nation_2" => un seul nœud "Nation".
        /// Construit aussi la liste de liens unifiés.
        /// </summary>
        private void ConstruireNoeudsUnifiesEtLiens()
        {
            // Dictionnaire baseName -> Noeud unifié
            var baseNameToNode = new Dictionary<string, Noeud<T>>();

            // 1) Parcourir tous les nœuds existants
            foreach (var noeudComplet in graphe.ListeAdjacence.Keys)
            {
                string nomComplet = noeudComplet.Id.ToString();
                // On coupe au premier underscore
                string baseName = nomComplet.Split('_')[0];

                if (!baseNameToNode.ContainsKey(baseName))
                {
                    // Créer un nouveau Noeud<T> avec juste la partie "baseName"
                    // On cast T en string => (T)(object)baseName
                    var nouveauNoeud = new Noeud<T>((T)(object)baseName);
                    baseNameToNode[baseName] = nouveauNoeud;
                }
            }

            // 2) Construire la liste de liens unifiés
            var listeLiens = new List<(Noeud<T>, Noeud<T>)>();
            foreach (var kvp in graphe.ListeAdjacence)
            {
                var noeudCompletSource = kvp.Key;
                string baseNameSource = noeudCompletSource.Id.ToString().Split('_')[0];
                var noeudUnifieSource = baseNameToNode[baseNameSource];

                foreach (var lien in kvp.Value)
                {
                    var noeudCompletDest = lien.Destination;
                    string baseNameDest = noeudCompletDest.Id.ToString().Split('_')[0];
                    var noeudUnifieDest = baseNameToNode[baseNameDest];

                    // Éviter les doublons => on peut simplement stocker (source, dest)
                    // On évite de dupliquer (dest, source) si on a un graphe non dirigé
                    listeLiens.Add((noeudUnifieSource, noeudUnifieDest));
                }
            }

            // 3) On enlève les doublons si le graphe est non-dirigé
            //    (ex. (A,B) et (B,A) c'est le même lien)
            var setLiens = new HashSet<(Noeud<T>, Noeud<T>)>(new LienUndirectedComparer<T>());
            foreach (var l in listeLiens)
            {
                setLiens.Add(l);
            }

            // On met dans liensUnifies
            liensUnifies = setLiens.ToList();

            // 4) Récupérer la liste finale des noeuds unifiés
            noeudsUnifies = baseNameToNode.Values.ToList();
        }

        /// <summary>
        /// Petit algo "force-directed" simplifié pour espacer les nœuds.
        /// </summary>
        private void CalculerDispositionForce()
        {
            // On place d'abord chaque nœud au hasard dans [0..largeurImage], [0..hauteurImage]
            Random rand = new Random();
            foreach (var n in noeudsUnifies)
            {
                positionsNoeuds[n] = new SKPoint(
                    rand.Next(0, largeurImage),
                    rand.Next(0, hauteurImage)
                );
            }

            // Quelques paramètres
            float forceRepulsion = 30000f;   // force de répulsion
            float forceAttire = 0.001f;      // force d'attraction
            float rayonMax = Math.Min(largeurImage, hauteurImage);

            // On fait 300 itérations (à ajuster si besoin)
            for (int iteration = 0; iteration < 300; iteration++)
            {
                // Stocke les déplacements
                var deplacements = new Dictionary<Noeud<T>, SKPoint>();
                foreach (var n in noeudsUnifies)
                    deplacements[n] = new SKPoint(0, 0);

                // 1) Repulsion entre tous les couples de noeuds
                for (int i = 0; i < noeudsUnifies.Count; i++)
                {
                    for (int j = i + 1; j < noeudsUnifies.Count; j++)
                    {
                        var nA = noeudsUnifies[i];
                        var nB = noeudsUnifies[j];
                        var pA = positionsNoeuds[nA];
                        var pB = positionsNoeuds[nB];

                        float dx = pB.X - pA.X;
                        float dy = pB.Y - pA.Y;
                        float dist2 = dx * dx + dy * dy;
                        if (dist2 < 0.01f) dist2 = 0.01f; // éviter division par zéro
                        float dist = (float)Math.Sqrt(dist2);

                        // Force de répulsion ~ forceRepulsion / dist^2
                        float f = forceRepulsion / dist2;

                        // Direction
                        float fx = f * dx / dist;
                        float fy = f * dy / dist;

                        // Appliquer
                        deplacements[nA] = new SKPoint(deplacements[nA].X - fx, deplacements[nA].Y - fy);
                        deplacements[nB] = new SKPoint(deplacements[nB].X + fx, deplacements[nB].Y + fy);
                    }
                }

                // 2) Attraction sur les liens
                foreach (var (A, B) in liensUnifies)
                {
                    if (A == B) continue;
                    var pA = positionsNoeuds[A];
                    var pB = positionsNoeuds[B];

                    float dx = pB.X - pA.X;
                    float dy = pB.Y - pA.Y;
                    float dist = (float)Math.Sqrt(dx * dx + dy * dy);
                    if (dist < 0.01f) dist = 0.01f;

                    // Force d'attraction ~ forceAttire * dist
                    float f = forceAttire * (dist);

                    // Direction
                    float fx = f * dx / dist;
                    float fy = f * dy / dist;

                    // Appliquer
                    deplacements[A] = new SKPoint(deplacements[A].X + fx, deplacements[A].Y + fy);
                    deplacements[B] = new SKPoint(deplacements[B].X - fx, deplacements[B].Y - fy);
                }

                // 3) Mettre à jour les positions
                foreach (var n in noeudsUnifies)
                {
                    var p = positionsNoeuds[n];
                    var d = deplacements[n];

                    // On limite un peu la vitesse pour éviter les mouvements trop grands
                    float maxDepl = 50f;
                    float deplX = Math.Max(-maxDepl, Math.Min(maxDepl, d.X));
                    float deplY = Math.Max(-maxDepl, Math.Min(maxDepl, d.Y));

                    float newX = p.X + deplX;
                    float newY = p.Y + deplY;

                    // On reste dans la zone [0..largeurImage], [0..hauteurImage]
                    newX = Math.Max(30, Math.Min(largeurImage - 30, newX));
                    newY = Math.Max(30, Math.Min(hauteurImage - 30, newY));

                    positionsNoeuds[n] = new SKPoint(newX, newY);
                }
            }
        }
    }

    /// <summary>
    /// Comparateur pour liens non dirigés : (A,B) == (B,A)
    /// </summary>
    public class LienUndirectedComparer<T> : IEqualityComparer<(Noeud<T>, Noeud<T>)>
    {
        public bool Equals((Noeud<T>, Noeud<T>) x, (Noeud<T>, Noeud<T>) y)
        {
            // On considère (A,B) == (B,A)
            return (x.Item1.Equals(y.Item1) && x.Item2.Equals(y.Item2))
                || (x.Item1.Equals(y.Item2) && x.Item2.Equals(y.Item1));
        }

        public int GetHashCode((Noeud<T>, Noeud<T>) obj)
        {
            // Pour un set non dirigé, on additionne les hash dans un ordre trié
            int h1 = obj.Item1.GetHashCode();
            int h2 = obj.Item2.GetHashCode();
            // on range le plus petit en premier
            if (h1 < h2)
                return h1 ^ (h2 + 0x9e3779b9 + (h1 << 6) + (h1 >> 2));
            else
                return h2 ^ (h1 + 0x9e3779b9 + (h2 << 6) + (h2 >> 2));
        }
    }
}
