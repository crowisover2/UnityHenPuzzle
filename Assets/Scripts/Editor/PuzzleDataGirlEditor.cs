#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace PuzzleHen.Data
{
    [CustomEditor(typeof(PuzzleDataGirl))]
    public class PuzzleDataGirlEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            PuzzleDataGirl script = (PuzzleDataGirl)target;

            GUILayout.Space(10);
            if (GUILayout.Button("Generate All Puzzle Data", GUILayout.Height(40)))
            {
                script.ProcessAllData();
            }
        }
    }
}
#endif