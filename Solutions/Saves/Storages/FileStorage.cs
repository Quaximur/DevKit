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
            var path = GetPath(key, _fileExtension);

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

        public void WriteBytes(string key, byte[] data, string extension)
        {
            var path = GetPath(key, extension);

            try
            {
                var directoryPath = Path.GetDirectoryName(path);
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                File.WriteAllBytes(path, data);
            }
            catch (Exception ex)
            {
                Debug.LogError($"FileStorage: Binary save operation failed: {ex.Message}");
            }
        }

        public byte[] ReadBytes(string key, string extension)
        {
            var path = GetPath(key, extension);
            if (!File.Exists(path))
                return null;

            try
            {
                return File.ReadAllBytes(path);
            }
            catch (Exception ex)
            {
                FLogger.LogError<FileStorage>(ex.Message);
                return null;
            }
        }

        public string Read(string key)
        {
            var path = GetPath(key, _fileExtension);

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
            var path = GetPath(key, _fileExtension);

            if (File.Exists(path))
                File.Delete(path);
        }

        public bool Exists(string key)
        {
            var path = GetPath(key, _fileExtension);
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

        private string GetPath(string key, string extension)
        {
            var path = Path.Combine(_basePath, String.Concat(key, ".", extension));
            return path;
        }
    }
}
