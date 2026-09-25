using System.Collections.Generic;

namespace DevKit.Saves
{
    public interface IDataStorage
    {
        public void Write(string key, string serializedData);
        public void WriteBytes(string key, byte[] data, string extension);
        public byte[] ReadBytes(string key, string extension);
        public string Read(string key);
        public void Delete(string key);
        public bool Exists(string key);
        public IReadOnlyList<string> GetKeys(string prefix);
    }
}
