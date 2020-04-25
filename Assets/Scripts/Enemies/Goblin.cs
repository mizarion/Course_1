using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Goblin : AbstractEnemy
{
    private void Awake()
    {
        InitializeProperties(DataManager.Stats.Goblin.Health, DataManager.Stats.Goblin.Manapool, DataManager.Stats.Goblin.Experience, 
            DataManager.Stats.Goblin.Damage,"Goblin goblinovich", 1, DataManager.Stats.Goblin.ExperienceCost);
    }
}
