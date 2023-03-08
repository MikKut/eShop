using Newtonsoft.Json;

namespace Infrastructure.JsonConverterWrapper
{
    public class JsonConvertWrapper : IJsonConvertWrapper
    {
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T? Deserialize<T>(string message)
        {
            return JsonConvert.DeserializeObject<T>(message);
        }
    }
}
