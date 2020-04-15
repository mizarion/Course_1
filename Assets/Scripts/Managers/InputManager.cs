using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Отвечает за обработку ввода
/// </summary>
public class InputManager : Singleton<InputManager>
{
    public LayerMask clickableLayer; // layermask used to isolate raycasts against clickable layers

    public Texture2D pointer;   // стандартный курсор
    public Texture2D target;    // курсор в виде прицел
    //public Texture2D doorway; // курсор в виде двери
    public Texture2D sword;     // курсор в виде меча

    [SerializeField] float _ScrollSens = 5;

    public EventVector3 OnClickEnviroment;  // Подписчик в эдиторе Hero.MavMeshAgent.destination
    public EventGameObject onClickAttackable; // 

    void Update()
    {
        // Если это начало игры, то данный функционал не требуется
        if (GameManager.instance.CurrentState == GameState.PREGAME)
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

                //// Недостаток - на добегание до места где стоял враг, а не сам враг
                //if (Input.GetMouseButtonDown(1))
                //{
                //    if (Vector3.Distance(hit.point, Player.instance.transform.position) < 3)
                //    {
                //        onClickAttackable?.Invoke(hit.collider.gameObject /*.GetComponent<ICharacter>()*/);
                //    }
                //    else
                //    {
                //        OnClickEnviroment?.Invoke(hit.point);
                //    }
                //}
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


        if (Input.GetAxis("Mouse ScrollWheel") < 0 && SmoothFollowTarget.instance.offset.magnitude < 30)
        {
            SmoothFollowTarget.instance.offset += SmoothFollowTarget.instance.offset * Time.deltaTime * _ScrollSens;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0 && SmoothFollowTarget.instance.offset.magnitude > 10)
        {
            SmoothFollowTarget.instance.offset -= SmoothFollowTarget.instance.offset * Time.deltaTime * _ScrollSens;
        }


        #region Нажатия клавиш

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CanvasManager.instance.PauseHandler();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CanvasManager.instance.RestartHandler();
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            CanvasManager.instance.SaveHandler();
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            CanvasManager.instance.LoadHandler();
        }
        #endregion
    }
}

[System.Serializable]
public class EventVector3 : UnityEvent<Vector3> { }


[System.Serializable]
public class EventGameObject : UnityEvent<GameObject> { }