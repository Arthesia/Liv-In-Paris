namespace LeonardOzanTimothe2ndRenduGRAPHE
{
    public class Lien<T>
    {
        public Noeud<T> Destination { get; set; }
        public int Poids { get; set; }

        public Lien(Noeud<T> destination, int poids)
        {
            Destination = destination;
            Poids = poids;
        }
    }
}
