using UnityEngine;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace PuzzleHen.Data
{
    [CreateAssetMenu(fileName = "PuzzleDataGirl", menuName = "Puzzle/PuzzleDataGirl")]
    public class PuzzleDataGirl : ScriptableObject
    {
        public string ID;
        public Sprite Cover, CoverGallery;
        public List<PuzzleDataList> GirlList = new List<PuzzleDataList>();
        public string SourcePath = "Assets/Sprites/SUCCUBUS";
        public string DestinationPath = "Assets/Data";

        public int Count => GirlList.Count;

        public string GetProgress()
        {
            int totalQuest = 0;
            int completedQuest = 0;
            foreach (PuzzleDataList data in GirlList)
            {
                foreach(var item in data.List)
                {
                    totalQuest++;
                    if(item.IsCompleted) completedQuest++;
                }
            }

            return "(" + completedQuest.ToString() + "/" + totalQuest.ToString() + ")";
        }

        public int TotalQuest()
        {
            int totalQuest = 0;
            foreach (PuzzleDataList data in GirlList)
            {
                foreach (var item in data.List)
                {
                    totalQuest++;
                }
            }

            return totalQuest;
        }

#if UNITY_EDITOR
        public void ProcessAllData()
        {
            if (string.IsNullOrEmpty(SourcePath) || string.IsNullOrEmpty(DestinationPath))
            {
                Debug.LogError("Source or Destination path is empty!");
                return;
            }

            GirlList.Clear();

            ID = this.name;

            string rootFolderName = new DirectoryInfo(SourcePath).Name;
            string finalDestRoot = Path.Combine(DestinationPath, rootFolderName).Replace("\\", "/");

            EnsureFolderExists(DestinationPath, rootFolderName);

            string[] sourceSubfolders = Directory.GetDirectories(SourcePath);

            for (int i = 0; i < sourceSubfolders.Length; i++)
            {
                string sourceSubPath = sourceSubfolders[i].Replace("\\", "/");
                string subFolderName = new DirectoryInfo(sourceSubPath).Name;

                EditorUtility.DisplayProgressBar("Processing Puzzles", $"Setting up: {subFolderName}", (float)i / sourceSubfolders.Length);

                // 1. Path where individual PuzzleData assets are stored
                EnsureFolderExists(finalDestRoot, subFolderName);
                string subDestPath = Path.Combine(finalDestRoot, subFolderName).Replace("\\", "/");

                // 2. Create/Get PuzzleDataList inside the Root mirrored folder
                PuzzleDataList currentList = GetOrCreateAsset<PuzzleDataList>(finalDestRoot, subFolderName + "_List");
                currentList.ID = subFolderName + "_List";

                // 3. FIX: Point the List's internal Path to the folder containing its PuzzleData
                currentList.Path = subDestPath;
                currentList.List.Clear();

                // 4. Process Images and Fill List
                string[] imageFiles = Directory.GetFiles(sourceSubPath, "*.*", SearchOption.TopDirectoryOnly);
                foreach (string file in imageFiles)
                {
                    if (file.EndsWith(".meta")) continue;

                    string relativeImagePath = file.Replace("\\", "/");
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(relativeImagePath);

                    if (sprite != null)
                    {
                        PuzzleData data = GetOrCreateAsset<PuzzleData>(subDestPath, "Data_" + sprite.name);
                        data.Preview = sprite;
                        data.NumSlices = 4;
                        data.SliceImage(); // Uses the improved cleanup logic from our previous step

                        currentList.List.Add(data);
                    }
                }

                EditorUtility.SetDirty(currentList);
                GirlList.Add(currentList);
            }

            EditorUtility.ClearProgressBar();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Batch Process Complete. All Lists now point to their respective subfolders.");
        }

        private void EnsureFolderExists(string parent, string folderName)
        {
            string fullPath = Path.Combine(parent, folderName).Replace("\\", "/");
            if (!AssetDatabase.IsValidFolder(fullPath))
            {
                AssetDatabase.CreateFolder(parent, folderName);
            }
        }

        private T GetOrCreateAsset<T>(string path, string name) where T : ScriptableObject
        {
            string fullPath = Path.Combine(path, name + ".asset").Replace("\\", "/");
            T asset = AssetDatabase.LoadAssetAtPath<T>(fullPath);

            if (asset == null)
            {
                asset = CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, fullPath);
            }
            return asset;
        }
#endif
    }
}