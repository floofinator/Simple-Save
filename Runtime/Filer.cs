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
        public void SetRoot(string root) { rootDirectory = root; }
        public void DeleteFile(string directory, string fileName)
        {
            string directoryPath = Path.Combine(rootDirectory, directory);
            string filePath = Path.Combine(directoryPath, fileName);

            if (File.Exists(filePath)) File.Delete(filePath);

            Debug.Log($"Deleted file from {directoryPath}.");
        }
        public bool FileExists(string directory, string fileName)
        {
            return File.Exists(Path.Combine(rootDirectory, directory, fileName));
        }
        public void DeleteDirectory(string directory)
        {
            string directoryPath = Path.Combine(rootDirectory, directory);

            if (Directory.Exists(directoryPath)) Directory.Delete(directoryPath, true);

            Debug.Log($"Deleted directory {directoryPath}.");
        }
        public bool DirectoryExists(string directory)
        {
            return Directory.Exists(Path.Combine(rootDirectory, directory));
        }
        public void CreateDirectory(string directory)
        {
            string directoryPath = Path.Combine(rootDirectory, directory);

            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

            Debug.Log($"Created directory {directoryPath}.");
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