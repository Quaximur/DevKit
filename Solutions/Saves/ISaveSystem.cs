using System.Collections.Generic;

namespace DevKit.Saves
{
    public interface ISaveSystem
    {
        public void Save<T>(string key, T data);
        public void SaveBytes(string key, byte[] data, string extension);
        public byte[] LoadBytes(string key, string extension);
        public string LoadRaw(string key);
        public IReadOnlyList<string> GetKeys(string prefix);

        public T Load<T>(string key)
        {
            return default;
        }

        public void Delete(string key);
        public bool Exists(string key);
    }
}
