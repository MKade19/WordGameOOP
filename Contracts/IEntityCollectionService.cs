namespace WordGameOOP.Contracts;
interface IEntityCollectionService<T>
{
    Task AddOneIfNotExistentAsync(T entity);
    Task AddOneAsync(T entity);
    Task SaveOneAsync(T entity);
    Task SaveCollectionAsync(IEnumerable<T> entity);
    Task<IEnumerable<T>> RestoreCollectionAsync();
}