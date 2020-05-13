using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsManager : Singleton<SkillsManager>
{
    [SerializeField, Range(50, 100)] float hpHealCost = 50;
    [SerializeField, Range(10, 20)] float agrCost = 50;
    [SerializeField, Range(20, 50)] float agrRange = 20;

    /// <summary>
    /// Первая способность - агрессия 
    /// </summary>
    public void FirstSkill()
    {
        if (GameManager.Instance.CurrentState == GameState.Running && GameManager.Instance.avaibleFirstSkill && Player.instance.Manapool > hpHealCost)
        {

            Player.instance.Manapool -= agrCost;
            foreach (var enemy in EnemyManager.Instance.GetAbstractEnemyPool)
            {
                if (enemy.gameObject.activeSelf && Vector3.Distance(enemy.transform.position, Player.instance.transform.position) < agrRange)
                {
                    enemy.agent.SetDestination(Player.instance.transform.position);
                }
            }
        }
    }

    /// <summary>
    /// Вторая способность - восстановление хм за счет маны
    /// </summary>
    public void SecondSkill()
    {
        if (GameManager.Instance.CurrentState == GameState.Running && GameManager.Instance.avaibleSecondSkill && Player.instance.Manapool > agrCost)
        {
            Player.instance.Health += hpHealCost;
            Player.instance.Manapool -= hpHealCost;
            CanvasManager.Instance.UpdateHUD();
        }
    }
}
