using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;

namespace Floofinator.SimpleSave
{
    public abstract class Filer
    {
        public static Filer Instance;
        protected string rootDirectory;
        public Filer() : this("") { }
        public Filer(string root)
        {
            SetRoot(root);
        }
        public void SetRoot(string root) { rootDirectory = Path.Combine(Application.persistentDataPath, "SaveData", root); }
        public void Compress()
        {
            if (!Directory.Exists(rootDirectory)) return;
            string directory = Path.GetDirectoryName(rootDirectory);
            string name = Path.GetFileName(rootDirectory);
            string fileName = Path.Combine(directory, name + ".save");

            if (File.Exists(fileName)) File.Delete(fileName);

            ZipFile.CreateFromDirectory(rootDirectory, fileName, System.IO.Compression.CompressionLevel.Fastest, false);
            
            Directory.Delete(rootDirectory, true);
        }
        public void UnCompress()
        {
            string directory = Path.GetDirectoryName(rootDirectory);
            string name = Path.GetFileName(rootDirectory);
            string fileName = Path.Combine(directory, name + ".save");

            if (!File.Exists(fileName)) return;
            if (Directory.Exists(rootDirectory)) Directory.Delete(rootDirectory, true);

            ZipFile.ExtractToDirectory(fileName, rootDirectory);
        }
        public void CleanUpUnCompress()
        {
            if (Directory.Exists(rootDirectory)) Directory.Delete(rootDirectory, true);
        }
        public void DeleteFile(string directory, string fileName)
        {
            string directoryPath = Path.Combine(rootDirectory, directory);
            string filePath = Path.Combine(directoryPath, fileName);

            if (File.Exists(filePath)) File.Delete(filePath);

            Debug.Log($"Deleted file from \"{directoryPath}\"");
        }
        public bool FileExists(string directory, string fileName)
        {
            return File.Exists(Path.Combine(rootDirectory, directory, fileName));
        }
        public void DeleteDirectory(string directory)
        {
            string directoryPath = Path.Combine(rootDirectory, directory);

            if (Directory.Exists(directoryPath)) Directory.Delete(directoryPath, true);
        }
        public string[] GetDirectories(string directory)
        {
            string directoryPath = Path.Combine(rootDirectory, directory);

            return PathsToNames(Directory.GetDirectories(directoryPath));
        }
        string[] PathsToNames(string[] paths)
        {
            string[] directoryNames = new string[paths.Length];

            for (int i = 0; i < paths.Length; i++)
            {
                directoryNames[i] = Path.GetFileName(paths[i]);
            }

            return directoryNames;
        }
        public string[] GetFiles(string directory)
        {
            string directoryPath = Path.Combine(rootDirectory, directory);
            return PathsToNames(Directory.GetFiles(directoryPath));
        }
        public bool DirectoryExists(string directory)
        {
            return Directory.Exists(Path.Combine(rootDirectory, directory));
        }
        public void CreateDirectory(string directory)
        {
            string directoryPath = Path.Combine(rootDirectory, directory);

            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        }
        public string GetFilePath(string directory, string fileName)
        {
            string directoryPath = Path.Combine(rootDirectory, directory);
            return Path.Combine(directoryPath, fileName);
        }
        public abstract void SaveFile(string directory, string fileName, object data);
        public abstract bool LoadFile(string directory, string fileName, Type saveType, out object data);
    }
}