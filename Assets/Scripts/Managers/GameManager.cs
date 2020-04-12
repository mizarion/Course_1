using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    PREGAME,    // начальное состояние / главное меню
    RUNNING,    // в процессе игры
    PAUSED      // пауза
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] public GameState CurrentState { get; private set; }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
         DontDestroyOnLoad(CanvasManager.instance.gameObject);
        // DontDestroyOnLoad(InputManager.instance.gameObject);

        CurrentState = GameState.PREGAME;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    TogglePause();
        //}
    }



    /// <summary>
    /// Обновляет состояние игры
    /// </summary>
    /// <param name="state">Состояние, в которое переходит</param>
    public void UpdateGameState(GameState state)
    {
        GameState privState = CurrentState;
        CurrentState = state;
        Debug.Log($"cur state:{CurrentState}, privState: {privState}");

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


    /// <summary>
    /// Отвечает за паузу
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
        CurrentState = GameState.RUNNING;
    }


    /// <summary>
    /// Перезапускает игру
    /// </summary>
    public void RestartGame()
    {
        AsyncOperation unloading = SceneManager.UnloadSceneAsync("PoligonVikings");

        //AsyncOperation loading = SceneManager.LoadSceneAsync(DataManager.Scenes.startScene);

        CurrentState = GameState.PREGAME;
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
