using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Игровые состояния
/// </summary>
public enum GameState
{
    PREGAME,    // начальное состояние / главное меню
    RUNNING,    // в процессе игры
    PAUSED      // пауза
}

/// <summary>
/// Отвечает за главную логику игры
/// </summary>
public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// Текущее состояние игры
    /// </summary>
    public GameState CurrentState { get; private set; }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(CanvasManager.instance.gameObject);
        // DontDestroyOnLoad(InputManager.instance.gameObject);

        CurrentState = GameState.PREGAME;
    }

    /// <summary>
    /// Применяет и обрабатывает изменение состояния игры
    /// </summary>
    /// <param name="state">Состояние, в которое переходит</param>
    public void UpdateGameState(GameState state)
    {
        GameState privState = CurrentState;
        CurrentState = state;
        //Debug.Log($"cur state:{CurrentState}, privState: {privState}");
        switch (CurrentState)
        {
            case GameState.PREGAME:
                Time.timeScale = 1;
                break;
            case GameState.RUNNING:
                Time.timeScale = 1;
                break;
            case GameState.PAUSED:
                Time.timeScale = 0;
                break;
            default:
                Debug.LogError("[GameManager/UpdateGameState] default State");
                break;
        }

        //Debug.Log($"curState: {state} time: {Time.timeScale}");
    }


    /// <summary>
    /// Изменяет состояние игры
    /// </summary>
    public void TogglePause()
    {
        if (CurrentState == GameState.RUNNING)
        {
            UpdateGameState(GameState.PAUSED);
        }
        else if (CurrentState == GameState.PAUSED)
        {
            UpdateGameState(GameState.RUNNING);
        }
    }



    /// <summary>
    /// Начинает игру, загружая главную сцену
    /// </summary>
    public void StartGame()
    {
        //AsyncOperation loading = SceneManager.LoadSceneAsync(DataManager.Scenes.MainGame); //, LoadSceneMode.Additive);
        AsyncOperation loading = SceneManager.LoadSceneAsync("PoligonVikings", LoadSceneMode.Additive);

        loading.completed += Loading_completed;

        CurrentState = GameState.RUNNING;
    }

    /// <summary>
    /// Выполняет необходимые действия после загрузки сцены
    /// </summary>
    /// <param name="obj"></param>
    private void Loading_completed(AsyncOperation obj)
    {
        if (obj.isDone)
        {
            if ( CanvasManager.instance.needLoad)
            {
                CanvasManager.instance.LoadHandler();
            }
            CanvasManager.instance.ActivateHUD(true);
        }
    }


    /// <summary>
    /// Перезапускает игру
    /// </summary>
    public void RestartGame()
    {
        AsyncOperation unloading = SceneManager.UnloadSceneAsync("PoligonVikings");

        //AsyncOperation loading = SceneManager.LoadSceneAsync(DataManager.Scenes.startScene);

        UpdateGameState(GameState.PREGAME);
    }


    const string position = "position";
    const string rotation = "rotation";

    const string pathHealth = "PlayerHealth";
    const string pathMana = "PlayerMana";
    const string pathExp = "PlayerExp";
    const string pathLevel = "PlayerLevel";

    /// <summary>
    /// Сохраняет игру
    /// </summary>
    public void SaveGame()
    {
        // Todo: сохранить массив врагов

        Player inst = Player.instance;

        #region Save player stats 

        PlayerPrefs.SetFloat(pathHealth, inst.Health);
        PlayerPrefs.SetFloat(pathMana, inst.Manapool);
        PlayerPrefs.SetFloat(pathExp, inst.Experience);
        PlayerPrefs.SetInt(pathLevel, inst.Level);

        #endregion

        SaveTransform(Camera.main.transform);
        SaveTransform(inst.transform);
        Debug.Log("[GameManager] SaveGame");
    }

    /// <summary>
    /// Загружает игру
    /// </summary>
    public void LoadGame()
    {
        Player inst = Player.instance;

        #region Load player stat

        inst.Health = PlayerPrefs.GetFloat(pathHealth);
        inst.Manapool = PlayerPrefs.GetFloat(pathMana);
        inst.Experience = PlayerPrefs.GetFloat(pathExp);
        inst.Level = PlayerPrefs.GetInt(pathLevel);

        #endregion

        LoadTransform(inst.transform);
        //LoadTransform(Camera.main.transform);
        Debug.Log("[GameManager] LoadGame");
    }



    /// <summary>
    /// Сохраняет данные персонажа в transform'е
    /// </summary>
    /// <param name="tr">Ссылка на трансформ</param>
    void SaveTransform(Transform tr)
    {
        // Сохраняем позицию
        string data = JsonUtility.ToJson(tr.position);
        PlayerPrefs.SetString(position, data);
        // сохраняем поворот
        data = JsonUtility.ToJson(tr.rotation);
        PlayerPrefs.SetString(rotation, data);
        // Выгружаем буфер
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Загружает данные персонажа в transform'е
    /// </summary>
    /// <param name="tr">Ссылка на трансформ</param>
    void LoadTransform(Transform tr)
    {
        string data = PlayerPrefs.GetString(position);
        tr.position = JsonUtility.FromJson<Vector3>(data);

        data = PlayerPrefs.GetString(rotation);
        tr.rotation = JsonUtility.FromJson<Quaternion>(data);
    }



    /// <summary>
    /// Завершает работу игры
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
