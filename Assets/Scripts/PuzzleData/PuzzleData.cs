using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ImagePuzzleData", menuName = "Puzzle/ImageData")]
public class ImagePuzzleData : ScriptableObject
{
    public Sprite Icon;
    public int Price;
    public bool IsUnlock;
    public Sprite Preview;
    public List<Sprite> Pieces;
    public PlayerGallery gallery;

    public ImagePuzzleData(int _price = 100, bool _isUnlock = false)
    {
        Price = _price;
        IsUnlock = _isUnlock;
    }
}

[Serializable]
public class PlayerGallery
{
    public string SceneName;
    public int SceneId;
   // public int HasUnlockScene;
  //  public int HasWatchScene;
}