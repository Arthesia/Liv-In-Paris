namespace LeonardOzanTimothe2ndRenduGRAPHE
{
    public class Noeud<T>
    {
        public T Id { get; set; }

        public Noeud(T id)
        {
            Id = id;
        }

        public override bool Equals(object obj)
        {
            return obj is Noeud<T> autre && EqualityComparer<T>.Default.Equals(Id, autre.Id);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Id);
        }
    }
}
