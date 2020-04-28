using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneBoat : MonoBehaviour
{
#pragma warning disable 649

    [SerializeField] GameObject cutscene;
    //[SerializeField] GameObject trigger;
    [SerializeField] GameObject hero;
    [SerializeField] GameObject boatFBX;
    [SerializeField] GameObject boatM;

#pragma warning restore 649

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            cutscene.SetActive(true);
            hero.transform.position = new Vector3(-68.5f, .1f, 135);
            //StartCoroutine(BoatRotine());
        }
    }

    //IEnumerator BoatRotine()
    //{
    //    //Debug.Log("Press");
    //    //// скрываем героя
    //    //hero.SetActive(false);
    //    //// скрываем корабль
    //    //boatFBX.SetActive(false);
    //    //// запускаем катсцену
    //    //cutscene.SetActive(true);

    //    //float timer = 0;
    //    //while (timer < 12)
    //    //{
    //    //    timer += Time.deltaTime;
    //    //    hero.transform.position = boatM.transform.position;
    //    //    yield return null;
    //    //}

    //    ////yield return new WaitForSeconds(10);

    //    //hero.transform.position = new Vector3(-68.5f, .1f, 135);
    //    //hero.SetActive(true);

    //    //cutscene.SetActive(false);

    //    //boatFBX.transform.position = new Vector3();
    //    //boatFBX.SetActive(true);

    //}
}
