using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotButton : MonoBehaviour
{
#pragma warning disable 649

    public string path;

    [SerializeField] TMPro.TextMeshProUGUI _dateTMP;

    [SerializeField] string _text;

    public bool isAvaible;

#pragma warning restore 649

    public string DateTMP
    {
        get => _text;
        set
        {
            _text = value;
            _dateTMP.text = value;
        }
    }

    /// <summary>
    /// Обновляет содержимое кнопки и ее отображение
    /// </summary>
    /// <param name="forceActivate"></param>
    public void UpdateButton(bool forceActivate = false)
    {
        gameObject.SetActive(isAvaible || forceActivate);
        _dateTMP.text = _text;
    }
}
