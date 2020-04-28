using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс, реализующий работу UI
/// </summary>
public class CanvasManager : Singleton<CanvasManager>
{
#pragma warning disable 649
    [Header("StartScene")]
    [SerializeField] GameObject _startSceneUI;   // Контейнер для объектов стартового меню
    //[SerializeField] Image _load;
    //[SerializeField] Button _start_Button;
    //[SerializeField] Button _quit_Button;   

    [Header("Settings")]
    [SerializeField] Image _pauseImage;         // Ссылка на меню паузы

    [Header("Save&Load")]
    [SerializeField] Image _loadMenu;
    [SerializeField] Image _saveMenu;
    //[SerializeField] Button _SaveOne;
    //[SerializeField] Button _SaveTwo;
    //[SerializeField] Button _SaveThree;
    public SaveSlotButton loadOne;
    public SaveSlotButton loadTwo;
    public SaveSlotButton loadThree;
    public SaveSlotButton saveOne;
    public SaveSlotButton saveTwo;
    public SaveSlotButton saveThree;

    [Header("HUD")]
    [SerializeField] GameObject HUD;            // Контейнер для объектов пользовательского интерфейса
    [SerializeField] Image _healthImage;        // Полоска здоровья героя
    [SerializeField] Image _manaImage;          // Полоска маны героя
    [SerializeField] Image _expImage;           // Полоска опыта героя'
    [SerializeField] Text _lvlText;
    [SerializeField] Text _messageText;

    [Header("Players Death")]
    [SerializeField] Image _DeathPanel;

    [HideInInspector] public bool needLoad;                       // отвечает за информирование о необходимости загрузки сохранения 
    [HideInInspector] public string path;

#pragma  warning restore 649

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
        _lvlText.text = "Level: " + player.Level.ToString();
    }

    public void UpdateMessage(string message)
    {
        _messageText.text = message;
    }

    /// <summary>
    /// Включает / выключает элементы интерфейса. 
    /// </summary>
    /// <param name="isGameStart">Это начало игры?</param>
    /// <param name="showPauseMenu">Показать меню паузы</param>
    /// <param name="showDeathMenu">Показать меню смерти</param>
    public void ActivateHUD(bool isGameStart, bool showPauseMenu = false, bool showDeathMenu = false)
    {
        HUD.SetActive(isGameStart);
        _startSceneUI.SetActive(!isGameStart);
        _pauseImage.gameObject.SetActive(showPauseMenu);
        _DeathPanel.gameObject.SetActive(showDeathMenu);
        _loadMenu.gameObject.SetActive(false);
        _saveMenu.gameObject.SetActive(false);
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
        //_loadMenu.gameObject.SetActive(false);
        //_saveMenu.gameObject.SetActive(false);
        //_pauseImage.gameObject.SetActive(!_pauseImage.gameObject.activeSelf);
        ActivateHUD(true, !_pauseImage.gameObject.activeSelf);
    }

    ///// <summary>
    ///// Обработчик кнопки сохранения игры.
    ///// </summary>
    //public void SaveHandler()
    //{
    //    GameManager.Instance.SaveGame("Saves/Save1");

    //    Debug.Log("[CanvasManger] SaveHandler");
    //}



    /// <summary>
    /// Обработчик загрузки игры
    /// </summary>
    public void LoadHandler()
    {
        //GameManager.Instance.LoadGame(path);

        //_DeathPanel.gameObject.SetActive(false);
        //_pauseImage.gameObject.SetActive(false);
        //GameManager.Instance.UpdateGameState(GameState.RUNNING);

        //Debug.Log("[CanvasManger] LoadHandler");
    }

    //public void StartGameAndLoadSave()
    //{
    //    StartHandler();
    //    _loadMenu.gameObject.SetActive(false);
    //    needLoad = true;
    //}

    /// <summary>
    /// Начинает игру с загруженным сохранением
    /// </summary>
    public void MakeSave(SaveSlotButton button)
    {
        //GameManager.Instance.SaveGame(path);
        GameManager.Instance.SaveGame(button);
        ActivateSaveMenu();
    }

    public void LoadSave(string path)
    {
        this.path = path;
        ActivateLoadMenu();
        if (GameManager.Instance.CurrentState == GameState.Pregame)
        {
            needLoad = true;
            Debug.Log("[CanvasManager] LoadSave");
            StartHandler();
        }
        else
        {
            ActivateHUD(true, true);
            GameManager.Instance.LoadGame(path);
        }
        //Debug.Log($"here path: {path}");
    }

    ///// <summary>
    ///// Закрывает меню загрузки
    ///// </summary>
    //public void CloseLoadMenu()
    //{
    //    _loadMenu.gameObject.SetActive(false);
    //}

    /// <summary>
    /// Закрывает меню загрузки
    /// </summary>
    public void ActivateLoadMenu()
    {
        _loadMenu.gameObject.SetActive(!_loadMenu.gameObject.activeSelf);
    }

    /// <summary>
    /// Закрывает меню сохранений
    /// </summary>
    public void ActivateSaveMenu()
    {
        _saveMenu.gameObject.SetActive(!_saveMenu.gameObject.activeSelf);
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
    /// Обработчик меню смерти
    /// </summary>
    public void DeathHandler()
    {
        _DeathPanel.gameObject.SetActive(true);
        GameManager.Instance.UpdateGameState(GameState.Death);
        //Debug.Log(GameManager.Instance.CurrentState);
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

    /// <summary>
    /// Обновляет доступные сохранения
    /// </summary>
    public void UpdateSaves()
    {
        loadOne.gameObject.SetActive(GameManager.Instance.isSaveOneAvailable);
        loadTwo.gameObject.SetActive(GameManager.Instance.isSaveTwoAvailable);
        loadThree.gameObject.SetActive(GameManager.Instance.isSaveThreeAvailable);

        saveOne.UpdateText();
        saveTwo.UpdateText();
        saveThree.UpdateText();
    }
}
