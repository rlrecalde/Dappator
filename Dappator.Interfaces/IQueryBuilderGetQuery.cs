namespace Dappator.Interfaces
{
    public interface IQueryBuilderGetQuery
    {
        /// <summary>
        /// Returns the built query and its values at the moment
        /// </summary>
        IQueryAndValues GetQuery();
    }
}
