namespace WordGameOOP;

interface ISerialize
{
    string Serialize<T>(IEnumerable<T> values);

    string Serialize<T>(T value);
}