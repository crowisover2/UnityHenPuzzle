using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnTopUp;

    //private void LoadScene(GameEnums.SceneOrder _order)
    //{
    //    SceneManager.LoadScene((int) _order);
    //}

    // Start is called before the first frame update
    void Start()
    {
        //btnPlay.onClick.AddListener(() => { LoadScene(GameEnums.SceneOrder.CHOOSE_GIRL); });
        //btnTopUp.onClick.AddListener(() => { LoadScene(GameEnums.SceneOrder.TOP_UP); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
