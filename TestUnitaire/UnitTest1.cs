using LeonardOzanTimothe2ndRenduGRAPHE;

namespace TestUnitaire
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void TestAjouterArc()
        {
            /// On cr�e un graphe vide
            var graphe = new Graphe<string>();

            /// On ajoute deux noeuds A et B
            var noeud1 = new Noeud<string>("A");
            var noeud2 = new Noeud<string>("B");

            /// On ajoute un arc de A vers B avec un poids de 1
            graphe.AjouterArc(noeud1, noeud2, 1);

            /// On v�rifie que A a bien une liaison vers B
            Assert.IsTrue(graphe.ListeAdjacence.ContainsKey(noeud1));
            Assert.AreEqual(1, graphe.ListeAdjacence[noeud1].Count);
            Assert.AreEqual(noeud2, graphe.ListeAdjacence[noeud1][0].Destination);
        }

        [TestMethod]
        public void TestRecupererTousLesNoeuds()
        {
            /// On cr�e un graphe avec une liaison entre A et B
            var graphe = new Graphe<string>();
            var noeud1 = new Noeud<string>("A");
            var noeud2 = new Noeud<string>("B");
            graphe.AjouterArc(noeud1, noeud2, 1);

            /// On r�cup�re tous les noeuds du graphe
            var noeuds = graphe.RecupererTousLesNoeuds();

            /// On v�rifie que les deux noeuds sont bien pr�sents
            Assert.AreEqual(2, noeuds.Count);
            Assert.IsTrue(noeuds.Contains(noeud1));
            Assert.IsTrue(noeuds.Contains(noeud2));
        }

        [TestMethod]
        public void TestBellmanFord()
        {
            /// On cr�e un graphe avec les noeuds A, B et C
            var graphe = new Graphe<string>();
            var noeud1 = new Noeud<string>("A");
            var noeud2 = new Noeud<string>("B");
            var noeud3 = new Noeud<string>("C");

            /// On relie A � B, puis B � C
            graphe.AjouterArc(noeud1, noeud2, 1);
            graphe.AjouterArc(noeud2, noeud3, 1);

            /// On cherche le chemin le plus court de A � C
            var chemin = graphe.BellmanFord(noeud1, noeud3);

            /// Le chemin attendu est A, B, C
            Assert.AreEqual(3, chemin.Count);
            Assert.AreEqual(noeud1, chemin[0]);
            Assert.AreEqual(noeud2, chemin[1]);
            Assert.AreEqual(noeud3, chemin[2]);
        }

        [TestMethod]
        public void TestFloydWarshall()
        {
            /// On cr�e le m�me graphe A, B, C
            var graphe = new Graphe<string>();
            var noeud1 = new Noeud<string>("A");
            var noeud2 = new Noeud<string>("B");
            var noeud3 = new Noeud<string>("C");

            /// On relie A � B et B � C
            graphe.AjouterArc(noeud1, noeud2, 1);
            graphe.AjouterArc(noeud2, noeud3, 1);

            /// On calcule le chemin avec Floyd-Warshall
            int coutTotal;
            var chemin = graphe.FloydWarshall(noeud1, noeud3, out coutTotal);

            /// Le chemin attendu est A, B, C et le co�t est 2
            Assert.AreEqual(3, chemin.Count);
            Assert.AreEqual(noeud1, chemin[0]);
            Assert.AreEqual(noeud2, chemin[1]);
            Assert.AreEqual(noeud3, chemin[2]);
            Assert.AreEqual(2, coutTotal);
        }
    }
}
