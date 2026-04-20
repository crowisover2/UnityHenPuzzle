using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace PuzzleHen
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private Button btnPlay;
        [SerializeField] private Button btnOption;
        [SerializeField] private Button btnGallery;
        [SerializeField] private Button btnCredit;

        private void LoadScene(string _id)
        {
            SceneManager.LoadScene(_id);
        }

        // Start is called before the first frame update
        void Start()
        {
            btnPlay.onClick.AddListener(() => { LoadScene(Constants.SceneChooseGirl); });
            btnOption.onClick.AddListener(() => { });
            btnCredit.onClick.AddListener(() => { });
            btnGallery.onClick.AddListener(() => { LoadScene(Constants.SceneGallery); });
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
