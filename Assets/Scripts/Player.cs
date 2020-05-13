using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

/// <summary>
/// Класс, реализующий игрока
/// </summary>
[System.Serializable]
public class Player : AbstractCharacter, System.ICloneable
{
    [HideInInspector] public static Player instance;
    bool isAttacking;
    GameObject attackTarget;

    public override int Level
    {
        get => base.Level;
        set
        {
            base.Level = value;
            if (value == 3)
            {
                GameManager.Instance.avaibleFirstSkill = true;
                CanvasManager.Instance.Skill1.gameObject.SetActive(true);
            }
            if (value == 6)
            {
                GameManager.Instance.avaibleSecondSkill = true;
                CanvasManager.Instance.Skill2.gameObject.SetActive(true);
            }
        }
    }

    private void Awake()
    {
        InitializeProperties(DataManager.Stats.Player.Health, DataManager.Stats.Player.Manapool, DataManager.Stats.Player.Experience, DataManager.Stats.Player.Damage, "Hero");

        instance = this;

        StartCoroutine(Recovery(5, 5));
    }

    private void FixedUpdate()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    /// <summary>
    /// Класс, служащий для сохранения данных героя
    /// </summary>
    public class PlayerData
    {
        public float Health;
        public float Manapool;
        public int Level;
        public float Experience;
    }

    /// <summary>
    /// Обрабатывает сохраненные данные
    /// </summary>
    /// <param name="data">Сохраненные данные</param>
    public void ApplyPlayerData(PlayerData data)
    {
        this.Level = data.Level;
        this.Health = data.Health;
        this.Manapool = data.Manapool;
        this.Experience = data.Experience;
    }

    /// <summary>
    /// Корутина для атаки
    /// Если Расстояние больше радиуса атаки, то герой сначала подбежит. И лишь потом ударит.
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackTarget()
    {
        agent.isStopped = false;

        while (Vector3.Distance(transform.position, attackTarget.transform.position) > 2)
        {
            agent.stoppingDistance = 2;
            agent.destination = attackTarget.transform.position;
            yield return null;
        }
        // Todo: Донастроить
        agent.isStopped = true;
        transform.LookAt(attackTarget.transform);
        Attack(attackTarget.GetComponent<ICharacter>());
    }

    /// <summary>
    /// Обработчик события завершения атаки
    /// </summary>
    public void Hit()
    {
        agent.isStopped = false;
        StopCoroutine(AttackTarget());
        isAttacking = false;
    }

    /// <summary>
    /// Обработчик события атаки
    /// </summary>
    /// <param name="target">Цель атаки</param>
    public void AttackHandler(GameObject target)
    {
        if (!isAttacking)
        {
            isAttacking = true;
            attackTarget = target;
            StartCoroutine(AttackTarget());
        }
    }

    /// <summary>
    /// Обрабатывает получение урона в размере damage.
    /// </summary>
    /// <param name="damage">Получаемый урон</param>
    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
        CanvasManager.Instance.UpdateHUD();
    }

    /// <summary>
    /// Восполняет здоровье и ману персонажа.
    /// Переопределение: Обновляет HUD
    /// </summary>
    /// <param name="healRecovery">Множитель скорости восстановления здоровья</param>
    /// <param name="manaRecovery">Множитель скорости восстановления маны</param>
    /// <param name="delay">Задержка между лечениями</param>
    /// <returns></returns>
    public override IEnumerator Recovery(float healRecovery = 1, float manaRecovery = 1, float delay = 0.1f)
    {
        yield return null;
        while (true)
        {
            Health += healRecovery * _listHp[Level] / 1000;
            Manapool += manaRecovery * _listMp[Level] / 1000;
            CanvasManager.Instance.UpdateHUD();
            yield return new WaitForSeconds(delay);
        }
    }

    /// <summary>
    /// Обработчик смерти героя
    /// </summary>
    public override void Die(/*float delay = 0*/)
    {
        CanvasManager.Instance.DeathHandler();
    }

    /// <summary>
    /// Клонирует данные героя
    /// </summary>
    /// <returns></returns>
    public object Clone()
    {
        return new PlayerData() { Health = this.Health, Manapool = this.Manapool, Level = this.Level, Experience = this.Experience }; ;
    }

    /// <summary>
    /// Завершает все корутины и после возобновляет Recovery
    /// </summary>
    /// <returns></returns>
    public IEnumerator ResetRotine()
    {
        Hit();
        StopAllCoroutines();
        yield return null;
        StartCoroutine(Recovery(5, 5));
    }
}
