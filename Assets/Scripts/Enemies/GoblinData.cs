using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinData 
{
    public GoblinData(Goblin goblin)
    {
        Health = goblin.Health;
        Manapool = goblin.Manapool;
        Level= goblin.Level;
    }

    //public GoblinData(GoblinBoss goblin)
    //{
    //    Health = goblin.Health;
    //    Manapool = goblin.Manapool;
    //    Level = goblin.Level;
    //}

    public float Health;
    public float Manapool;
    public int Level;
}
