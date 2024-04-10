namespace Dappator.Interfaces
{
    public interface IException
    {
        string Query { get; }

        string Message { get; }

        string StackTrace { get; }
    }
}
