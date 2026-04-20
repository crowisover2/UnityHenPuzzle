using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace PuzzleHen.ChooseGirl
{
    public class ButtonGirlProperties : MonoBehaviour
    {
        [SerializeField] private Text textTag;
        [SerializeField] private Button button;
        [SerializeField] private Image image;

        public void SetName(string _text) => textTag.text = _text;

        public void AddListener(UnityAction _action)
        {
            button.onClick.AddListener(_action);
        }

        public void SetSprite(Sprite _sprite) => image.sprite = _sprite;

    }
}