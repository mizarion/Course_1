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

    public EventVector3 OnClickEnviroment;  // Подписчик в эдиторе Hero.MavMeshAgent.destination

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
            //bool door = false;
            //if (hit.collider.gameObject.CompareTag("Doorway"))
            //{
            //    Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
            //    door = true;
            //}
            //else
            //{
            //    Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
            //}
            Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
            if (Input.GetMouseButtonDown(1))
            {
                //if (door)
                //{
                //    Transform doorway = hit.collider.transform;
                //    OnClickEnviroment?.Invoke(doorway.position + doorway.forward * 10);
                //}
                //else
                //{
                //}

                OnClickEnviroment?.Invoke(hit.point);
            }
        }
        else
        {
            Cursor.SetCursor(pointer, Vector2.zero, CursorMode.Auto);
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


