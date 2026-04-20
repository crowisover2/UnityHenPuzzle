using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using PuzzleHen.Data;

namespace PuzzleHen.CustomPlayerPref
{
    public static class SPrefsExampleUsage
    {
        // Static data container
        private static DataPuzzleHen _data = new();

        // Helper to ensure the directory exists
        private static string SavePath => Path.Combine(Application.persistentDataPath, "save");
        private static string FilePath => Path.Combine(SavePath, "savefile.txt");

        public static string GetString(string key) => _data.ListString.Find(x => x.key == key)?.value ?? string.Empty;

        public static void SetString(string key, string value)
        {
            var inString = _data.ListString.Find((x) => x.key == key);
            if (inString != null)
            {
                inString.value = value;
                return;
            }

            _data.ListString.Add(new CustomString(key, value));
        }

        public static void Load()
        {
            if (File.Exists(FilePath))
            {
                try
                {
                    string json = File.ReadAllText(FilePath);
                    DataPuzzleHen loadedData = JsonUtility.FromJson<DataPuzzleHen>(json);

                    if (loadedData != null)
                    {
                        _data = loadedData;
                        Debug.Log("Progression loaded successfully.");
                        return;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Load failed: {e.Message}");
                }
            }

            // Fallback if file is missing or corrupted
            FirstProgression();
        }

        //public static void Save()
        //{
        //    string tempPath = FilePath + ".tmp";
        //    string json = JsonUtility.ToJson(_data, true);

        //    Directory.CreateDirectory(tempPath);

        //    File.WriteAllText(tempPath, json);
        //    if (File.Exists(FilePath)) File.Delete(FilePath);
        //    File.Move(tempPath, FilePath);
        //}

        public static void Save()
        {
            string json = JsonUtility.ToJson(_data, true);

            // Fix: Create the directory containing the file, not a directory named after the file
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }

            if (File.Exists(FilePath))
            {
                string tempPath = SavePath + ".tmp";
                File.WriteAllText(tempPath, json);
                File.Delete(FilePath);
                File.Move(tempPath, FilePath);
            }
            else
            {
                File.WriteAllText(FilePath, json);
            }
        }

        public static bool HasKey(string key)
        {
            var inString = _data.ListString.Find((x) => x.key == key);
            if (inString != null) return true;

            return false;
        }

        public static void DeleteKey(string key)
        {
            var inString = _data.ListString.Find((x) => x.key == key);
            if (inString != null)
            {
                _data.ListString.Remove(inString);
            }
        }

        private static void FirstProgression()
        {
            _data = new DataPuzzleHen
            {
                ListString = new List<CustomString>(),
            };
            Debug.Log("New progression data initialized.");

            PuzzleDataMiner.InjectDataFirstProgression();
        }

        public static void DeleteAll()
        {
            _data.ListString.Clear();
            if (File.Exists(FilePath)) File.Delete(FilePath);
        }
    }

    [Serializable]
    public class DataPuzzleHen
    {
        // Fields must be public or [SerializeField] for JsonUtility
        public List<CustomString> ListString = new();
    }

    [Serializable]
    public class CustomString
    {
        // Changed to public so JsonUtility can see them
        public string key;
        public string value;

        public CustomString(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
    }
}