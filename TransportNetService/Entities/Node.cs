namespace TransportNetService.Entities
{
    public class Node
    {
        public int Potencial;

        public Node(int id, int value)
        {
            Id = id;
            Value = value;
        }

        public int Id { get; }
        public int Value { get; set; }
    }
}