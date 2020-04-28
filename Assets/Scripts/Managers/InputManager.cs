using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// �����, ����������� ��������� ����������������� �����
/// </summary>
public class InputManager : Singleton<InputManager>
{
#pragma warning disable 649

    [SerializeField] LayerMask clickableLayer; // layermask ������������ ��� ����������� ������������ �����

    [SerializeField] Texture2D pointer;   // ����������� ������
    [SerializeField] Texture2D target;    // ������ � ���� �������
    //public Texture2D doorway;           // ������ � ���� �����
    [SerializeField] Texture2D sword;     // ������ � ���� ����

    [SerializeField] float _scrollSens = 5;     // ���������������� �������� �����.

    public EventVector3 OnClickEnviroment;      // ������� ��� ����������� ������ � ������� ����� �����.  // ��������� � ������� Hero.NavMeshAgent.destination
    public EventGameObject onClickAttackable;   // ������� ��� ����� �����.

#pragma warning restore 649

    void Update()
    {
        // ���� ��� ������ ���� ��� ������, �� ������ ���������� �� ���������
        if (GameManager.Instance.CurrentState == GameState.Pregame || GameManager.Instance.CurrentState == GameState.Death)
        {
            return;
        }

        // ---------------------
        #region ������� ������ ����

        // �������� ������ �� ����� :)
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50, clickableLayer.value) && !EventSystem.current.IsPointerOverGameObject())
        {
            // ���� ������ �������� ��������� IEnemy - ������ ��� ����, �������� ����� ���������
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
        #region �������� �������� �����

        // �������� �������� ����� - �����������/��������� ������
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
        #region ������� ������

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
/// �����, (��������� UnityEvent<Vector3>) ��� �������� ������� � ������������ ����� Vector3.
/// </summary>
[System.Serializable]
public class EventVector3 : UnityEvent<Vector3> { }

/// <summary>
/// �����, (���������  UnityEvent<GameObject> ��� �������� ������� � ������������ ����� GameObject.
/// </summary>
[System.Serializable]
public class EventGameObject : UnityEvent<GameObject> { }