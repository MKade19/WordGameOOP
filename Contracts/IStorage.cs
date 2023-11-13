namespace WordGameOOP.Contracts;
interface IStorage 
{
    Task SaveAsync(string path, string content);

    Task<string> RestoreAsync(string path);

    void Delete(string path);
}