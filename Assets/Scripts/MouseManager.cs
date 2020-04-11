using UnityEngine;
using UnityEngine.Events;

public class MouseManager : MonoBehaviour
{
    public LayerMask clickableLayer; // layermask used to isolate raycasts against clickable layers

    public Texture2D pointer; // normal mouse pointer
    public Texture2D target; // target mouse pointer
    public Texture2D doorway; // doorway mouse pointer

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
            Debug.Log("Raycast");
        }
        else
        {
            Debug.Log("not raycast");
            Cursor.SetCursor(pointer, Vector2.zero, CursorMode.Auto);
        }
    }
}

[System.Serializable]
public class EventVector3 : UnityEvent<Vector3> { }


