namespace TransportNetService
{
    internal interface IPlanBuilder
    {
        TransportTable Build(TransportTable table);
    }
}