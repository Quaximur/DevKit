using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DevKit.Utils;
using UnityEngine;

namespace DevKit.Saves
{
    public class FileStorage : IDataStorage
    {
        private readonly string _basePath;
        private readonly string _fileExtension;

        public FileStorage(string fileExtension)
        {
#if UNITY_EDITOR
            _basePath = Path.Combine(Application.dataPath, "SaveData");
            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);
#else
            _basePath = UnityEngine.Application.persistentDataPath;
#endif

            _fileExtension = fileExtension;
        }

        public void Write(string key, string serializedData)
        {
            var path = GetPath(key);

            try
            {
                var directoryPath = Path.GetDirectoryName(path);
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                File.WriteAllText(path, serializedData);
            }
            catch (Exception ex)
            {
                Debug.LogError($"FileStorage: Save operation failed: {ex.Message}");
            }
        }

        public string Read(string key)
        {
            var path = GetPath(key);

            try
            {
                var text = File.ReadAllText(path);

                return text;
            }
            catch (Exception e)
            {
                FLogger.LogError<FileStorage>(e.Message);
                return null;
            }
        }

        public void Delete(string key)
        {
            var path = GetPath(key);

            if (File.Exists(path))
                File.Delete(path);
        }

        public bool Exists(string key)
        {
            var path = GetPath(key);
            var exists = File.Exists(path);

            return exists;
        }

        public IReadOnlyList<string> GetKeys(string prefix)
        {
            var directory = Path.Combine(_basePath, prefix);
            if (!Directory.Exists(directory))
                return Array.Empty<string>();

            return Directory.GetFiles(directory, $"*.{_fileExtension}")
                .Select(path => Path.ChangeExtension(Path.GetRelativePath(_basePath, path), null)
                    .Replace(Path.DirectorySeparatorChar, '/'))
                .ToArray();
        }

        private string GetPath(string key)
        {
            var path = Path.Combine(_basePath, String.Concat(key, ".", _fileExtension));
            return path;
        }
    }
}