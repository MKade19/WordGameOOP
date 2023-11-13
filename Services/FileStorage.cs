using WordGameOOP.Contracts;
using WordGameOOP.Constants;

namespace WordGameOOP.Services;
class FileStorage : IStorage
{
    private IOutput _output;
    public FileStorage(IOutput output)
    {
        _output = output;
    }

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
            _output.ShowMessage(MessageConstants.FILE_NOT_FOUND_MESSAGE);
            return String.Empty;
        }
        catch(Exception e)
        {
            _output.ShowMessage(e.Message);
            return String.Empty;
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
            _output.ShowMessage(e.Message);
        }
        finally
        {
            if (sw is not null)
            {
                 sw.Close();
            }
        }
        
    }


    /// <summary>
    /// Deletes file on given <paramref name="path"/>
    /// </summary>
    /// <param name="path">Path of file to delete</param>
    public void Delete(string path) 
    {
        if (File.Exists(path)) 
        {
            File.Delete(path);
        } 
    }
}