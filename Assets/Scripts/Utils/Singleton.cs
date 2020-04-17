using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс, реализующий паттерн Singleton. 
/// </summary>
/// <typeparam name="T">Тип</typeparam>
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"[Singleton] Trying to instantiate a second instance of singleton class");
        }
        Instance = (T)this;
    }
}
