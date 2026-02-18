using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class ChooseGirlController : MonoBehaviour
{
        /*
    [Space(5)]
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform containerButton;

    [Space(5)]
    [Header("Total Coin")]
    [SerializeField] private Text coinTxt;
    [SerializeField] private Button btnTopUP;

    [Space(5)]
    [SerializeField] private Button btnBack;

    private void CreateButton()
    {
        for (int i = 0; i < PuzzleDataMiner.GetGirlCount(); i++)
        {
            int index = i;
            PuzzleDataList girl = PuzzleDataMiner.GetGirlAt(index);

            ButtonTopUp button = Instantiate(buttonPrefab, containerButton).GetComponent<ButtonTopUp>();

            string percentage = PuzzleDataMiner.GetOneGirlProgression(index);

            button.SetText(percentage);
            button.SetSprite(girl.GetThumbnail());
            button.AddListener(() => {
                SelectPuzzle(index);
            });

            //buttons.Add(button);
        }

        btnBack.onClick.AddListener(() => { BackToMenu(); });
        //btnTopUP.onClick.AddListener(() => { SceneManager.LoadScene((int)GameEnums.SceneOrder.TOP_UP); });
    }

    public void BackToMenu()
    {
       // SceneManager.LoadScene((int)GameEnums.SceneOrder.MENU);
    }

    public void SelectPuzzle(int _index)
    {
        PuzzleDataMiner.SetGirl(_index);
       // SceneManager.LoadScene((int)GameEnums.SceneOrder.SHOP);
    }

    private void Start()
    {
        CreateButton();
    }

    private void Update()
    {
       // coinTxt.text = CoinManager.Coins + "";
    }*/
}

