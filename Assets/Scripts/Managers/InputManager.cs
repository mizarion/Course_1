using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Класс, реализующий обработку пользовательского ввода
/// </summary>
public class InputManager : Singleton<InputManager>
{
#pragma warning disable 649

    [SerializeField] LayerMask clickableLayer; // layermask используется для определения кликабельных слоев

    [SerializeField] Texture2D pointer;   // стандартный курсор
    [SerializeField] Texture2D target;    // курсор в виде прицела
    //public Texture2D doorway;           // курсор в виде двери
    [SerializeField] Texture2D sword;     // курсор в виде меча

    [SerializeField] float _scrollSens = 5;     // Чувствительность колесика мышки.

    public EventVector3 OnClickEnviroment;      // Событие для перемещения игрока с помощью клика мышки.  // Подписчик в эдиторе Hero.NavMeshAgent.destination
    public EventGameObject onClickAttackable;   // Событие для атаки врага.

#pragma warning restore 649

    void Update()
    {
        // Если это начало игры или смерть, то данный функционал не требуется
        if (GameManager.Instance.CurrentState == GameState.Pregame || GameManager.Instance.CurrentState == GameState.Death)
        {
            return;
        }

        // ---------------------
        #region Нажатие кнопок мыши

        // Стреляем лучами по сцене :)
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50, clickableLayer.value) && !EventSystem.current.IsPointerOverGameObject())
        {
            // Если объект содержит интерфейс IEnemy - значит это враг, которого можно атаковать
            if (hit.collider.GetComponent(typeof(AbstractEnemy)) != null /*|| hit.collider.CompareTag("Enemy")*/)
            {
                Cursor.SetCursor(sword, new Vector2(16, 16), CursorMode.Auto);

                if (Input.GetMouseButtonDown(0))
                {
                    onClickAttackable?.Invoke(hit.collider.gameObject);
                }
            }
            else
            {
                Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
                if (Input.GetMouseButton(1))
                {
                    OnClickEnviroment?.Invoke(hit.point);
                }
            }
        }
        else
        {
            Cursor.SetCursor(pointer, Vector2.zero, CursorMode.Auto);
        }

        #endregion
        // ---------------------

        // ---------------------
        #region Вращение колесика мышки

        // Вращение колесика мышки - Приближение/Отдаление камеры
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && SmoothFollowTarget.Instance.Offset.magnitude < 30)
        {
            SmoothFollowTarget.Instance.Offset += SmoothFollowTarget.Instance.Offset * Time.deltaTime * _scrollSens;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0 && SmoothFollowTarget.Instance.Offset.magnitude > 10)
        {
            SmoothFollowTarget.Instance.Offset -= SmoothFollowTarget.Instance.Offset * Time.deltaTime * _scrollSens;
        }

        #endregion
        // ---------------------

        // ---------------------
        #region Нажатия клавиш

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CanvasManager.Instance.PauseHandler();
        }
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    CanvasManager.Instance.RestartHandler();
        //}
        //if (Input.GetKeyDown(KeyCode.F5))
        //{
        //    CanvasManager.Instance.SaveHandler();
        //}
        //if (Input.GetKeyDown(KeyCode.F6))
        //{
        //    CanvasManager.Instance.LoadHandler();
        //}

        #endregion
        // ---------------------

    }
}

/// <summary>
/// Класс, (наследник UnityEvent<Vector3>) для создание событий с передаваемым типом Vector3.
/// </summary>
[System.Serializable]
public class EventVector3 : UnityEvent<Vector3> { }

/// <summary>
/// Класс, (наследник  UnityEvent<GameObject> для создание событий с передаваемым типом GameObject.
/// </summary>
[System.Serializable]
public class EventGameObject : UnityEvent<GameObject> { }