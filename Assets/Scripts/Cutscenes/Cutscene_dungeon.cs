using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс, отвечающий за переход между локациями. Переход с острова в подземелье
/// </summary>
public class Cutscene_dungeon : MonoBehaviour
{
    /// <summary>
    /// Встроенный метод. Используется для обработки нахождения в триггере
    /// </summary>
    /// <param name="other">Вошедший коллайдер</param>
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.canEnterInDungeon)
            {
                CanvasManager.Instance.UpdateMessage("Нажмите E чтобы войти в подземелье");
                if (Input.GetKey(KeyCode.E))
                {
                    CanvasManager.Instance.UpdateMessage(string.Empty);
                    GameManager.Instance.ChangeScene(Scenes.Dungeon, true);
                }
            }
            else
            {
                CanvasManager.Instance.UpdateMessage("Вы должны очистить локацию прежде чем войти");
            }
        }
    }

    /// <summary>
    /// Встроенный метод. Используется для обработки выхода из триггера
    /// </summary>
    /// <param name="other">Вышедший коллайдер</param>
    private void OnTriggerExit(Collider other)
    {
        CanvasManager.Instance.UpdateMessage(string.Empty);
    }
}
