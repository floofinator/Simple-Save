using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace Floofinator.SimpleSave
{
    public class JsonFiler : Filer
    {
        public override void SaveFile(string directory, string fileName, object data)
        {
            string filePath = GetFilePath(directory, fileName);
            
            if (!DirectoryExists(directory)) CreateDirectory(directory);

            string dataJson = JsonUtility.ToJson(data, true);

            using FileStream stream = new(filePath, FileMode.Create);
            using StreamWriter writer = new(stream);
            writer.Write(dataJson);

            Debug.Log($"Saved file to \"{filePath}\".");
        }
        public override bool LoadFile(string directory, string fileName, Type saveType, out object data)
        {
            string filePath = GetFilePath(directory, fileName);

            if (!FileExists(directory, fileName))
            {
                data = default;
                Debug.Log($"No file to load from \"{filePath}\".");
                return false;
            }

            using FileStream stream = new(filePath, FileMode.Open);
            using StreamReader reader = new(stream);
            string dataJson = reader.ReadToEnd();

            data = JsonUtility.FromJson(dataJson, saveType);

            Debug.Log($"Loaded file from \"{filePath}\".");

            return true;
        }
    }
}