using UnityEngine;

/// <summary>
/// Класс, реализующий работу кнопки сохранений
/// </summary>
public class SaveSlotButton : MonoBehaviour
{
#pragma warning disable 649

    public string path;       // Путь до папки с сохранением

    [SerializeField] TMPro.TextMeshProUGUI _dateTMP; // Ссылка компонент с текстом

    [SerializeField] string _text;  // Хранит значение для сериализации

    public bool isAvaible;          // Доступность сохранения

#pragma warning restore 649

    /// <summary>
    /// Свойство для работы с отображаемым текстом на кнопке сохранения 
    /// </summary>
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
    /// <param name="forceActivate">Принудительная активация</param>
    public void UpdateButton(bool forceActivate = false)
    {
        gameObject.SetActive(isAvaible || forceActivate);
        _dateTMP.text = _text;
    }
}
