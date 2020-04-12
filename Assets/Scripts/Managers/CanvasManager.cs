using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : Singleton<CanvasManager>
{
    [Header("StartScene")]
    [SerializeField] GameObject StartSceneUI;
    [SerializeField] Image _load;
    [SerializeField] Button _start_Button;
    [SerializeField] Button _quit_Button;

    [Header("Settings")]
    [SerializeField] Image _settings_Image;

    [Header("HUD")]
    [SerializeField] GameObject HUD;
    [SerializeField] Image _healthImage;
    [SerializeField] Image _manaImage;
    [SerializeField] Image _expImage;

    //[Header("Player")]
    //[SerializeField] Player _player;


    /// <summary>
    /// Открывает меню настроек
    /// </summary>
    public void OpenSettings()
    {
        _settings_Image.gameObject.SetActive(!_settings_Image.gameObject.activeSelf);
    }

    /// <summary>
    /// Обновляет HUD
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
    /// Переключает UI из меню в игровой режим.
    /// </summary>
    /// <param name="isGameStart">Это начало игры?</param>
    public void ActivateHUD(bool isGameStart)
    {
        HUD.SetActive(isGameStart);

        StartSceneUI.SetActive(!isGameStart);
    }


    #region GameState / Handlers

    /// <summary>
    /// Начинает загрузку сцены
    /// </summary>
    public void StartHandler()
    {
        //_start_Button.gameObject.SetActive(false);
        //_load.gameObject.SetActive(true);

        ActivateHUD(true);

        GameManager.instance.StartGame();

        // ToDo: update image: "_load"
    }


    /// <summary>
    /// Обработчик возобновления игры
    /// </summary>
    public void PauseHandler()
    {
        GameManager.instance.TogglePause();

        _settings_Image.gameObject.SetActive(!_settings_Image.gameObject.activeSelf);
    }

    /// <summary>
    /// Обработчик перезапуска игры
    /// </summary>
    public void RestartHandler()
    {
        ActivateHUD(false);
        GameManager.instance.RestartGame();
        // Todo: активировать интерфейс
    }

    /// <summary>
    /// Завершает работу игры
    /// </summary>
    public void QuitHandler()
    {
        GameManager.instance.QuitGame();
    }

    #endregion

}
