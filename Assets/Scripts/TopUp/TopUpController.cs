using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class TopUpController : MonoBehaviour
{
    /*[Space(5)]
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform containerButton;

    [Space(5)]
    [Header("Data Top Up")]
    [SerializeField] private TopUpData topUpData;

    [Space(5)]
    [Header("Total Coin")]
    [SerializeField] private Text coinTxt;

    [Space(5)]
    [Header("Panel")]
    [SerializeField] private GameObject panelThankYou;
    [SerializeField] private GameObject panelBuy;
    [SerializeField] private Button btnYes;
    [SerializeField] private Button btnBack;

    //private List<ButtonTopUp> buttons = new List<ButtonTopUp>();

    private Action choosenAction;

    private void CreateButton()
    {
        for (int i = 0; i < topUpData.topUps.Count; i++)
        {
            int price = topUpData.topUps[i];
            ButtonTopUp button = Instantiate(buttonPrefab, containerButton).GetComponent<ButtonTopUp>();

            button.SetText("" + price);
            button.AddListener(() => {
                panelBuy.SetActive(true);
                choosenAction = new(() => { TopUp(price); });
            });

            //buttons.Add(button);
        }

        btnYes.onClick.AddListener(() => { Buy(); });
        btnBack.onClick.AddListener(() => { BackToMenu(); });
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene((int)GameEnums.SceneOrder.MENU);
    }

    private void TopUp(int _price)
    {
        CoinManager.AddCoins(_price);
    }

    public void Buy()
    {
        panelBuy.SetActive(false);
        choosenAction.Invoke();
        panelThankYou.SetActive(true);
    }


    private void Start()
    {
        CreateButton();
    }

    private void Update()
    {
        coinTxt.text = CoinManager.Coins + "";
    }*/
}
