using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Floofinator.SimpleSave
{
    public class BinaryFiler : Filer
    {
        readonly BinaryFormatter formatter = new();
        public override void SaveFile(string directory, string fileName, object data)
        {
            string filePath = GetFilePath(directory, fileName);
            
            if (!DirectoryExists(directory)) CreateDirectory(directory);

            using FileStream stream = new(filePath, FileMode.Create);
            formatter.Serialize(stream, data);

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
            data = formatter.Deserialize(stream);

            Debug.Log($"Loaded file from \"{filePath}\".");

            return true;
        }
    }
}
