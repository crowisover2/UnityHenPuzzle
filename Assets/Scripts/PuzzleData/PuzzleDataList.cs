using UnityEngine;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "PuzzleDataList", menuName = "Puzzle/PuzzleDataList")]
public class PuzzleDataList : ScriptableObject
{
    public List<PuzzleData> List = new List<PuzzleData>();
    public string Path = "Assets/Textures/Puzzles"; // Example path
    public bool IsUsingChildren = true;

#if UNITY_EDITOR
    public void CollectAndProcess()
    {
        List.Clear();

        // Find all PuzzleData assets in the path
        string[] guids = AssetDatabase.FindAssets("t:PuzzleData", new[] { Path });

        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);

            // Check if it's in a subfolder and if we should ignore it
            if (!IsUsingChildren)
            {
                string directory = System.IO.Path.GetDirectoryName(assetPath).Replace("\\", "/");
                if (directory != Path) continue;
            }

            PuzzleData data = AssetDatabase.LoadAssetAtPath<PuzzleData>(assetPath);
            if (data != null)
            {
                // Update progress bar
                float progress = (float)i / guids.Length;
                EditorUtility.DisplayProgressBar("Processing Puzzle List", $"Slicing: {data.name}", progress);

                // Automatically trigger the slice for each found asset
                data.SliceImage();
                List.Add(data);
            }
        }

        EditorUtility.ClearProgressBar();
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        Debug.Log($"[PuzzleDataList] Successfully processed {List.Count} puzzles.");
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(PuzzleDataList))]
public class PuzzleDataListEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        PuzzleDataList list = (PuzzleDataList)target;

        if (GUILayout.Button("Batch Collect & Slice Puzzles"))
        {
            list.CollectAndProcess();
        }
    }
}
#endif