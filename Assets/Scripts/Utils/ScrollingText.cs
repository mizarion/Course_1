using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс, реализующий визуализацию информации об уроне.
/// </summary>
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

    /// <summary>
    /// Встроенный метод, который отрабатывает, каждые Time.fixedDeltaTime.  
    /// </summary>
    void FixedUpdate()
    {
        if (_timer > _duration)
        {
            Destroy(gameObject);
        }
        _timer += Time.deltaTime;
        transform.Translate(Vector3.up * Time.deltaTime * _speed);

    }

    /// <summary>
    /// Устанавливает текст и цвет 
    /// </summary>
    /// <param name="damage">Количество урона</param>
    /// <param name="color">Цвет</param>
    public void SetTextAndColor(string damage, Color color)
    {
        _textMesh.text = damage;
        _textMesh.color = color;
    }
}
