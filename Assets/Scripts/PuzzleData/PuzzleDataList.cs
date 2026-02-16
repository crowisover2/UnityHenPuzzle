using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PuzzleDataList", menuName = "Puzzle/DataList")]
public class PuzzleDataList : ScriptableObject
{
    [SerializeField] private Sprite thumbnail;
    [SerializeField] private List<ImagePuzzleData> list;

    public int Count
    {
        get { return list.Count; }
    }

    public Sprite GetThumbnail() { return thumbnail; }

    public ImagePuzzleData GetPuzzle(int _indexPuzzle)
    {
        return list[_indexPuzzle];
    }

    public Sprite GetPreviewAt(int _index)
    {
        return list[_index].Preview;
    }

    public Sprite GetPiecesAt(int _indexImage, int _indexPieces)
    {
        return list[_indexImage].Pieces[_indexPieces];
    }

    public PlayerGallery GetGalleryAt(int _indexImage)
    {
        return list[_indexImage].gallery;
    }
}
