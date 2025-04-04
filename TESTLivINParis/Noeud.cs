namespace LeonardOzanTimothe2ndRenduGRAPHE
{
    public class Noeud<T>
    {
        // C'est l'identifiant du nœud, par exemple "Nation_1" ou "Bercy_6"
        public T Id { get; set; }

        // Constructeur : on crée un nœud en lui donnant son identifiant
        public Noeud(T id)
        {
            Id = id;
        }

        // Méthode pour comparer deux nœuds (est-ce que c’est le même ?)
        // Elle est utilisée quand on cherche dans une liste ou un dictionnaire
        public override bool Equals(object obj)
        {
            return obj is Noeud<T> autre && EqualityComparer<T>.Default.Equals(Id, autre.Id);
        }

        // Pour que le nœud fonctionne bien dans les dictionnaires (clé unique)
        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Id);
        }
    }
}
