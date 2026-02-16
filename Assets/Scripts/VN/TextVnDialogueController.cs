using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextVnDialogueController : MonoBehaviour
{
    [SerializeField] private GameObject dialogueUIObj;
    [SerializeField] private GameObject introBlackObj;
    [SerializeField] private TextMeshProUGUI actorNameTxt;
    [SerializeField] private TextMeshProUGUI actorDialogueTxt;
    [SerializeField] private Button nextDialogueBtn;

    private VisualNovelManager visualNovelManager;
    private VisualNovelManager.GalleryStep curDialogueStep;
    private int curDialogueIndex = 0;
    private VnDialogueData curDialogueData;
    private Coroutine revealTextRoutine;
    private bool isRevealingText;

    public VisualNovelManager.GalleryStep GetCurDialogueStep() { return curDialogueStep;  }

    public void Init(VnDialogueData _dialogueData)
    {
        //visualNovelManager = FindObjectOfType<VisualNovelManager>(true);
        curDialogueData = _dialogueData;

        nextDialogueBtn.onClick.AddListener(delegate {
            if (!isRevealingText)
            {
                curDialogueIndex++;
            }
            DisplayDialogue();
        });
    }
    public void UpdateDialogueStep(VisualNovelManager.GalleryStep _step) { curDialogueStep = _step; }

    public void DisplayDialogue()
    {
        nextDialogueBtn.gameObject.SetActive(true);
        if (isRevealingText)
        {
            isRevealingText = false;
            StopCoroutine(revealTextRoutine);
            actorDialogueTxt.text = GetChangedDialogue(curDialogueData.GetGalleryDialogue(curDialogueStep).DialogueList[curDialogueIndex].ActorDialogue);
        }
        else
        {
            if(curDialogueStep == VisualNovelManager.GalleryStep.STEP_BEGINNING)
            {
                introBlackObj.SetActive(true);
            }
            else { introBlackObj.gameObject.SetActive(false); }

            if(curDialogueIndex >= curDialogueData.GetGalleryDialogue(curDialogueStep).DialogueList.Count)
            {
                curDialogueIndex = 0;
                visualNovelManager.UpdatePostDialogueState(curDialogueStep);
            }
            else
            {
                dialogueUIObj.SetActive(true);
                actorNameTxt.text = curDialogueData.GetGalleryDialogue(curDialogueStep).DialogueList[curDialogueIndex].ActorName;
                revealTextRoutine = StartCoroutine(ShowText(GetChangedDialogue(curDialogueData.GetGalleryDialogue(curDialogueStep).DialogueList[curDialogueIndex].ActorDialogue)));
            }
        }
    }

    private string GetChangedDialogue(string _dialogueTxt)
    {
        string changedDialogue = _dialogueTxt;
     //   string mcName = SPrefs.GetString("MC" + PlayerPrefsData.SaveName, "Kazuma");
     //   changedDialogue = changedDialogue.Replace("MC", mcName);
        List<VisualNovelManager.GalleryScene> galleryGirlList = visualNovelManager.GetGalleryScenes();
        for (int i = 0; i < galleryGirlList.Count; i++)
        {
            changedDialogue = changedDialogue.Replace(galleryGirlList[i].Name, galleryGirlList[i].DialogueName);
        }
        return changedDialogue;
    }
    //public void HideDialogue()
    //{
    //    dialogueUIObj.SetActive(true);
    //    nextDialogueBtn.gameObject.SetActive(false);
    //}
    private IEnumerator ShowText(string _dialogue)
    {
        isRevealingText = true;
        for (int i = 0; i < _dialogue.Length; i++)
        {
            actorDialogueTxt.text = _dialogue.Substring(0,i);
            yield return new WaitForSeconds(0.05f);
        }
        isRevealingText = false;
    }
}
