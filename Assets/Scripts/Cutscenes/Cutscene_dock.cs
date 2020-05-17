using UnityEngine;

/// <summary>
/// Класс, отвечающий за переход между локациями. Переход с причала на остров.
/// </summary>
public class Cutscene_dock : MonoBehaviour
{
    /// <summary>
    /// Встроенный метод. Используется для обработки нахождения в триггере
    /// </summary>
    /// <param name="other">Вошедший коллайдер</param>
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.canTeleport)
            {
                CanvasManager.Instance.UpdateMessage("Нажмите E для перехода в новую локацию");
                if (Input.GetKey(KeyCode.E))
                {
                    var timeScale = Time.timeScale;
                    Time.timeScale = 0;
                    Player.instance.agent.isStopped = true;
                    Player.instance.gameObject.SetActive(false);
                    Player.instance.transform.position = new Vector3(-68, 0.1f, 136);
                    Player.instance.gameObject.SetActive(true);
                    Player.instance.agent.isStopped = false;
                    Time.timeScale = timeScale;
                    StartCoroutine(Player.instance.ResetRotine());
                    OnTriggerExit(other);
                }
            }
            else
            {
                CanvasManager.Instance.UpdateMessage("Для перехода в другую локацию необходимо убить больше гоблинов!");
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
