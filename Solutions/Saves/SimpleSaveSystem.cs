using System.Collections.Generic;

namespace DevKit.Saves
{
    public class SimpleSaveSystem : ISaveSystem
    {
        private readonly ISerializer _serializer;
        private readonly IDataStorage _storage;

        public SimpleSaveSystem(ISerializer serializer, IDataStorage storage)
        {
            _serializer = serializer;
            _storage = storage;
        }

        public void Save<T>(string key, T data)
        {
            var serializedData = _serializer.Serialize(data);
            _storage.Write(key, serializedData);
        }

        public void SaveBytes(string key, byte[] data, string extension)
        {
            _storage.WriteBytes(key, data, extension);
        }

        public byte[] LoadBytes(string key, string extension)
        {
            return _storage.ReadBytes(key, extension);
        }

        public T Load<T>(string key)
        {
            var serializedData = _storage.Read(key);
            var data = _serializer.Deserialize<T>(serializedData);
            
            return data;
        }
        
        public string LoadRaw(string key)
        {
            var serializedData = _storage.Read(key);

            return serializedData;
        }

        public IReadOnlyList<string> GetKeys(string prefix)
        {
            return _storage.GetKeys(prefix);
        }

        public void Delete(string key)
        {
            _storage.Delete(key);
        }

        public bool Exists(string key)
        {
            var exists = _storage.Exists(key);
            return exists;
        }
    }
}
