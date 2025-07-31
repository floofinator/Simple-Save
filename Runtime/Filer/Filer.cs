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
        public string Root;
        string persistentDataPath;
        public Filer() : this("") { }
        public Filer(string root)
        {
            Root = root;
            persistentDataPath = Application.persistentDataPath;
        }
        string RootPath => Path.Combine(persistentDataPath, "SaveData", Root);
        public void Compress()
        {
            if (!Directory.Exists(RootPath))
            {
                Debug.Log("Nothing to compress");
                return;
            }

            Debug.Log("Compress");

            string directory = Path.GetDirectoryName(RootPath);
            string name = Path.GetFileName(RootPath);
            string fileName = Path.Combine(directory, name + ".save");

            if (File.Exists(fileName)) File.Delete(fileName);

            ZipFile.CreateFromDirectory(RootPath, fileName, System.IO.Compression.CompressionLevel.Fastest, false);

            Directory.Delete(RootPath, true);
        }
        public void UnCompress()
        {
            string directory = Path.GetDirectoryName(RootPath);
            string name = Path.GetFileName(RootPath);
            string fileName = Path.Combine(directory, name + ".save");

            if (!File.Exists(fileName))
            {   
                Debug.Log("Nothing to uncompress");
                return;
            }

            Debug.Log("UnCompress");

            if (Directory.Exists(RootPath)) Directory.Delete(RootPath, true);

            ZipFile.ExtractToDirectory(fileName, RootPath);
        }
        public void DeleteUnCompress()
        {
            if (Directory.Exists(RootPath))
            {
                Directory.Delete(RootPath, true);
                Debug.Log("Delete UnCompress");
            }
            else
            {
                Debug.Log("No UnCompress to Delete");
            }
        }
        public void DeleteFile(string directory, string fileName)
        {
            string directoryPath = Path.Combine(RootPath, directory);
            string filePath = Path.Combine(directoryPath, fileName);

            if (File.Exists(filePath)) File.Delete(filePath);

            Debug.Log($"Deleted file from \"{directoryPath}\"");
        }
        public bool FileExists(string directory, string fileName)
        {
            return File.Exists(Path.Combine(RootPath, directory, fileName));
        }
        public void RenameFile(string directory, string fileName, string newName)
        {
            if (File.Exists(Path.Combine(RootPath, directory, fileName)))
            {
                File.Move(Path.Combine(RootPath, directory, fileName), Path.Combine(RootPath, directory, newName));
            }
        }
        public void DeleteDirectory(string directory)
        {
            string directoryPath = Path.Combine(RootPath, directory);

            if (Directory.Exists(directoryPath)) Directory.Delete(directoryPath, true);
        }
        public string[] GetDirectories(string directory)
        {
            string directoryPath = Path.Combine(RootPath, directory);

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
            string directoryPath = Path.Combine(RootPath, directory);
            return PathsToNames(Directory.GetFiles(directoryPath));
        }
        public bool DirectoryExists(string directory)
        {
            return Directory.Exists(Path.Combine(RootPath, directory));
        }
        public void CreateDirectory(string directory)
        {
            string directoryPath = Path.Combine(RootPath, directory);

            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        }
        public string GetFilePath(string directory, string fileName)
        {
            string directoryPath = Path.Combine(RootPath, directory);
            return Path.Combine(directoryPath, fileName);
        }
        public abstract void SaveFile(string directory, string fileName, object data);
        public abstract bool LoadFile(string directory, string fileName, Type saveType, out object data);
    }
}