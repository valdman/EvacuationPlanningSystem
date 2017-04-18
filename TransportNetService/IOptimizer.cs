namespace TransportNetService
{
    public interface IOptimizer
    {
        TransportTable optimize(TransportTable table);
    }
}