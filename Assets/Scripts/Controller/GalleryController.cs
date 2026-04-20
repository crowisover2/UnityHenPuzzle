using PuzzleHen.ChooseGirl;
using PuzzleHen.Data;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PuzzleHen.Gallery
{
    public class GalleryController : ChooseGirlController
    {
        [Header("Display Image")]
        [SerializeField] private Image displayTV;
        [SerializeField] private Button closeTV;

        public override void Start()
        {
            base.Start();

            closeTV.onClick.AddListener(() => { 
                displayTV.gameObject.SetActive(false);
            });
        }
        public override Sprite GetSprite(PuzzleDataGirl _girl) => _girl.CoverGallery;
        public override string SetStringToDisplay(int _index) => PuzzleDataMiner.GetGirls(_index).ID;

        public override void SelectPuzzle()
        {
            displayTV.sprite = PuzzleDataMiner.Current.Preview;
            displayTV?.gameObject.SetActive(true);
        }
        public override bool Unlock(PuzzleData _data)
        {
            return _data.IsCompleted;
        }
    }
}
