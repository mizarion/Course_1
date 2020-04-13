using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Отвечает за UI
/// </summary>
public class CanvasManager : Singleton<CanvasManager>
{
    [Header("StartScene")]
    [SerializeField] GameObject StartSceneUI;   // 'Родитель' объектов главного меню
    //[SerializeField] Image _load;
    //[SerializeField] Button _start_Button;
    //[SerializeField] Button _quit_Button;   

    [Header("Settings")]
    [SerializeField] Image _settings_Image;

    [Header("HUD")]
    [SerializeField] GameObject HUD;     // 'Родитель' объектов HUD'а
    [SerializeField] Image _healthImage;
    [SerializeField] Image _manaImage;
    [SerializeField] Image _expImage;

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
    /// Переключает UI из главного меню в игровой режим.
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
        ActivateHUD(true);

        GameManager.instance.StartGame();

    }

    /// <summary>
    /// Ставит игру на паузу и открывает меню паузы/настроек
    /// </summary>
    public void PauseHandler()
    {
        GameManager.instance.TogglePause();

        _settings_Image.gameObject.SetActive(!_settings_Image.gameObject.activeSelf);
    }

    /// <summary>
    /// Сохраняет игру
    /// </summary>
    public void SaveHandler()
    {
        // todo: Сделать сохранение
        Debug.Log("[CanvasManger] SaveHandler");
    }

    /// <summary>
    /// Загружает сохранение
    /// </summary>
    public void LoadHandler()
    {
        // todo: Сделать загрузку сохранения
        Debug.Log("[CanvasManger] LoadHandler");
    }

    /// <summary>
    /// Обработчик перезапуска игры
    /// </summary>
    public void RestartHandler()
    {
        ActivateHUD(false);
        GameManager.instance.RestartGame();
    }

    /// <summary>
    /// Завершает работу игры
    /// </summary>
    public void QuitHandler()
    {
        GameManager.instance.QuitGame();
    }

    #endregion

    public void Debuger()
    {
        Debug.Log("[CanvasManager] Debuger");
    }
}
