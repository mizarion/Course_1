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

    [SerializeField] float _ScrollSens = 5;

    public EventVector3 OnClickEnviroment;  // ��������� � ������� Hero.MavMeshAgent.destination
    public EventGameObject onClickAttackable; // 

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
            // ���� ������ �������� ��������� IEnemy - ������ ��� ����, �������� ����� ���������
            if (hit.collider.GetComponent(typeof(IEnemy)) != null /*|| hit.collider.CompareTag("Enemy")*/)
            {
                Cursor.SetCursor(sword, new Vector2(16, 16), CursorMode.Auto);

                //// ���������� - �� ��������� �� ����� ��� ����� ����, � �� ��� ����
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


[System.Serializable]
public class EventGameObject : UnityEvent<GameObject> { }