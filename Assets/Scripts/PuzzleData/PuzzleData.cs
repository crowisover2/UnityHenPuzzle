using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PuzzleHen.Data
{
    [CreateAssetMenu(fileName = "PuzzleData", menuName = "Puzzle/PuzzleData")]
    public class PuzzleData : ScriptableObject
    {
        public string ID;
        public Sprite Preview;
        public List<Sprite> Pieces = new List<Sprite>();
        public int NumSlices = 4; // Default to 4 for a 4x4 grid
        public bool IsUnlock { set; get; }
        public bool IsCompleted { set; get; }

#if UNITY_EDITOR
        public void SliceImage()
        {
            if (Preview == null) return;

            ID = this.name;

            // 1. PHYSICAL CLEANUP: Find and remove all existing sub-assets from the file
            string assetPath = AssetDatabase.GetAssetPath(this);
            Object[] allAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);

            foreach (Object asset in allAssets)
            {
                // Don't destroy the ScriptableObject itself! 
                // We only want to destroy the Sprites (sub-assets)
                if (asset is Sprite)
                {
                    AssetDatabase.RemoveObjectFromAsset(asset);
                    DestroyImmediate(asset, true);
                }
            }

            // Clear the logical list
            Pieces.Clear();

            // 2. TEXTURE PREPARATION
            Texture2D tex = Preview.texture;
            string texPath = AssetDatabase.GetAssetPath(tex);
            TextureImporter importer = AssetImporter.GetAtPath(texPath) as TextureImporter;

            if (importer != null && !importer.isReadable)
            {
                importer.isReadable = true;
                importer.SaveAndReimport();
            }

            // 3. SLICING LOGIC
            float width = tex.width / (float)NumSlices;
            float height = tex.height / (float)NumSlices;

            for (int y = NumSlices - 1; y >= 0; y--)
            {
                for (int x = 0; x < NumSlices; x++)
                {
                    Rect rect = new Rect(x * width, y * height, width, height);
                    Sprite newPiece = Sprite.Create(tex, rect, new Vector2(0.5f, 0.5f));
                    newPiece.name = $"{Preview.name}_{Pieces.Count}";

                    // Add the new sprite back into the asset database
                    AssetDatabase.AddObjectToAsset(newPiece, this);
                    Pieces.Add(newPiece);
                }
            }

            // 4. FINALIZATION
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif
    }

    // Custom Editor to show the button
#if UNITY_EDITOR
    [CustomEditor(typeof(PuzzleData))]
    public class PuzzleDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            PuzzleData data = (PuzzleData)target;

            if (GUILayout.Button("Slice Preview into Pieces"))
            {
                EditorUtility.DisplayProgressBar("Slicing Puzzle", "Generating sprites...", 0.5f);
                data.SliceImage();
                EditorUtility.ClearProgressBar();
            }
        }
    }
#endif
}