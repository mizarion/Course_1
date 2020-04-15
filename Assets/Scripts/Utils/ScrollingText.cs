using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class ScrollingText : MonoBehaviour
{
    TextMesh _textMesh;
    [SerializeField] float _duration = 3;
    [SerializeField] float _speed = 3;
    float _timer;

    void Awake()
    {
        _textMesh = GetComponent<TextMesh>();
        _timer = 0;
        transform.LookAt(Camera.main.transform.position); 
    }

    void Update()
    {
        if (_timer > _duration)
        {
            Destroy(gameObject);
        }
        _timer += Time.deltaTime;
        transform.Translate(Vector3.up * Time.deltaTime * _speed);

    }

    public void SetTextAndColor(string damage, Color color)
    {
        _textMesh.text = damage;
        _textMesh.color = color;
    }
}
