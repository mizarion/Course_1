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
    Pregame,    // начальное состояние / главное меню
    Running,    // в процессе игры
    Pause,      // пауза
    Death
}

/// <summary>
/// Содержит данные о сценах
/// </summary>
public enum Scenes
{
    StartScene = 0,
    MainGame = 1,
    Dungeon = 2
}

/// <summary>
/// Отвечает за главную логику игры
/// </summary>
public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// Текущее состояние игры
    /// </summary>
    public GameState CurrentState;

    /// <summary>
    /// Текущая сцена
    /// </summary>
    public Scenes CurrentScene;

    [HideInInspector] public bool isSaveOneAvailable;   // Доступность 1 сохранения
    [HideInInspector] public bool isSaveTwoAvailable;   // Доступность 2 сохранения
    [HideInInspector] public bool isSaveThreeAvailable; // Доступность 3 сохранения

    [SerializeField] int _deathCounter;     // Счетчик убийств

    public bool canTeleport;        // Возможность перемещения на остров
    public bool canEnterInDungeon;  // Возможность перемещения в подземелье

    public bool avaibleFirstSkill;  // Доступность 1 способности
    public bool avaibleSecondSkill; // Доступность 2 способности

    /// <summary>
    /// Свойство, обрабатывающее количество смертей
    /// </summary>
    public int DeathCounter
    {
        get => _deathCounter;
        set
        {
            _deathCounter = value;
            if (_deathCounter == 10)
            {
                EnemyManager.Instance.StartNewWawe(10, 4);
            }
            else if (_deathCounter == 20)
            {
                canTeleport = true;
            }
            else if (_deathCounter == 30)
            {
                canEnterInDungeon = true;
            }
        }
    }

    /// <summary>
    /// Встроенный метод. Используется для задавания уничтожаемых объектов, инициализации полей и проверки существующих сохранений. 
    /// </summary>
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(CanvasManager.Instance.gameObject);
        DontDestroyOnLoad(AudioManager.Instance.gameObject);

        CurrentState = GameState.Pregame;
        CurrentScene = Scenes.StartScene;

        CheckSaves();
    }

    /// <summary>
    /// Изменяет и обрабатывает изменение состояния игры
    /// </summary>
    /// <param name="state">Состояние, в которое переходит</param>
    public void UpdateGameState(GameState state)
    {
        CurrentState = state;
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

    /// <summary>
    /// Обрабатывает ограничение fps
    /// </summary>
    /// <param name="count">Максимальное количество FPS</param>
    public void LimitFPS(int count)
    {
        Application.targetFrameRate = count;
    }

    /// <summary>
    /// Сбрасывает данные GameManager
    /// </summary>
    void ResetGameManager()
    {
        _deathCounter = 0;
        canTeleport = false;
        canEnterInDungeon = false;
        avaibleFirstSkill = false;
        avaibleSecondSkill = false;
        CanvasManager.Instance.Skill1.gameObject.SetActive(false);
        CanvasManager.Instance.Skill2.gameObject.SetActive(false);
    }

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
        if (CanvasManager.Instance.needLoad)
        {
            using (StreamReader sr = new StreamReader(CanvasManager.Instance.path + @"\CurrentLevel.json"))
            {
                CurrentScene = JsonUtility.FromJson<Scenes>(sr.ReadToEnd());
            }
        }
        else
        {
            CurrentScene = Scenes.MainGame;
        }

        SceneManager.LoadSceneAsync((int)CurrentScene, LoadSceneMode.Additive).completed += delegate (AsyncOperation op)
        {
            if (CanvasManager.Instance.needLoad)
            {
                LoadGame(CanvasManager.Instance.path);

                CanvasManager.Instance.needLoad = false;
            }
            else
            {
                CanvasManager.Instance.ShowStory();
                ResetGameManager();
            }
            CanvasManager.Instance.ActivateUI(showHUD: true);
            UpdateGameState(GameState.Running);
        };
    }

    /// <summary>
    /// Обрабатывает переход между игровыми сценами, с загрузкой необходимых данных
    /// </summary>
    /// <param name="newScene">Новая сцена</param>
    /// <param name="changeLocation">Это загрузка или переход в новую локацию?</param>
    public void ChangeScene(Scenes newScene, bool changeLocation = false)
    {
        TogglePause();

        if (CurrentScene == Scenes.StartScene)
        {
            SceneManager.LoadSceneAsync((int)newScene, LoadSceneMode.Additive).completed += delegate (AsyncOperation obj2)
            {
                CurrentScene = newScene;
                if (CanvasManager.Instance.needLoad)
                {
                    LoadGame(CanvasManager.Instance.path);
                    CanvasManager.Instance.needLoad = false;
                }
            };
        }
        else
        {
            Player.PlayerData playerData = Player.instance.Clone() as Player.PlayerData;
            SceneManager.UnloadSceneAsync((int)CurrentScene).completed += delegate (AsyncOperation obj)
            {
                SceneManager.LoadSceneAsync((int)newScene, LoadSceneMode.Additive).completed += delegate (AsyncOperation obj2)
                {
                    CurrentScene = newScene;
                    if (changeLocation)
                    {
                        Player.instance.ApplyPlayerData(playerData);
                    }
                    else  //(CanvasManager.Instance.needLoad)   // тут желательно проверить, но вроде и так работает
                    {
                        LoadGame(CanvasManager.Instance.path);
                        CanvasManager.Instance.needLoad = false;
                    }
                };
            };
        }
        TogglePause();
    }


    /// <summary>
    /// Перезапускает игру, выгружая текущую сцену и меняя режим интерфейса на стартовое меню.  
    /// </summary>
    public void RestartGame()
    {
        SceneManager.UnloadSceneAsync((int)CurrentScene).completed += delegate (AsyncOperation asyncOperation)
        {
            CurrentScene = Scenes.StartScene;
            UpdateGameState(GameState.Pregame);
        };
    }

    #region Save & Load     

    /// <summary>
    /// Проверяет существование сохранений и сообщает об их наличии
    /// </summary>
    public void CheckSaves()
    {
        CheckSave(@"Saves/Save1", CanvasManager.Instance.saveOne, CanvasManager.Instance.loadOne);
        CheckSave(@"Saves/Save2", CanvasManager.Instance.saveTwo, CanvasManager.Instance.loadTwo);
        CheckSave(@"Saves/Save3", CanvasManager.Instance.saveThree, CanvasManager.Instance.loadThree);
        CanvasManager.Instance.UpdateSaves();
    }

    /// <summary>
    /// Проверяет наличие сохранения
    /// </summary>
    /// <param name="path">Путь сохранения</param>
    /// <param name="save">Скрипт на кнопке сохранения</param>
    /// <param name="load">Скрипт на кнопке загрузки</param>
    private void CheckSave(string path, SaveSlotButton save, SaveSlotButton load)
    {
        if (File.Exists($"{path}/CurrentLevel.json") && File.Exists($"{path}/EnemyGoblin.json") && File.Exists($"{path}/EnemyTransform.json") && File.Exists($"{path}/GameManager.json")
            && File.Exists($"{path}/PlayerData.json") && File.Exists($"{path}/PlayerTransform.json") && File.Exists($"{path}/SaveButton.json"))
        {
            using (StreamReader sr = new StreamReader($"{path}/SaveButton.json"))
            {
                JsonUtility.FromJsonOverwrite(sr.ReadToEnd(), save);
                load.DateTMP = save.DateTMP;
                load.isAvaible = true;
            }
        }
    }

    /// <summary>
    /// Сохраняет игру
    /// </summary>
    public void SaveGame(SaveSlotButton button)
    {
        UpdateGameState(GameState.Pause);
        button.DateTMP = System.DateTime.Now.ToString();
        string path = button.path;
        Directory.CreateDirectory(path);
        string PlayerTransform = path + @"\PlayerTransform.json";
        string PlayerPlayer = path + @"\PlayerData.json";
        string EnemyTransform = path + @"\EnemyTransform.json";
        string EnemyGoblin = path + @"\EnemyGoblin.json";
        string ButtonPath = path + @"\SaveButton.json";
        string GameManagerPath = path + @"\GameManager.json";
        string CurrentLevelPath = path + @"\CurrentLevel.json";
        Player player = Player.instance;

        // Сохраняем GameManager
        using (StreamWriter sw = new StreamWriter(GameManagerPath))
        {
            string data = JsonUtility.ToJson(this, true);
            sw.Write(data);
        }
        // Сохраняем отдельно информацию о текущей сцене
        using (StreamWriter sw = new StreamWriter(CurrentLevelPath))
        {
            string data = JsonUtility.ToJson(CurrentScene, true);
            sw.Write(data);
        }
        // Сохраняем информацию кнопки
        using (StreamWriter sw = new StreamWriter(ButtonPath))
        {
            string data = JsonUtility.ToJson(button, true);
            sw.Write(data);
        }
        // Сохраняем Player героя
        using (StreamWriter sw = new StreamWriter(PlayerPlayer))
        {
            string data = JsonUtility.ToJson(player, true);
            sw.Write(data);
        }
        // Сохраняем transform.position героя
        using (StreamWriter sw = new StreamWriter(PlayerTransform))
        {
            string data = JsonUtility.ToJson(player.transform.position, true);
            sw.Write(data);
        }
        // сохраняем Transform врагов
        using (FileStream fs = new FileStream(EnemyTransform, FileMode.Create))
        {
            List<string> enemies = new List<string>();
            foreach (var item in EnemyManager.Instance.GetActiveAbstractEnemyPool)
            {
                enemies.Add(JsonUtility.ToJson(item.transform.position));
            }
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<string>));
            serializer.WriteObject(fs, enemies);
        }
        // Сохраняем информацию (Goblin) об активных врагах
        using (StreamWriter sw = new StreamWriter(EnemyGoblin))
        {
            foreach (var goblin in EnemyManager.Instance.GetActiveAbstractEnemyPool)
            {
                sw.WriteLine(JsonUtility.ToJson(new GoblinData(goblin as Goblin)));
            }
        }

        CheckSaves();
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
        string EnemyGoblin = path + @"\EnemyGoblin.json";

        string GameManagerPath = path + @"\GameManager.json";

        Player player = Player.instance;

        // Загружаем GameManager
        using (StreamReader sr = new StreamReader(GameManagerPath))
        {
            string data = sr.ReadToEnd();
            JsonUtility.FromJsonOverwrite(data, this);
        }
        // Загружаем Player (статы) героя
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
            player.gameObject.SetActive(false);
            player.transform.position = JsonUtility.FromJson<Vector3>(data);
            player.gameObject.SetActive(true);
        }
        // Загружаем Transform и Golbin врагов
        using (FileStream fset = new FileStream(EnemyTransform, FileMode.Open))
        using (StreamReader swgob = new StreamReader(EnemyGoblin))
        {
            DataContractJsonSerializer Transformserializer = new DataContractJsonSerializer(typeof(List<string>));
            List<string> enemiesTransform = Transformserializer.ReadObject(fset) as List<string>;
            // Деактивируем врагов
            foreach (var item in EnemyManager.Instance.GetActiveAbstractEnemyPool)
            {
                item.gameObject.SetActive(false);
            }
            for (int i = 0; i < enemiesTransform.Count; i++)
            {
                var enemy = EnemyManager.Instance.GetEnemy;
                enemy.SetActive(false);
                enemy.transform.position = JsonUtility.FromJson<Vector3>(enemiesTransform[i]);
                var goblin = enemy.GetComponent<Goblin>();
                GoblinData goblinData = JsonUtility.FromJson<GoblinData>(swgob.ReadLine());
                goblin.ApplyData(goblinData);
                goblin.gameObject.SetActive(true);
                goblin.agent.ResetPath();
            }
        }
        StartCoroutine(player.ResetRotine());
        EnemyManager.Instance.DestroyAllRagdolls();
        CanvasManager.Instance.UpdateHUD();
        CanvasManager.Instance.UpdateSkills();
    }

    /// <summary>
    /// Проверяет текущую сцену и загружаемую, и решает, что дальше делать.
    /// </summary>
    /// <param name="path">Путь до папки с сохранениями</param>
    public void CheckCurrentScene(string path)
    {
        string CurrentLevelPath = path + @"\CurrentLevel.json";

        // Загружаем информацию о возможной сцене
        using (StreamReader sr = new StreamReader(CurrentLevelPath))
        {
            var privScene = CurrentScene;
            var newScene = JsonUtility.FromJson<Scenes>(sr.ReadToEnd());
            if (privScene != newScene)
            {
                ChangeScene(newScene);
            }
            else
            {
                LoadGame(path);
            }
        }
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
