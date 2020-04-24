using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
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

        // Todo: Debugonly:
        if (Directory.Exists("Saves"))
        {
            Debug.Log("exist Saves");
            if (Directory.Exists(@"Saves\Save1"))
            {
                Debug.Log(@"exist Saves\Save1 ");
                // Todo: активировать возможность загрузки 
            }
        }
        else
        {
            Directory.CreateDirectory("Saves");
        }
    }

    /// <summary>
    /// Изменяет и обрабатывает изменение состояния игры
    /// </summary>
    /// <param name="state">Состояние, в которое переходит</param>
    public void UpdateGameState(GameState state)
    {
        //GameState privState = CurrentState;
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


    /// <summary>
    /// Сохраняет игру
    /// </summary>
    public void SaveGame(string path)
    {
        UpdateGameState(GameState.PAUSED);

        Directory.CreateDirectory(path);
        string PlayerTransform = path + @"\PT.json";
        string PlayerPlayer = path + @"\PP.json";
        string EnemyTransform = path + @"\ET.json";

        Player player = Player.instance;

        // Сохраняем Player героя
        using (StreamWriter sw = new StreamWriter(PlayerPlayer))
        {
            string data = JsonUtility.ToJson(player, true);
            sw.Write(data);
        }
        // Сохраняем Player героя
        using (StreamWriter sw = new StreamWriter(PlayerTransform))
        {
            string data = JsonUtility.ToJson(player.transform.position, true);
            sw.Write(data);
        }
        // сохраняем Transform врагов
        using (FileStream fs = new FileStream(EnemyTransform, FileMode.Create))
        {
            List<string> enemies = new List<string>();
            foreach (var item in EnemyManager.Instance.GetPool)
            {
                if (item.activeSelf)
                {
                    enemies.Add(JsonUtility.ToJson(item.transform.position));
                }
            }
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<string>));
            serializer.WriteObject(fs, enemies);
        }

        Debug.Log("[GameManager] SaveGame");
        //UpdateGameState(GameState.RUNNING);
    }

    /// <summary>
    /// Загружает игру
    /// </summary>
    public void LoadGame(string path)
    {
        UpdateGameState(GameState.PAUSED);

        string PlayerTransform = path + @"\PT.json";
        string PlayerPlayer = path + @"\PP.json";
        string EnemyTransform = path + @"\ET.json";

        if (!File.Exists(PlayerTransform) || !File.Exists(PlayerPlayer) || !File.Exists(EnemyTransform))
        {
            return;
        }

        Player player = Player.instance;

        // Загружаем Player героя
        using (StreamReader sr = new StreamReader(PlayerPlayer))
        {
            string data = sr.ReadToEnd();
            JsonUtility.FromJsonOverwrite(data, player);
            player.GetComponent<NavMeshAgent>().ResetPath();
        }
        // Загружаем Transform героя
        using (StreamReader sr = new StreamReader(PlayerTransform))
        {
            string data = sr.ReadToEnd();
            player.transform.position = JsonUtility.FromJson<Vector3>(data);
        }
        // Загружаем Transform врагов
        using (FileStream fs = new FileStream(EnemyTransform, FileMode.Open))
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<string>));
            List<string> enemies = serializer.ReadObject(fs) as List<string>;
            for (int i = 0; i < EnemyManager.Instance.GetPool.Count; i++)
            {
                for (; i < enemies.Count; i++)
                {
                    EnemyManager.Instance.GetPool[i].transform.position = JsonUtility.FromJson<Vector3>(enemies[i]);
                    EnemyManager.Instance.GetPool[i].SetActive(true);
                    EnemyManager.Instance.GetPool[i].GetComponent<NavMeshAgent>().ResetPath();
                }
                EnemyManager.Instance.GetPool[i].SetActive(false);
            }
        }
        CanvasManager.Instance.UpdateHUD();
        Debug.Log("[GameManager] LoadGame");
        //UpdateGameState(GameState.RUNNING);
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
