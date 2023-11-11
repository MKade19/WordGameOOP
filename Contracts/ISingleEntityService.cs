using WordGameOOP.Models;

namespace WordGameOOP.Contracts;
interface ISingleEntityService<T>
{
    Task SaveAsync(T entity);

    Task<T?> RestoreAsync();
}