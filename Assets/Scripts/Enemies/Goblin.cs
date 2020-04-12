using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Goblin : AbstractEnemy
{
    private void Awake()
    {
        InitializeProperties(DataManager.Stats.Goblin.Health, DataManager.Stats.Goblin.Manapool, DataManager.Stats.Goblin.Experience, "Goblin goblinovich", 1, DataManager.Stats.Goblin.ExperienceCost);
    }

    //private void Update()
    //{
    //    //if (Input.GetKeyDown(KeyCode.Q))
    //    if (GetAggressive(player.transform.position))
    //    {
    //        //Debug.LogError("here");
    //        Move(Player.instance.transform.position);
    //    }
    //}
}
