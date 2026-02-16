using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Spine.Unity;
public class VisualNovelManager : MonoBehaviour
{
    [Serializable]
    public class GalleryScene
    {
        public string Name;
        public string DialogueName;
        public List<GameObject> BackgroundList;
        public List<GameObject> SceneList;
        public List<GameObject> ClimaxList;
        public List<VnDialogueData> DialogueData;
        public List<AudioClip> LoopAudioClipList;
        public List<AudioClip> FinishAudioClipList;

    }

    [Serializable]
    public enum GalleryStep
    {
        STEP_BEGINNING = 0,
        STEP_MIDDLE = 1,
        STEP_LAST = 2
    }

    [SerializeField] private Transform gallerySceneContainer;
    [SerializeField] private GameObject hudCanvasObj;
    [SerializeField] private List<GalleryScene> galleryScenesList;
    [SerializeField] private Button closeGalleryBtn;
    [SerializeField] private GameObject whiteScreenVFX;
    [SerializeField] private GameObject audioSource;

    private CharVnAnimationController charVnAnimationController;
    private TextVnDialogueController textVnDialogueController;

    private string curVnSceneName;
    private int curGallerySceneId;

    public void Init()
    {

    }

    public List<GalleryScene> GetGalleryScenes()
    {
        List<GalleryScene> sceneList = new List<GalleryScene>();
        for (int i = 0; i < sceneList.Count; i++)
        {
            sceneList.Add(galleryScenesList[i]);
        }
        return sceneList;
    }

    public void QuitGallery()
    {
        //SceneManager.LoadScene((int)GameEnums.SceneOrder.SHOP);
        //UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();
    }

    private void HideGallery()
    {
        for (int i = 0; i < galleryScenesList.Count; i++)
        {
            for (int j = 0; j < galleryScenesList[i].SceneList.Count; j++)
            {
                galleryScenesList[i].BackgroundList[j].SetActive(false);
                galleryScenesList[i].SceneList[j].SetActive(false);
            }
            for (int j = 0; j < galleryScenesList[i].ClimaxList.Count; j++)
            {
                galleryScenesList[i].ClimaxList[j].SetActive(false);
            }
        }
    }

    private void DisplayGallery()
    {
        HideGallery();
        Debug.Log(curVnSceneName);
        for (int i = 0; i < galleryScenesList.Count; i++)
        {
            if (galleryScenesList[i].Name.Contains(curVnSceneName))
            {
                galleryScenesList[i].BackgroundList[curGallerySceneId].SetActive(true);
                galleryScenesList[i].SceneList[curGallerySceneId].SetActive(true);
                textVnDialogueController.Init(galleryScenesList[i].DialogueData[curGallerySceneId]);
                
                SkeletonAnimation animation = galleryScenesList[i].SceneList[curGallerySceneId].GetComponent<SkeletonAnimation>();
                if(animation != null)
                {
                    charVnAnimationController.Init(animation);
                }
                else {
                    charVnAnimationController.Init(true);
                }
                break;
            }
        }

    }

    public void StartGalleryDialogueFirstTime()
    {
        UpdateGalleryStep(GalleryStep.STEP_BEGINNING);
     //   DisplayDialogue();
    }

    public void UpdateGalleryStep(GalleryStep _step)
    {
        switch (_step)
        {
            case GalleryStep.STEP_BEGINNING:
                textVnDialogueController.UpdateDialogueStep(GalleryStep.STEP_BEGINNING);
                break;
            case GalleryStep.STEP_MIDDLE:
                textVnDialogueController.UpdateDialogueStep(GalleryStep.STEP_MIDDLE);
                //PlayLoopAudio();
                break;
            case GalleryStep.STEP_LAST:
                //PlayFinishAudio
                //charVnAnimationController.PlayClimaxAnimation();
                textVnDialogueController.UpdateDialogueStep(GalleryStep.STEP_LAST);
                break;

        }
        DisplayDialogue();
    }

    public void UpdatePostDialogueState(GalleryStep _step){
        switch (_step)
        {
            case GalleryStep.STEP_BEGINNING:
                charVnAnimationController.PlayAnimation("Walk");
                UpdateGalleryStep(GalleryStep.STEP_MIDDLE);
                break;
            case GalleryStep.STEP_MIDDLE:
                charVnAnimationController.PlayAnimation("Idle");
                UpdateGalleryStep(GalleryStep.STEP_LAST);
                break;
            case GalleryStep.STEP_LAST:
                QuitGallery();
                break;
        }
    }

    public void DisplayDialogue() { textVnDialogueController.DisplayDialogue(); }

    #region UNITY
    private void Awake()
    {
       // charVnAnimationController = FindObjectOfType<CharVnAnimationController>();
       // textVnDialogueController = FindObjectOfType<TextVnDialogueController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerGallery gallery = PuzzleDataMiner.GetGallery();
        curVnSceneName = gallery.SceneName;
        curGallerySceneId = gallery.SceneId;

        Init();
        DisplayGallery();

        StartGalleryDialogueFirstTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if(Application.platform == RuntimePlatform.WindowsEditor ||
               Application.platform == RuntimePlatform.OSXEditor ||
               Application.platform == RuntimePlatform.WindowsEditor)
            {
                if(textVnDialogueController.GetCurDialogueStep() != GalleryStep.STEP_BEGINNING)
                {
                    if (hudCanvasObj.activeSelf) hudCanvasObj.SetActive(false);
                    else hudCanvasObj.SetActive(true);
                }
            }
        }
    }
    #endregion
}
