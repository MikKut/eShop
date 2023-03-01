using Newtonsoft.Json;

namespace Infrastructure.JsonConverterWrapper
{
    public class JsonConvertWrapper : IJsonConvertWrapper
    {
        public string Serialize(object obj) => JsonConvert.SerializeObject(obj);
        public T? Deserialize<T>(string message) => JsonConvert.DeserializeObject<T>(message);
    }
}
