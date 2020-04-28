using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_dock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasManager.Instance.UpdateMessage("Press E to teleport");
            if (GameManager.Instance.canTeleport)
            {
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
                OnTriggerExit(other);
                Debug.Log("Teleport");
                }
            }
            else
            {
                CanvasManager.Instance.UpdateMessage("you should kill more goblins!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CanvasManager.Instance.UpdateMessage(string.Empty);
    }
}
