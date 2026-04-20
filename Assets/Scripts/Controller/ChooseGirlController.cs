using JetBrains.Annotations;
using NUnit.Framework;
using PuzzleHen.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PuzzleHen.ChooseGirl
{
    public class ChooseGirlController : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private GameObject panelGirl;
        [SerializeField] private GameObject panelChallenge;

        [Header("Panel Challenge Setting")]
        [SerializeField] private Transform parentBtnChallenge;
        [SerializeField] private Text txtDisplayName;
        [SerializeField] private Button prefabChallenge;
        [SerializeField] private Button btnBackCha;

        [Header("Panel Girl Setting")]
        [SerializeField] private Transform parentBtnGirl;
        [SerializeField] private ButtonGirlProperties prefabGirl;
        [SerializeField] private Button btnBackGirl;

        private int girlID;

        private List<Button> btnChallenges = new();

        #region Unity
        public virtual void Start()
        {
            InitPanelGirl();
            InitChallenge();
        }

        private void Update()
        {
            // coinTxt.text = CoinManager.Coins + "";
        }
        #endregion

        public virtual Sprite GetSprite(PuzzleDataGirl _girl) => _girl.Cover;
        public virtual string SetStringToDisplay(int _index)=> PuzzleDataMiner.GetOneGirlProgression(_index);

        public virtual bool Unlock(PuzzleData _data)
        {
            return _data.IsUnlock;
        }

        private void InitPanelGirl()
        {
            btnBackGirl.onClick.AddListener(() => {
                BackToMenu();
            });

            for (int i = 0; i < PuzzleDataMiner.Count; i++)
            {
                int index = i;
                PuzzleDataGirl girl = PuzzleDataMiner.GetGirls(index);

                ButtonGirlProperties button = Instantiate(prefabGirl, parentBtnGirl);

                string percentage = SetStringToDisplay(index);

                button.SetName(percentage);
                button.SetSprite(GetSprite(girl));

                button.AddListener(() => {
                    panelGirl.SetActive(false);
                    
                    girlID = index;
                    
                    InitChallenge2();
                    panelChallenge.SetActive(true);
                });
            }

            panelGirl.SetActive(true);
        }
        private void InitChallenge()
        {
            btnBackCha.onClick.AddListener(() => {
                panelChallenge.SetActive(false);
                panelGirl.SetActive(true);
            });
        }
        private void InitChallenge2()
        {
            // 1. Cache the data once to avoid repeated calls
            PuzzleDataGirl girl = PuzzleDataMiner.GetGirls(girlID);

            txtDisplayName.text = girl.ID;

            int totalNeeded = girl.TotalQuest();
            int buttonIndex = 0;

            // 2. Iterate through the nested data
            for (int i = 0; i < girl.GirlList.Count; i++)
            {
                int index = i;
                var currentGirlItem = girl.GirlList[i];

                for (int j = 0; j < currentGirlItem.List.Count; j++)
                {
                    int index2 = j;
                    PuzzleData here = currentGirlItem.List[j];
                    Button template;

                    // Check if we can reuse an existing button or need to make a new one
                    if (buttonIndex < btnChallenges.Count)
                    {
                        template = btnChallenges[buttonIndex];
                        template.gameObject.SetActive(true);
                    }
                    else
                    {
                        template = Instantiate(prefabChallenge, parentBtnChallenge);
                        btnChallenges.Add(template);
                    }

                    // 3. Update the button state
                    template.image.sprite = here.Preview;
                    template.onClick.RemoveAllListeners();
                    template.onClick.AddListener(() =>
                    {
                        PuzzleDataMiner.SetCurrent(here, girlID, index, index2);
                        SelectPuzzle();
                    });

                    template.interactable = Unlock(here);

                    buttonIndex++;
                }
            }

            // 4. Cleanup: Hide any buttons that are leftover (if total < btnChallenges.Count)
            for (int i = buttonIndex; i < btnChallenges.Count; i++)
            {
                if (btnChallenges[i].gameObject.activeSelf)
                {
                    btnChallenges[i].gameObject.SetActive(false);
                }
            }
        }

        public void BackToMenu()
            => SceneManager.LoadScene(Constants.SceneMenu);
            //=> Debug.Log("To Menu");

        public virtual void SelectPuzzle()
            //=> SceneManager.LoadScene(Constants.SceneGame);
            => Debug.Log("Play" + PuzzleDataMiner.Current.ID);
    }
}

