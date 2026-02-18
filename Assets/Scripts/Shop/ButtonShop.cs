using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonShop : MonoBehaviour
{
        /*
    [SerializeField] private Text textTag;
    [SerializeField] private GameObject panelTextTag;
    [SerializeField] private Button button;
    [SerializeField] private GameObject lockIcon;

    [SerializeField] private Color softLockColor = new Color(1, 1, 1, 0.5f);

    private bool softLock;
    private ImagePuzzleData dataPuzzle;

    public void SetDataPuzzle(ImagePuzzleData _dataPuzzle) => dataPuzzle = _dataPuzzle;

    public void SetText(string _text) => textTag.text = _text;

    public void AddListener(UnityAction _whenLock, UnityAction _whenUnlock)
    {
        button.onClick.AddListener(() => {
            if (softLock) _whenLock.Invoke();
            else _whenUnlock.Invoke();
        });
    }

    public void SetSprite(Sprite _sprite) => button.image.sprite = _sprite;

    private void ChangeLock(bool _value)
    {
        softLock = _value;
        lockIcon.SetActive(_value);
        panelTextTag.SetActive(_value);

        if (!softLock) button.image.color = Color.white;
        else button.image.color = softLockColor;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        try { ChangeLock(!dataPuzzle.IsUnlock); } catch { }
    }    */
}
