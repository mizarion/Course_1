using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


/// <summary>
/// Игровые состояния
/// </summary>
public enum GameState
{
    Pregame,    // начальное состояние / главное меню
    Running,    // в процессе игры
    Pause,      // пауза
    Death
}

/// <summary>
/// Отвечает за главную логику игры
/// </summary>
public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// Текущее состояние игры
    /// </summary>
    public GameState CurrentState /*{ get; private set; }*/ ;

    //public DataManager.Scenes CurrentScene { get; private set; }

    [HideInInspector] public bool isSaveOneAvailable;
    [HideInInspector] public bool isSaveTwoAvailable;
    [HideInInspector] public bool isSaveThreeAvailable;

    [SerializeField] int _deathCounter;

    public bool canTeleport;

    public int DeathCounter
    {
        get => _deathCounter; set
        {
            _deathCounter = value;
            if (_deathCounter == 10)
            {
                EnemyManager.Instance.StartNewWawe(5);
            }
            if (_deathCounter == 15)
            {
                canTeleport = true;
            }
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(CanvasManager.Instance.gameObject);
        // DontDestroyOnLoad(InputManager.instance.gameObject);

        CurrentState = GameState.Pregame;
        //CurrentScene = DataManager.Scenes.StartScene;

        //// Todo: Debugonly:
        //if (Directory.Exists("Saves"))
        //{
        //    Debug.Log("exist Saves");
        //    if (Directory.Exists(@"Saves\Save1"))
        //    {
        //        Debug.Log(@"exist Saves\Save1 ");
        //        // Todo: активировать возможность загрузки 
        //    }
        //}
        //else
        //{
        //    Directory.CreateDirectory("Saves");
        //}

        CheckSaves();
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
            case GameState.Pregame:
                Time.timeScale = 0;
                break;
            case GameState.Running:
                Time.timeScale = 1;
                break;
            case GameState.Pause:
                Time.timeScale = 0;
                break;
            case GameState.Death:
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
        if (CurrentState == GameState.Running)
        {
            UpdateGameState(GameState.Pause);
        }
        else if (CurrentState == GameState.Pause)
        {
            UpdateGameState(GameState.Running);
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

        UpdateGameState(GameState.Running);
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
                //CanvasManager.Instance.LoadHandler();
                LoadGame(CanvasManager.Instance.path);
                UpdateGameState(GameState.Running);
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

        UpdateGameState(GameState.Pregame);
        //UpdateGameScene(DataManager.Scenes.MainGame);
    }

    //   todo: Задокументировать 

    #region Save & Load     


    public void CheckSaves()
    {
        // Todo: проверить другие файлы
        if (File.Exists(@"Saves/Save1/PlayerTransform.json") && File.Exists(@"Saves/Save1/PlayerData.json") && File.Exists(@"Saves/Save1/EnemyTransform.json"))
        {
            using (StreamReader sr = new StreamReader(@"Saves/Save1/SaveButton.json"))
            {
                JsonUtility.FromJsonOverwrite(sr.ReadToEnd(), CanvasManager.Instance.saveOne);
                CanvasManager.Instance.loadOne.GetComponent<SaveSlotButton>().Name_Text.text = CanvasManager.Instance.saveOne.DateTime;
            }
            isSaveOneAvailable = true;
        }
        if (File.Exists(@"Saves/Save2/PlayerTransform.json") && File.Exists(@"Saves/Save2/PlayerData.json") && File.Exists(@"Saves/Save2/EnemyTransform.json"))
        {
            using (StreamReader sr = new StreamReader(@"Saves/Save2/SaveButton.json"))
            {
                JsonUtility.FromJsonOverwrite(sr.ReadToEnd(), CanvasManager.Instance.saveTwo);
                CanvasManager.Instance.loadTwo.GetComponent<SaveSlotButton>().Name_Text.text = CanvasManager.Instance.saveTwo.DateTime;
            }
            isSaveTwoAvailable = true;
        }
        if (File.Exists(@"Saves/Save3/PlayerTransform.json") && File.Exists(@"Saves/Save3/PlayerData.json") && File.Exists(@"Saves/Save3/EnemyTransform.json"))
        {
            using (StreamReader sr = new StreamReader(@"Saves/Save3/SaveButton.json"))
            {
                JsonUtility.FromJsonOverwrite(sr.ReadToEnd(), CanvasManager.Instance.saveThree);
                CanvasManager.Instance.loadThree.GetComponent<SaveSlotButton>().Name_Text.text = CanvasManager.Instance.saveThree.DateTime;
            }
            isSaveThreeAvailable = true;
        }
        CanvasManager.Instance.UpdateSaves();
    }

    /// <summary>
    /// Сохраняет игру
    /// </summary>
    public void SaveGame(SaveSlotButton button)
    {
        UpdateGameState(GameState.Pause);
        button.DateTime = System.DateTime.Now.ToString();
        string path = button.path;
        Directory.CreateDirectory(path);
        string PlayerTransform = path + @"\PlayerTransform.json";
        string PlayerPlayer = path + @"\PlayerData.json";
        string EnemyTransform = path + @"\EnemyTransform.json";
        //string EnemyGoblin = path + @"\EnemyGoblin.json";
        string ButtonPath = path + @"\SaveButton.json";
        string GameManagerPath = path + @"\GameManager.json";

        // Сохраняем GameManager
        using (StreamWriter sw = new StreamWriter(GameManagerPath))
        {
            string data = JsonUtility.ToJson(this, true);
            sw.Write(data);
        }
        // Сохраняем информацию кнопки
        using (StreamWriter sw = new StreamWriter(ButtonPath))
        {
            string data = JsonUtility.ToJson(button, true);
            sw.Write(data);
        }

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
        // сохраняем Transform и Goblin врагов
        using (FileStream fs = new FileStream(EnemyTransform, FileMode.Create))
        {
            //using (FileStream fsgob = new FileStream(EnemyGoblin, FileMode.Create))
            //{
            List<string> enemies = new List<string>();
            //List<Goblin> goblins = new List<Goblin>();
            foreach (var item in EnemyManager.Instance.GetPool)
            {
                if (item.activeSelf)
                {
                    enemies.Add(JsonUtility.ToJson(item.transform.position));
                    //goblins.Add(item.GetComponent<Goblin>());
                }
            }
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<string>));
            serializer.WriteObject(fs, enemies);
            //DataContractJsonSerializer serializerGob = new DataContractJsonSerializer(typeof(List<Goblin>));
            //serializerGob.WriteObject(fsgob, goblins);
            //}
        }

        CheckSaves();
        Debug.Log("[GameManager] SaveGame");
        //UpdateGameState(GameState.RUNNING);
    }

    /// <summary>
    /// Загружает игру
    /// </summary>
    public void LoadGame(string path)
    {
        UpdateGameState(GameState.Pause);

        string PlayerTransform = path + @"\PlayerTransform.json";
        string PlayerPlayer = path + @"\PlayerData.json";
        string EnemyTransform = path + @"\EnemyTransform.json";
        //string EnemyGoblin = path + @"\EnemyGoblin.json";
        string GameManagerPath = path + @"\GameManager.json";

        if (!File.Exists(PlayerTransform) || !File.Exists(PlayerPlayer) || !File.Exists(EnemyTransform))
        {
            return;
        }

        Player player = Player.instance;

        // Загружаем Player (статы) героя
        using (StreamReader sr = new StreamReader(PlayerPlayer))
        {
            string data = sr.ReadToEnd();
            JsonUtility.FromJsonOverwrite(data, player);
            player.GetComponent<NavMeshAgent>().ResetPath();
        }
        // Загружаем GameManager
        using (StreamReader sr = new StreamReader(GameManagerPath))
        {
            string data = sr.ReadToEnd();
            JsonUtility.FromJsonOverwrite(data, this);
        }
        // Загружаем Transform героя
        using (StreamReader sr = new StreamReader(PlayerTransform))
        {
            string data = sr.ReadToEnd();
            player.gameObject.SetActive(false);
            player.transform.position = JsonUtility.FromJson<Vector3>(data);
            player.gameObject.SetActive(true);
        }
        // Загружаем Transform и Golbin врагов
        using (FileStream fset = new FileStream(EnemyTransform, FileMode.Open))
        {
            //using (FileStream fsgob = new FileStream(EnemyGoblin, FileMode.Open))
            //{
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<string>));
            List<string> enemies = serializer.ReadObject(fset) as List<string>;
            //DataContractJsonSerializer serializerGob = new DataContractJsonSerializer(typeof(List<Goblin>));
            //List<Goblin> newGoblins = serializerGob.ReadObject(fsgob) as List<Goblin>;
            for (int i = 0; i < EnemyManager.Instance.GetPool.Count; i++)
            {
                EnemyManager.Instance.GetPool[i].SetActive(false);
                //for (; i < enemies.Count; i++)
                //{
                    //EnemyManager.Instance.GetPool[i].transform.position = JsonUtility.FromJson<Vector3>(enemies[i]);

                    //var goblin = EnemyManager.Instance.GetPool[i].GetComponent<Goblin>();

                    ////JsonUtility.FromJson<Goblin>(goblins[i]);
                    ////goblin.Health = newGoblins[i].Health;
                    ////goblin.healthBar = goblin.gameObject.GetComponentsInChildren<GameObject>().Last();

                    //goblin.OnEnable();
                    //EnemyManager.Instance.GetPool[i].SetActive(true);
                    //goblin.agent.ResetPath();
                //}
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                var enemy = EnemyManager.Instance.GetEnemy;
                enemy.transform.position = JsonUtility.FromJson<Vector3>(enemies[i]);
                var goblin = enemy.GetComponent<Goblin>();

                //JsonUtility.FromJson<Goblin>(goblins[i]);
                //goblin.Health = newGoblins[i].Health;
                //goblin.healthBar = goblin.gameObject.GetComponentsInChildren<GameObject>().Last();
                
                goblin.OnEnable();
                enemy.SetActive(true);
                goblin.agent.ResetPath();
            }
            //}
        }
        EnemyManager.Instance.DestroyAllRagdolls();
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
