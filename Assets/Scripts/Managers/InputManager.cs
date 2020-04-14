using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// �������� �� ��������� �����
/// </summary>
public class InputManager : Singleton<InputManager>
{
    public LayerMask clickableLayer; // layermask used to isolate raycasts against clickable layers

    public Texture2D pointer;   // ����������� ������
    public Texture2D target;    // ������ � ���� ������
    //public Texture2D doorway; // ������ � ���� �����
    public Texture2D sword;     // ������ � ���� ����

    public EventVector3 OnClickEnviroment;  // ��������� � ������� Hero.MavMeshAgent.destination

    void Update()
    {
        // ���� ��� ������ ����, �� ������ ���������� �� ���������
        if (GameManager.instance.CurrentState == GameState.PREGAME)
        {
            return;
        }

        // �������� ������ �� ����� :)
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

        #region ������� ������

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


