using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс, реализующий работу UI
/// </summary>
public class CanvasManager : Singleton<CanvasManager>
{
    [Header("StartScene")]
    [SerializeField] GameObject _startSceneUI;   // Контейнер для объектов стартового меню
    //[SerializeField] Image _load;
    //[SerializeField] Button _start_Button;
    //[SerializeField] Button _quit_Button;   

    [Header("Settings")]
    [SerializeField] Image _pauseImage;         // Ссылка на меню паузы

    [Header("HUD")]
    [SerializeField] GameObject HUD;            // Контейнер для объектов пользовательского интерфейса
    [SerializeField] Image _healthImage;        // Полоска здоровья героя
    [SerializeField] Image _manaImage;          // Полоска маны героя
    [SerializeField] Image _expImage;           // Полоска опыта героя
    //[SerializeField] Text _levelText;

    public bool needLoad;                       // отвечает за информирование о необходимости загрузки сохранения 

    /// <summary>
    /// Обновляет значения _healthImage, _manaImage, _expImage
    /// </summary>
    public void UpdateHUD()
    {
        Player player = Player.instance;
        int lvl = player.Level;
        _healthImage.fillAmount = player.Health / DataManager.Stats.Player.Health[lvl];
        _manaImage.fillAmount = player.Manapool / DataManager.Stats.Player.Manapool[lvl];
        _expImage.fillAmount = player.Experience / DataManager.Stats.Player.Experience[lvl];
    }

    /// <summary>
    /// Переключает режим интерфейса. 
    /// Из стартового в игровой режим и обратно
    /// </summary>
    /// <param name="isGameStart">Это начало игры?</param>
    public void ActivateHUD(bool isGameStart)
    {
        HUD.SetActive(isGameStart);
        if (!isGameStart && _pauseImage.gameObject.activeSelf)
        {
            _pauseImage.gameObject.SetActive(false);
        }
        _startSceneUI.SetActive(!isGameStart);
    }


    #region GameState / Handlers

    /// <summary>
    /// Обработчик загрузки сцены
    /// </summary>
    public void StartHandler()
    {
        //ActivateHUD(true);
        _startSceneUI.SetActive(false);
        GameManager.Instance.StartGame();
    }

    /// <summary>
    /// Ставит игру на паузу или возобновляет.
    /// Открывает меню паузы.
    /// Открывает меню паузы.
    /// </summary>
    public void PauseHandler()
    {
        GameManager.Instance.TogglePause();

        _pauseImage.gameObject.SetActive(!_pauseImage.gameObject.activeSelf);
    }

    /// <summary>
    /// Обработчик кнопки сохранения игры.
    /// </summary>
    public void SaveHandler()
    {
        GameManager.Instance.SaveGame();

        Debug.Log("[CanvasManger] SaveHandler");
    }

    /// <summary>
    /// Обработчик загрузки игры
    /// </summary>
    public void LoadHandler()
    {
        GameManager.Instance.LoadGame();

        Debug.Log("[CanvasManger] LoadHandler");
    }

    /// <summary>
    /// Начинает игру с загруженным сохранением
    /// </summary>
    public void StartGameAndLoadSave()
    {
        StartHandler();
        needLoad = true;
    }


    /// <summary>
    /// Обработчик перезапуска игры
    /// </summary>
    public void RestartHandler()
    {
        ActivateHUD(false);
        GameManager.Instance.RestartGame();
    }


    public void OptionsHandler()
    {
        RestartHandler();
    }

    /// <summary>
    /// Обработчик завершения игры
    /// </summary>
    public void QuitHandler()
    {
        GameManager.Instance.QuitGame();
    }

    #endregion

    public void Debuger()
    {
        Debug.Log("[CanvasManager] Debuger");
    }
}
