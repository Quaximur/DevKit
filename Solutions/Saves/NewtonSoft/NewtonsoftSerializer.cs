#if NEWTONSOFT_JSON
using Newtonsoft.Json;

namespace DevKit.Saves
{
    public class NewtonsoftSerializer : ISerializer
    {
        public string Serialize<T>(T rawData)
        {
            return JsonConvert.SerializeObject(rawData);
        }

        public T Deserialize<T>(string serializedData)
        {
            return JsonConvert.DeserializeObject<T>(serializedData);
        }
    }
}
#endif