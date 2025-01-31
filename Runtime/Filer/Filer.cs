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
        public string Root {get; private set;}
        public Filer() : this("") { }
        public Filer(string root)
        {
            SetRoot(root);
        }
        public void SetRoot(string root) { Root = Path.Combine(Application.persistentDataPath, "SaveData", root); }
        public void Compress()
        {
            if (!Directory.Exists(Root)) return;
            string directory = Path.GetDirectoryName(Root);
            string name = Path.GetFileName(Root);
            string fileName = Path.Combine(directory, name + ".save");

            if (File.Exists(fileName)) File.Delete(fileName);

            ZipFile.CreateFromDirectory(Root, fileName, System.IO.Compression.CompressionLevel.Fastest, false);
            
            // Directory.Delete(Root, true);
        }
        public void UnCompress()
        {
            string directory = Path.GetDirectoryName(Root);
            string name = Path.GetFileName(Root);
            string fileName = Path.Combine(directory, name + ".save");

            if (!File.Exists(fileName)) return;
            if (Directory.Exists(Root)) Directory.Delete(Root, true);

            ZipFile.ExtractToDirectory(fileName, Root);
        }
        public void DeleteUnCompress()
        {
            if (Directory.Exists(Root)) Directory.Delete(Root, true);
        }
        public void DeleteFile(string directory, string fileName)
        {
            string directoryPath = Path.Combine(Root, directory);
            string filePath = Path.Combine(directoryPath, fileName);

            if (File.Exists(filePath)) File.Delete(filePath);

            Debug.Log($"Deleted file from \"{directoryPath}\"");
        }
        public bool FileExists(string directory, string fileName)
        {
            return File.Exists(Path.Combine(Root, directory, fileName));
        }
        public void RenameFile(string directory, string fileName, string newName)
        {
            if (File.Exists(Path.Combine(Root, directory, fileName)))
            {
                File.Move(Path.Combine(Root, directory, fileName), Path.Combine(Root, directory, newName));
            }
        }
        public void DeleteDirectory(string directory)
        {
            string directoryPath = Path.Combine(Root, directory);

            if (Directory.Exists(directoryPath)) Directory.Delete(directoryPath, true);
        }
        public string[] GetDirectories(string directory)
        {
            string directoryPath = Path.Combine(Root, directory);

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
            string directoryPath = Path.Combine(Root, directory);
            return PathsToNames(Directory.GetFiles(directoryPath));
        }
        public bool DirectoryExists(string directory)
        {
            return Directory.Exists(Path.Combine(Root, directory));
        }
        public void CreateDirectory(string directory)
        {
            string directoryPath = Path.Combine(Root, directory);

            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        }
        public string GetFilePath(string directory, string fileName)
        {
            string directoryPath = Path.Combine(Root, directory);
            return Path.Combine(directoryPath, fileName);
        }
        public abstract void SaveFile(string directory, string fileName, object data);
        public abstract bool LoadFile(string directory, string fileName, Type saveType, out object data);
    }
}