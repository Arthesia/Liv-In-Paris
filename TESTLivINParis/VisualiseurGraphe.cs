using SkiaSharp;
using System.Globalization;

namespace LeonardOzanTimothe2ndRenduGRAPHE
{
    public class VisualiseurGraphe<T>
    {
        private Graphe<T> graphe;
        private int largeurImage = 2500;
        private int hauteurImage = 2000;
        private Dictionary<Noeud<T>, SKPoint> positionsNoeuds;
        private List<Noeud<T>> noeudsUnifies;
        private List<(Noeud<T> A, Noeud<T> B)> liensUnifies;

        public VisualiseurGraphe(Graphe<T> graphe)
        {
            this.graphe = graphe;
            this.positionsNoeuds = new Dictionary<Noeud<T>, SKPoint>();
            this.noeudsUnifies = new List<Noeud<T>>();
            this.liensUnifies = new List<(Noeud<T>, Noeud<T>)>();
        }

        public void DessinerGraphe(string cheminFichier)
        {
            /// On prépare les données pour ne garder qu’un seul noeud par station (ex : Nation_1 et Nation_6 deviennent juste Nation)
            ConstruireNoeudsUnifiesEtLiens();

            /// On place chaque station sur l’image selon sa position GPS qu’on lit dans le CSV
            CalculerDispositionParCoordonnees("MetroParis.csv");

            /// On crée une image vide où on va tout dessiner
            using (var imageBitmap = new SKBitmap(largeurImage, hauteurImage))
            using (var toile = new SKCanvas(imageBitmap))
            {
                toile.Clear(SKColors.White); // fond blanc

                /// Style pour les traits entre les stations
                var pinceauLien = new SKPaint { Color = SKColors.Gray, StrokeWidth = 2, IsAntialias = true };

                /// Style pour les cercles des stations
                var pinceauNoeud = new SKPaint { Color = SKColors.Blue, IsAntialias = true };

                /// Style pour écrire le nom des stations
                var pinceauTexte = new SKPaint
                {
                    Color = SKColors.Black,
                    TextSize = 12,
                    IsAntialias = true,
                    Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold)
                };

                /// On trace les lignes entre les stations connectées
                foreach (var (A, B) in liensUnifies)
                {
                    SKPoint pA = positionsNoeuds[A];
                    SKPoint pB = positionsNoeuds[B];
                    toile.DrawLine(pA, pB, pinceauLien);
                }

                /// On trace les cercles + noms des stations
                foreach (var noeud in noeudsUnifies)
                {
                    SKPoint pos = positionsNoeuds[noeud];
                    toile.DrawCircle(pos, 10, pinceauNoeud);

                    string texte = noeud.Id.ToString();

                    /// On décale un peu le texte pour éviter qu’il se superpose au cercle
                    float decalageX = 12f;
                    float decalageY = -12f;

                    /// Petite astuce pour éviter que tous les textes se collent au même endroit
                    if ((pos.X + pos.Y) % 2 == 0)
                    {
                        decalageX = -25f;
                        decalageY = 20f;
                    }

                    toile.DrawText(texte, pos.X + decalageX, pos.Y + decalageY, pinceauTexte);
                }

                /// On enregistre l’image finale en PNG
                using var image = SKImage.FromBitmap(imageBitmap);
                using var donnees = image.Encode(SKEncodedImageFormat.Png, 100);
                using var flux = File.OpenWrite(cheminFichier);
                donnees.SaveTo(flux);
            }
        }

        private void ConstruireNoeudsUnifiesEtLiens()
        {
            /// Ce dico garde un seul noeud par station, peu importe la ligne
            var baseNomVersNoeud = new Dictionary<string, Noeud<T>>();

            /// On crée un noeud pour chaque station unique (en enlevant le _ligne)
            foreach (var noeudComplet in graphe.ListeAdjacence.Keys)
            {
                string nomComplet = noeudComplet.Id.ToString();
                string baseNom = nomComplet.Split('_')[0];

                if (!baseNomVersNoeud.ContainsKey(baseNom))
                {
                    var nouveauNoeud = new Noeud<T>((T)(object)baseNom);
                    baseNomVersNoeud[baseNom] = nouveauNoeud;
                }
            }

            /// On construit les liaisons entre ces noeuds unifiés
            var listeLiens = new List<(Noeud<T>, Noeud<T>)>();

            foreach (var kvp in graphe.ListeAdjacence)
            {
                var sourceComplet = kvp.Key;
                string baseSource = sourceComplet.Id.ToString().Split('_')[0];
                var sourceUnifie = baseNomVersNoeud[baseSource];

                foreach (var lien in kvp.Value)
                {
                    var destComplet = lien.Destination;
                    string baseDest = destComplet.Id.ToString().Split('_')[0];
                    var destUnifie = baseNomVersNoeud[baseDest];

                    listeLiens.Add((sourceUnifie, destUnifie));
                }
            }

            /// On garde chaque lien une seule fois, peu importe l’ordre
            var ensembleLiens = new HashSet<(Noeud<T>, Noeud<T>)>(new LienUndirectedComparer<T>());
            foreach (var lien in listeLiens)
            {
                ensembleLiens.Add(lien);
            }

            liensUnifies = ensembleLiens.ToList();
            noeudsUnifies = baseNomVersNoeud.Values.ToList();
        }

