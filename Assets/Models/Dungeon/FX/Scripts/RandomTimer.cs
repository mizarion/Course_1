using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RandomTimer : MonoBehaviour
{
    public Vector2 randomRange = new Vector2(1, 5);
    public UnityEvent OnTimeout;

    void Start()
    {
        Invoke("Tick", Random.Range(randomRange.x, randomRange.y));
    }

    void Tick()
    {
        OnTimeout.Invoke();
        Start();
    }
}
