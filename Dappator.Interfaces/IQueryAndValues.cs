namespace Dappator.Interfaces
{
    public interface IQueryAndValues
    {
        string Query { get; }

        object[] Values { get; }
    }
}
