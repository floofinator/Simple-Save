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
        public string Root
        public Filer() : this("") { }
        public Filer(string root)
        {
            Root = root;
        }
        string GetSystemRoot()
        {
            return Path.Combine(Application.persistentDataPath, "SaveData", Root);
        }
        public void Compress()
        {
            if (!Directory.Exists(GetSystemRoot())) return;
            string directory = Path.GetDirectoryName(GetSystemRoot());
            string name = Path.GetFileName(GetSystemRoot());
            string fileName = Path.Combine(directory, name + ".save");

            if (File.Exists(fileName)) File.Delete(fileName);

            ZipFile.CreateFromDirectory(GetSystemRoot(), fileName, System.IO.Compression.CompressionLevel.Fastest, false);
            
            Directory.Delete(GetSystemRoot(), true);
        }
        public void UnCompress()
        {
            string directory = Path.GetDirectoryName(GetSystemRoot());
            string name = Path.GetFileName(GetSystemRoot());
            string fileName = Path.Combine(directory, name + ".save");

            if (!File.Exists(fileName)) return;
            if (Directory.Exists(GetSystemRoot())) Directory.Delete(GetSystemRoot(), true);

            ZipFile.ExtractToDirectory(fileName, GetSystemRoot());
        }
        public void DeleteUnCompress()
        {
            if (Directory.Exists(GetSystemRoot())) Directory.Delete(GetSystemRoot(), true);
        }
        public void DeleteFile(string directory, string fileName)
        {
            string directoryPath = Path.Combine(GetSystemRoot(), directory);
            string filePath = Path.Combine(directoryPath, fileName);

            if (File.Exists(filePath)) File.Delete(filePath);

            Debug.Log($"Deleted file from \"{directoryPath}\"");
        }
        public bool FileExists(string directory, string fileName)
        {
            return File.Exists(Path.Combine(GetSystemRoot(), directory, fileName));
        }
        public void RenameFile(string directory, string fileName, string newName)
        {
            if (File.Exists(Path.Combine(GetSystemRoot(), directory, fileName)))
            {
                File.Move(Path.Combine(GetSystemRoot(), directory, fileName), Path.Combine(GetSystemRoot(), directory, newName));
            }
        }
        public void DeleteDirectory(string directory)
        {
            string directoryPath = Path.Combine(GetSystemRoot(), directory);

            if (Directory.Exists(directoryPath)) Directory.Delete(directoryPath, true);
        }
        public string[] GetDirectories(string directory)
        {
            string directoryPath = Path.Combine(GetSystemRoot(), directory);

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
            string directoryPath = Path.Combine(GetSystemRoot(), directory);
            return PathsToNames(Directory.GetFiles(directoryPath));
        }
        public bool DirectoryExists(string directory)
        {
            return Directory.Exists(Path.Combine(GetSystemRoot(), directory));
        }
        public void CreateDirectory(string directory)
        {
            string directoryPath = Path.Combine(GetSystemRoot(), directory);

            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        }
        public string GetFilePath(string directory, string fileName)
        {
            string directoryPath = Path.Combine(GetSystemRoot(), directory);
            return Path.Combine(directoryPath, fileName);
        }
        public abstract void SaveFile(string directory, string fileName, object data);
        public abstract bool LoadFile(string directory, string fileName, Type saveType, out object data);
    }
}