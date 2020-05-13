using System;
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

    [Header("Settings")]
    [SerializeField] Image _settingsPanel;
    [SerializeField] Image _pauseImage;         // Ссылка на меню паузы
    [SerializeField] Slider _musicVolume;
    [SerializeField] TMPro.TextMeshProUGUI _FPSLimit_TMP;
    [SerializeField] Slider _FPSVolume;

    [Header("Save&Load")]
    [SerializeField] Image _loadMenu;
    [SerializeField] Image _saveMenu;
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
    [SerializeField] TMPro.TextMeshProUGUI _lvlText;
    [SerializeField] TMPro.TextMeshProUGUI _messageText;
    public Button Skill1, Skill2;
    [SerializeField] Image _storyImage;
    [SerializeField] Image _victoryPanel;

    [Header("Players Death")]
    [SerializeField] Image _DeathPanel;

    [HideInInspector] public bool needLoad;                       // отвечает за информирование о необходимости загрузки сохранения 
    [HideInInspector] public string path;

#pragma  warning restore 649

    /// <summary>
    /// Обновляет значения здоровья, маны, опыта, уровня
    /// </summary>
    public void UpdateHUD()
    {
        Player player = Player.instance;
        int lvl = player.Level;

        _healthImage.fillAmount = player.Health / DataManager.Stats.Player.Health[lvl];
        _manaImage.fillAmount = player.Manapool / DataManager.Stats.Player.Manapool[lvl];
        _expImage.fillAmount = player.Experience / DataManager.Stats.Player.Experience[lvl];
        _lvlText.text = $"Level: {player.Level}";
    }

    /// <summary>
    /// Отобразить сообщение в левой части экрана
    /// </summary>
    /// <param name="message"></param>
    public void UpdateMessage(string message)
    {
        _messageText.text = message;
    }

    /// <summary>
    /// Включает / выключает элементы интерфейса. 
    /// </summary>
    /// <param name="showHUD"> Показывать HUD</param>
    /// <param name="showStartMenu"> Показывать стартовое меню</param>
    /// <param name="showPauseMenu">Показать меню паузы</param>
    /// <param name="showDeathMenu">Показать меню смерти</param>
    public void ActivateUI(bool showHUD = false, bool showStartMenu = false, bool showPauseMenu = false, bool showDeathMenu = false)
    {
        HUD.SetActive(showHUD);
        _startSceneUI.SetActive(showStartMenu);
        _pauseImage.gameObject.SetActive(showPauseMenu);
        _DeathPanel.gameObject.SetActive(showDeathMenu);
        _loadMenu.gameObject.SetActive(false);
        _saveMenu.gameObject.SetActive(false);
        _settingsPanel.gameObject.SetActive(false);
        _victoryPanel.gameObject.SetActive(false);
    }

    #region GameState / Handlers

    /// <summary>
    /// Обработчик загрузки сцены
    /// </summary>
    public void StartHandler()
    {
        GameManager.Instance.StartGame();
    }

    /// <summary>
    /// Ставит игру на паузу или возобновляет и показывает/скрывает меню паузы.
    /// </summary>
    public void PauseHandler()
    {
        GameManager.Instance.TogglePause();
        ActivateUI(HUD.activeSelf, showPauseMenu: !_pauseImage.gameObject.activeSelf);
    }

    /// <summary>
    /// Начинает игру с загруженным сохранением
    /// </summary>
    /// <param name="button">Нажатая кнопка сохранения</param>
    public void MakeSave(SaveSlotButton button)
    {
        GameManager.Instance.SaveGame(button);
        ActivateSaveMenu();
    }

    /// <summary>
    /// Загружает сохранение по определенному пути.
    /// </summary>
    /// <param name="path">Путь сохранения</param>
    public void LoadSave(string path)
    {
        this.path = path;
        ActivateUI();
        if (GameManager.Instance.CurrentState == GameState.Pregame)
        {
            needLoad = true;
            StartHandler();
        }
        else
        {
            GameManager.Instance.CheckCurrentScene(path);
            ActivateUI(showHUD: true, showPauseMenu: true);
        }
    }

    /// <summary>
    /// Открывает / Закрывает меню загрузки
    /// </summary>
    public void ActivateLoadMenu()
    {
        _loadMenu.gameObject.SetActive(!_loadMenu.gameObject.activeSelf);
    }

    /// <summary>
    /// Открывает / Закрывает меню сохранений
    /// </summary>
    public void ActivateSaveMenu()
    {
        _saveMenu.gameObject.SetActive(!_saveMenu.gameObject.activeSelf);
    }

    /// <summary>
    /// Открывает / Закрывает меню настроек
    /// </summary>
    public void ActivateSettings()
    {
        _settingsPanel.gameObject.SetActive(!_settingsPanel.gameObject.activeSelf);
    }

    /// <summary>
    /// Обрабатывает действия в меню настроек
    /// </summary>
    /// <param name="action">Индекс изменение в меню настроек</param>
    public void SettingsHandler(int action = 0)
    {
        var audioSource = AudioManager.Instance.AudioSource;
        switch (action)
        {
            case 0: audioSource.volume = _musicVolume.value; break;
            case 1: AudioManager.Instance.PlayNextClip(); break;
            case 2: GameManager.Instance.LimitFPS((int)_FPSVolume.value); _FPSLimit_TMP.text = $"FPS Limit {(int)_FPSVolume.value}"; break;
            default:
                break;
        }
    }

    /// <summary>
    /// Показывает / Скрывает меню с предысторией
    /// </summary>
    public void ShowStory()
    {
        _storyImage.gameObject.SetActive(!_storyImage.gameObject.activeSelf);
    }

    /// <summary>
    /// Обработчик перезапуска игры / выхода в главное меню
    /// </summary>
    public void RestartHandler()
    {
        ActivateUI(showStartMenu: true);
        GameManager.Instance.RestartGame();
    }

    /// <summary>
    /// Обработчик меню смерти
    /// </summary>
    public void DeathHandler()
    {
        _DeathPanel.gameObject.SetActive(true);
        GameManager.Instance.UpdateGameState(GameState.Death);
    }

    /// <summary>
    /// Обработчик победы над главным злодеем
    /// </summary>
    public void Victory()
    {
        _victoryPanel.gameObject.SetActive(true);
        GameManager.Instance.TogglePause();
    }

    /// <summary>
    /// Обработчик завершения игры
    /// </summary>
    public void QuitHandler()
    {
        GameManager.Instance.QuitGame();
    }

    #endregion

    #region Spells

    /// <summary>
    /// Обработчик способности 1.
    /// Хорошо было бы сделать отображение нажатия...
    /// </summary>
    public void SpellOne()
    {
        SkillsManager.Instance.FirstSkill();
    }

    /// <summary>
    /// Обработчик способности 2
    /// Хорошо было бы сделать отображение нажатия...
    /// </summary>
    public void SpellTwo()
    {
        SkillsManager.Instance.SecondSkill();
    }

    #endregion


    /// <summary>
    /// Обновляет доступные сохранения
    /// </summary>
    public void UpdateSaves()
    {
        loadOne.UpdateButton();
        loadTwo.UpdateButton();
        loadThree.UpdateButton();
        saveOne.UpdateButton(true);
        saveTwo.UpdateButton(true);
        saveThree.UpdateButton(true);
    }

    /// <summary>
    /// Обновляет отображение доступных навыков
    /// </summary>
    public void UpdateSkills()
    {
        Skill1.gameObject.SetActive(GameManager.Instance.avaibleFirstSkill);
        Skill2.gameObject.SetActive(GameManager.Instance.avaibleSecondSkill);
    }
}
