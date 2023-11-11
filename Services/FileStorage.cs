using WordGameOOP.Contracts;

namespace WordGameOOP.Services;
class FileStorage : IStorage
{
    /// <summary>
    /// Restores data from file with provided <paramref name="path"/> asynchronously
    /// </summary>
    /// <param name="path">Path to file</param>
    /// <returns>String which contain data from file.</returns>
    public async Task<string> RestoreAsync(string path)
    {
        StreamReader? sr = null;
        try 
        {
            sr = new StreamReader(path);
            string content = await sr.ReadToEndAsync();
            return content;
        }
        catch(FileNotFoundException)
        {
            Console.WriteLine("File not found!");
            return "";
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return "";
        }
        finally
        {
            if (sr is not null) 
            {
                sr.Close();
            }
        }
    }

    /// <summary>
    /// Save the provided <paramref name="content"/> to file 
    /// with provided <paramref name="path"/> asynchronously
    /// </summary>
    /// <param name="path">Path to file</param>
    /// <param name="content">Data that sould be stored</param>
    /// <returns></returns>
    public async Task SaveAsync(string path, string content)
    {
        StreamWriter? sw = null;

        try
        {
            sw = new StreamWriter(path);
            await sw.WriteAsync(content);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            if (sw is not null)
            {
                 sw.Close();
            }
        }
        
    }
}