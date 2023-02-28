namespace Infrastructure.JsonConverterWrapper
{
    public interface IJsonConvertWrapper
    {
        T? Deserialize<T>(string message);
        string Serialize(object obj);
    }
}