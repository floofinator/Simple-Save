using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Floofinator.SimpleSave
{
    public abstract class Filer
    {
        protected string rootDirectory;
        public Filer() : this("") { }
        public Filer(string root)
        {
            SetRoot(root);
        }
        public void SetRoot(string root) { rootDirectory = Path.Combine(Application.persistentDataPath, root); }
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