using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    PREGAME,
    RUNNING,
    PAUSED
}


public class GameManager : Singleton<GameManager>
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
