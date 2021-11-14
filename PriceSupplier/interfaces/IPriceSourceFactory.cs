namespace PriceSupplier
{
    public interface IPriceSourceFactory
    {
        PriceSource GetSource(string ccyPair);
    }
}
