using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "VisualNovelMode/TextDialogue")]
public class VnDialogueData : ScriptableObject
{
        /*
    public List<TextDialogue> GalleryDialogueList;

    public TextDialogue GetGalleryDialogue(VisualNovelManager.GalleryStep _step)
    {
        TextDialogue textDialogue = null;
        for (int i = 0; i < GalleryDialogueList.Count; i++)
        {
            if (GalleryDialogueList[i].DialogueStep == _step) { textDialogue = GalleryDialogueList[i]; }
        }
        return textDialogue;
    }
}

[Serializable]
public class TextDialogue
{
    public VisualNovelManager.GalleryStep DialogueStep;
    public List<DialogueData> DialogueList;
}
[Serializable]
public struct DialogueData
{
    public string ActorName;
    [TextArea(3, 10)] public string ActorDialogue;*/
}