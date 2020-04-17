using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Класс, реализующий обработку пользовательского ввода
/// </summary>
public class InputManager : Singleton<InputManager>
{
    public LayerMask clickableLayer; // layermask используется для изоляции raycast от кликабельных слоев

    public Texture2D pointer;   // стандартный курсор
    public Texture2D target;    // курсор в виде прицела
    //public Texture2D doorway; // курсор в виде двери
    public Texture2D sword;     // курсор в виде меча

    [SerializeField] float _ScrollSens = 5;

    public EventVector3 OnClickEnviroment;  // Подписчик в эдиторе Hero.MavMeshAgent.destination
    public EventGameObject onClickAttackable; // 

    void Update()
    {
        // Если это начало игры, то данный функционал не требуется
        if (GameManager.Instance.CurrentState == GameState.PREGAME)
        {
            return;
        }

        // Стреляем лучами по сцене :)
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50, clickableLayer.value) && !EventSystem.current.IsPointerOverGameObject())
        {
            // Если объект содержит интерфейс IEnemy - значит это враг, которого можно атаковать
            if (hit.collider.GetComponent(typeof(IEnemy)) != null /*|| hit.collider.CompareTag("Enemy")*/)
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
                if (Input.GetMouseButtonDown(1))
                {
                    OnClickEnviroment?.Invoke(hit.point);
                }
            }
        }
        else
        {
            Cursor.SetCursor(pointer, Vector2.zero, CursorMode.Auto);
        }

        // Вращение колесика мышки - Приближение/Отдаление камеры
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && SmoothFollowTarget.Instance.Offset.magnitude < 30)
        {
            SmoothFollowTarget.Instance.Offset += SmoothFollowTarget.Instance.Offset * Time.deltaTime * _ScrollSens;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0 && SmoothFollowTarget.Instance.Offset.magnitude > 10)
        {
            SmoothFollowTarget.Instance.Offset -= SmoothFollowTarget.Instance.Offset * Time.deltaTime * _ScrollSens;
        }


        #region Нажатия клавиш

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CanvasManager.Instance.PauseHandler();
        }
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    CanvasManager.Instance.RestartHandler();
        //}
        if (Input.GetKeyDown(KeyCode.F5))
        {
            CanvasManager.Instance.SaveHandler();
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            CanvasManager.Instance.LoadHandler();
        }
        #endregion
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