        private void CalculerDispositionParCoordonnees(string cheminCSV)
        {
            /// Ce dico va contenir les coordonnées GPS de chaque station
            var positionsGPS = new Dictionary<string, (float lon, float lat)>();
            var lignes = File.ReadAllLines(cheminCSV);

            /// On lit chaque ligne du CSV (en sautant la 1ère qui est l’en-tête)
            for (int i = 1; i < lignes.Length; i++)
            {
                var champs = lignes[i].Split(',');

                if (champs.Length < 5) continue;

                string nomStation = champs[2].Trim();
                if (!float.TryParse(champs[3], NumberStyles.Float, CultureInfo.InvariantCulture, out float lon)) continue;
                if (!float.TryParse(champs[4], NumberStyles.Float, CultureInfo.InvariantCulture, out float lat)) continue;

                /// On évite d’écraser les coordonnées si elles existent déjà
                if (!positionsGPS.ContainsKey(nomStation))
                    positionsGPS[nomStation] = (lon, lat);
            }

            if (positionsGPS.Count == 0)
            {
                Console.WriteLine("Aucune coordonnée GPS valide trouvée.");
                return;
            }

            /// On récupère les bornes min et max des coordonnées pour les adapter à l’image
            float minLon = positionsGPS.Values.Min(p => p.lon);
            float maxLon = positionsGPS.Values.Max(p => p.lon);
            float minLat = positionsGPS.Values.Min(p => p.lat);
            float maxLat = positionsGPS.Values.Max(p => p.lat);
            float marge = 50;

            /// On transforme chaque coordonnée GPS en position X/Y sur l’image
            foreach (var noeud in noeudsUnifies)
            {
                string nom = noeud.Id.ToString();
                if (positionsGPS.TryGetValue(nom, out var coords))
                {
                    float x = (coords.lon - minLon) / (maxLon - minLon) * (largeurImage - 2 * marge) + marge;
                    float y = (1 - (coords.lat - minLat) / (maxLat - minLat)) * (hauteurImage - 2 * marge) + marge;
                    positionsNoeuds[noeud] = new SKPoint(x, y);
                }
                else
                {
                    /// Si on n’a pas les coordonnées, on place le point au milieu
                    positionsNoeuds[noeud] = new SKPoint(largeurImage / 2, hauteurImage / 2);
                }
            }
        }
    }

    public class LienUndirectedComparer<T> : IEqualityComparer<(Noeud<T>, Noeud<T>)>
    {
        /// Ce comparer dit que (A,B) = (B,A), donc pas de doublons de liens
        public bool Equals((Noeud<T>, Noeud<T>) x, (Noeud<T>, Noeud<T>) y)
        {
            return (x.Item1.Equals(y.Item1) && x.Item2.Equals(y.Item2))
                || (x.Item1.Equals(y.Item2) && x.Item2.Equals(y.Item1));
        }

        public int GetHashCode((Noeud<T>, Noeud<T>) obj)
        {
            uint h1 = (uint)obj.Item1.GetHashCode();
            uint h2 = (uint)obj.Item2.GetHashCode();
            uint prime = 0x9e3779b9;

            /// Petite formule pour avoir un hash unique, peu importe l’ordre
            if (h1 < h2)
            {
                uint val = h2 + prime + (h1 << 6) + (h1 >> 2);
                return (int)(h1 ^ val);
            }
            else
            {
                uint val = h1 + prime + (h2 << 6) + (h2 >> 2);
                return (int)(h2 ^ val);
            }
        }
    }
}
