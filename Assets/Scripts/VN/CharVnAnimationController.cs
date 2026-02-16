using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class CharVnAnimationController : MonoBehaviour
{
   // [SerializeField] private string idleAnimation;
   // [SerializeField] private string movingAnimation;
  //  [SerializeField] private string afterClimaxAnimation;
  //  [SerializeField] private float animSpeedIncrease = 0.5f;

    private VisualNovelManager visualNovelManager;
    [SerializeField] private SkeletonAnimation curVnSpine;
    private string spineTextId = "Level 3/Animation_Sprite_";

    private bool noSpine;

    public void Init(SkeletonAnimation _spineAnime)
    {
        curVnSpine = _spineAnime;
        //visualNovelManager = FindObjectOfType<VisualNovelManager>(true);
    }

    public void Init(bool _value)
    {
        noSpine = _value;
    }

    //public void IncreaseAnimationSpeed()
    //{
    //    curVnSpine.AnimationState.TimeScale += animSpeedIncrease;
    //}

    public void PlayAnimation(string temp)
    {
        if (noSpine) return;
        curVnSpine.AnimationState.SetAnimation(0, spineTextId + temp, true);
    }

    public void ResetAnimation()
    {
        curVnSpine.AnimationState.SetAnimation(0, spineTextId + "Idle", true);
        curVnSpine.AnimationState.TimeScale = 1;
    }
}
