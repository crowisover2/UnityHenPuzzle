using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDataMiner : MonoBehaviour
{
    private static PuzzleDataMiner instance;

    [SerializeField] private List<PuzzleDataList> dataList;
    [SerializeField] private int choosenGirl;
    [SerializeField] private int choosenPic;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);

        choosenGirl = 0;
        choosenPic = 0;
    }

    #region Choose Girl
    public static int GetGirlCount() { return instance.dataList.Count; }
    public static PuzzleDataList GetGirlAt(int _index) { return instance.dataList[_index]; }
    public static void SetGirl(int _choosenGirl)
    {
        instance.choosenGirl = _choosenGirl;
    }

    public static string GetOneGirlProgression(int _choosenGirl)
    {
        float percentUnlock = 0;
        PuzzleDataList item = GetGirlAt(_choosenGirl);
        for (int i = 0; i < item.Count; i++)
        {
            bool isTrue = item.GetPuzzle(i).IsUnlock;
            if (isTrue) percentUnlock++;
        }


        float percentage = 100.0f * percentUnlock / item.Count;
        return percentage + "";
    }
    #endregion

    #region Game
    public static Sprite GetPreview() { 
        return instance.dataList[instance.choosenGirl].GetPreviewAt(instance.choosenPic);
    }

    public static Sprite GetPiecesAt(int _indexPieces)
    {
        return instance.dataList[instance.choosenGirl].GetPiecesAt(instance.choosenPic, _indexPieces);
    }
    #endregion

    #region For Shop
    public static int GetPuzzleCount() { return instance.dataList[instance.choosenGirl].Count; }
    public static void SetPic(int _choosenPic)
    {
        instance.choosenPic = _choosenPic;
    }
    public static ImagePuzzleData GetPuzzle(int _indexPuzzle)
    {
        return instance.dataList[instance.choosenGirl].GetPuzzle(_indexPuzzle);
    }
    #endregion

    #region VisualNovel
    public static PlayerGallery GetGallery()
    {
        return instance.dataList[instance.choosenGirl].GetGalleryAt(instance.choosenPic);
    }
    #endregion
}
