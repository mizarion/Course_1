using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_dungeon : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.canEnterInDungeon)
            {
                CanvasManager.Instance.UpdateMessage("Нажмите E чтобы войти в подземелье");
                if (Input.GetKey(KeyCode.E))
                {
                    CanvasManager.Instance.UpdateMessage(string.Empty);
                    GameManager.Instance.ChangeScene(Scenes.Dungeon, true);
                }
            }
            else
            {
                CanvasManager.Instance.UpdateMessage("Вы должны очистить локацию прежде чем войти");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CanvasManager.Instance.UpdateMessage(string.Empty);
    }
}
