using UnityEngine;
using UnityEngine.Events;

public class InputManager : Singleton<InputManager>
{
    public LayerMask clickableLayer; // layermask used to isolate raycasts against clickable layers

    public Texture2D pointer; // normal mouse pointer
    public Texture2D target; // target mouse pointer
    public Texture2D doorway; // doorway mouse pointer
    public Texture2D sword;

    public EventVector3 OnClickEnviroment;

    void Update()
    {
        // Raycast into scene
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50, clickableLayer.value))
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //GameManager.instance.UpdateGameState(GameState.PAUSED);
            CanvasManager.instance.PauseHandler();
            Debug.Log($"you press escape: timescale: {Time.timeScale}, curGameState: {GameManager.instance.CurrentState}");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //GameManager.instance.RestartGame();
            CanvasManager.instance.RestartHandler();
        }
    }
}

[System.Serializable]
public class EventVector3 : UnityEvent<Vector3> { }


