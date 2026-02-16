using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopMenuController : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform containerButton;
    
    [Space(5)]
    [Header("Panel")]
    [SerializeField] private GameObject panelTopUp;
    [SerializeField] private GameObject panelBuy;

    [Space(5)]
    [Header("Panel Buy Properties")]
    [SerializeField] private Button btnBuy;

    [Space(5)]
    [SerializeField] private Text txtCurrentMoney;
    [SerializeField] private Button btnBack;

    private ImagePuzzleData itemInWaiting;

    private List<ButtonShop> buttons = new List<ButtonShop>();

    #region Init Button
    private void CreateButton()
    {
        for (int i = 0; i < PuzzleDataMiner.GetPuzzleCount(); i++)
        {
            int index = i;

            ImagePuzzleData data = PuzzleDataMiner.GetPuzzle(index);

            ButtonShop button = Instantiate(buttonPrefab, containerButton).GetComponent<ButtonShop>();
            button.SetSprite(data.Icon);
            button.SetText(""+data.Price);
            button.SetDataPuzzle(data);
            button.AddListener(
                () =>
                {
                    //Lock
                    OpenTopUp(data);

                },
                () =>
                {
                    //Unlock
                    OpenItem(index);
                });
            
            buttons.Add(button);
        }
    }

    public void OpenItem(int _index)
    {
        PuzzleDataMiner.SetPic(_index);
        //SceneManager.LoadScene((int)GameEnums.SceneOrder.GAME);
    }

    public void OpenTopUp(ImagePuzzleData _data)
    {
        //if(_data.Price <= CoinManager.Coins)
        //{
        //    itemInWaiting = _data;
        //    panelBuy.SetActive(true);
        //}
        //else panelTopUp.SetActive(true);
    }

    public void BuyTheDeal()
    {
        itemInWaiting.IsUnlock = true;
        //CoinManager.RemoveCoins(itemInWaiting.Price);
        panelBuy.SetActive(false);
    }

    private void BackBtn()
    {
        //SceneManager.LoadScene((int)GameEnums.SceneOrder.CHOOSE_GIRL);
    }

    private void UpdatePrice()
    {
        //txtCurrentMoney.text = CoinManager.Coins + "";
    }

    #endregion

    #region Unity
    private void Start()
    {
        btnBuy.onClick.AddListener(() => { BuyTheDeal(); });
        btnBack.onClick.AddListener(() => { BackBtn(); });
        CreateButton();
        UpdatePrice();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackBtn();
        }

        UpdatePrice();
    }
    #endregion

    /*

    public void UnlockPicture()
    {
        int cost = CutsceneList.Instance.listOfPictures[selectedPicture].price;

        if (CoinManager.Instance.Coins >= cost)
        {
            buttons[selectedPicture].image.color = Color.white;
            CutsceneList.Instance.listOfPictures[selectedPicture].unlock = true;
            CutsceneList.Instance.SaveData(selectedPicture);
            CoinManager.Instance.RemoveCoins(cost);
            UpdatePrice(true);
            SoundManager.Instance.PlaySound(SoundManager.Instance.unlock);
        }
        else return;
    }

    public void BackBtn()
    {
        if (previewPicture.IsActive())
        {
            previewPicture.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.button);
            SceneManager.LoadScene("Gameplay");
        }
    }

    #region VisualNovel
    void OpenCutscene(string name)
    {
        CutsceneList.Instance.SetCutscene(name);
        SceneManager.LoadScene("Cutscene");
    }
    #endregion

    private void Start()
    {
        btnUnlock.onClick.AddListener(() => { UnlockPicture(); });
        btnBack.onClick.AddListener(() => { BackBtn(); });
        CreateButtonPic();
        selectedPicture = 0;
        UpdatePrice();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackBtn();
        }
        txtCurrentMoney.text = CoinManager.Instance.Coins + "";
    }*/
}
