using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
    //public DataManager.Scenes CurrentScene { get; private set; }


    void Start()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(CanvasManager.Instance.gameObject);
        // DontDestroyOnLoad(InputManager.instance.gameObject);

        CurrentState = GameState.PREGAME;
        //CurrentScene = DataManager.Scenes.StartScene;
    }

    /// <summary>
    /// Изменяет и обрабатывает изменение состояния игры
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


    //public void UpdateGameScene(DataManager.Scenes newScene)
    //{
    //    // Todo: Добавить проверку на предыдущую сцену
    //    // Этот метод должен помочь настроить многоуровневость

    //    var privScene = CurrentScene;
    //    CurrentScene = newScene;

    //    switch (CurrentScene)
    //    {
    //        case DataManager.Scenes.StartScene:

    //            break;
    //        case DataManager.Scenes.MainGame:

    //            break;
    //        default:
    //            break;
    //    }
    //}


    /// <summary>
    /// Возобновляет/приостанавливает игру
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
        AsyncOperation loading = SceneManager.LoadSceneAsync(/*"PoligonVikings"*/ (int)DataManager.Scenes.MainGame, LoadSceneMode.Additive);

        loading.completed += Loading_completed;

        UpdateGameState(GameState.RUNNING);
        //UpdateGameScene(DataManager.Scenes.MainGame);

    }

    /// <summary>
    /// Выполняет необходимые действия после загрузки сцены
    /// </summary>
    /// <param name="op">Асинхронная операция</param>
    private void Loading_completed(AsyncOperation op)
    {
        if (op.isDone)
        {
            if (CanvasManager.Instance.needLoad)
            {
                CanvasManager.Instance.LoadHandler();
                CanvasManager.Instance.needLoad = false;
            }
            CanvasManager.Instance.ActivateHUD(true);
        }
    }


    /// <summary>
    /// Перезапускает игру.
    /// Выгружает текущую сцену, меняя режим интерфейса на стартовое меню.  
    /// </summary>
    public void RestartGame()
    {
        AsyncOperation unloading = SceneManager.UnloadSceneAsync(/*"PoligonVikings"*/ (int)DataManager.Scenes.MainGame);

        //AsyncOperation loading = SceneManager.LoadSceneAsync(DataManager.Scenes.startScene);

        UpdateGameState(GameState.PREGAME);
        //UpdateGameScene(DataManager.Scenes.MainGame);
    }

    //   todo: Задокументировать 

    #region Save & Load     


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
        // Todo: сохранить массив врагов и настроить сохранение координат персонажа
        TogglePause();

        Player inst = Player.instance;

        #region Save player stats 

        PlayerPrefs.SetFloat(pathHealth, inst.Health);
        PlayerPrefs.SetFloat(pathMana, inst.Manapool);
        PlayerPrefs.SetFloat(pathExp, inst.Experience);
        PlayerPrefs.SetInt(pathLevel, inst.Level);

        #endregion

        SaveTransform(inst.transform, "Player");
        SaveTransform(Camera.main.transform, "Camera");
        Debug.Log("[GameManager] SaveGame");
        TogglePause();
    }

    /// <summary>
    /// Загружает игру
    /// </summary>
    public void LoadGame()
    {
        TogglePause();
        Player inst = Player.instance;

        #region Load player stat

        inst.Health = PlayerPrefs.GetFloat(pathHealth);
        inst.Manapool = PlayerPrefs.GetFloat(pathMana);
        inst.Experience = PlayerPrefs.GetFloat(pathExp);
        inst.Level = PlayerPrefs.GetInt(pathLevel);
        CanvasManager.Instance.UpdateHUD();
        #endregion

        LoadTransform(inst.transform, "Player");
        LoadTransform(Camera.main.transform, "Camera");
        Debug.Log("[GameManager] LoadGame");

        // Костыль для NavMesh'а
        inst.agent.gameObject.SetActive(false);
        inst.agent.gameObject.SetActive(true);
        inst.agent.ResetPath();

        TogglePause();
    }

    /// <summary>
    /// Сохраняет данные персонажа в transform'е
    /// </summary>
    /// <param name="tr">Ссылка на трансформ</param>
    /// <param name="pathName">Дополнительный путь</param>
    void SaveTransform(Transform tr, string pathName)
    {
        // Сохраняем позицию
        string data = JsonUtility.ToJson(tr.position);
        PlayerPrefs.SetString(position + pathName, data);

        //PlayerPrefs.SetFloat(position + pathName + "x", tr.position.x);
        //PlayerPrefs.SetFloat(position + pathName + "y", tr.position.y);
        //PlayerPrefs.SetFloat(position + pathName + "z", tr.position.z);

        // сохраняем поворот
        data = JsonUtility.ToJson(tr.rotation);
        PlayerPrefs.SetString(rotation + pathName, data);
        // Выгружаем буфер
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Загружает данные персонажа в transform'е
    /// </summary>
    /// <param name="tr">Ссылка на трансформ</param>
    /// <param name="pathName">Дополнительный путь</param>
    void LoadTransform(Transform tr, string pathName)
    {
        string data = PlayerPrefs.GetString(position + pathName);
        tr.position = JsonUtility.FromJson<Vector3>(data);


        //float pX = PlayerPrefs.GetFloat(position + pathName + "x");
        //float pY = PlayerPrefs.GetFloat(position + pathName + "y");
        //float pZ = PlayerPrefs.GetFloat(position + pathName + "z");
        //tr.position = new Vector3(pX, pY, pZ);

        data = PlayerPrefs.GetString(rotation + pathName);
        tr.rotation = JsonUtility.FromJson<Quaternion>(data);
    }

    #endregion

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
