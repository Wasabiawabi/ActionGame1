using System.IO;
using System;
using UnityEngine;

namespace HandmadeLibrary.Json
{
    /// <summary>
    /// Jsonファイルでセーブデータを扱うためのクラス
    /// </summary>
    public class JsonFileHandler
    {
        public void SaveToJson<T>(string filePath, T data)
        {
            string directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string jsonData = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, jsonData);
        }

        public T ReadFromJson<T>(string filePath, T data)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The file at path {filePath} was not found.");
            }
            string jsonData = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<T>(jsonData);
            return data;
        }
    }
}