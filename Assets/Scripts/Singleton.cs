using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T instance { get; private set; }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Debug.LogError($"[Singleton] Trying to instantiate a second instance of singleton class");
        }
        instance = (T)this;
    }
}
