using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Абстрактный класс представляющий базовый функционал врага
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public abstract class AbstractEnemy : AbstractCharacter, IEnemy
{
    ///// <summary>
    ///// Конструктор для создания абстрактного персонажа
    ///// </summary>
    ///// <param name="health">Список значений количества здоровья, соответствующий уровню героя</param>
    ///// <param name="mana">Список значений количества маны, соответствующий уровню героя</param>
    ///// <param name="experience">Список значений количества опыта, соответствующий уровню героя</param>
    ///// <param name="name">Имя персонажа</param>
    ///// <param name="lvl">Уровень персонажа</param>
    //public AbstractEnemy(List<int> health, List<int> mana, List<int> experience, string name, int lvl = 1) : base(health, mana, experience, name, lvl) { }

    protected Player player;

    protected float _expCost;

    [SerializeField] protected float _agrRadius = 10;
    [SerializeField] protected float _attackRadius; //= 2;
    [SerializeField] protected float _dps = 10;
    [SerializeField] protected float _movespeed = 3;
    [SerializeField] protected float _attackRate = 1;

    /// <summary>
    /// Метод (Конструктор) для создания абстрактного врага.
    /// </summary>
    /// <param name="health">Список значений количества здоровья, соответствующий уровню героя</param>
    /// <param name="mana">Список значений количества маны, соответствующий уровню героя</param>
    /// <param name="experience">Список значений количества опыта, соответствующий уровню героя</param>
    /// <param name="name">Имя персонажа</param>
    /// <param name="lvl">Уровень персонажа</param>
    /// <param name="expCost">Получаемое количество опыта за убийство</param>
    protected void InitializeProperties(List<int> health, List<int> mana, List<int> experience, string name, int lvl = 1, float expCost = 0)
    {
        base.InitializeProperties(health, mana, experience, name, lvl);
        _expCost = expCost;

        player = Player.instance;

        agent.stoppingDistance += .5f;
        _attackRadius = agent.stoppingDistance + .5f;
    }

    /// <summary>
    /// Проверяет, находится ли игрок в радиусе агра
    /// </summary>
    /// <param name="target">Цель</param>
    /// <param name="agrRadius">Радиус агра</param>
    /// <returns></returns>
    public virtual bool GetAggressive(Vector3 target, float agrRadius = 10)
    {
        return Vector3.Distance(transform.position, target) < agrRadius;
    }


    float timer;

    /// <summary>
    /// Реализует движение к переданной позиции
    /// </summary>
    /// <param name="target">Координаты</param>
    /// <param name="movespeed">Скорость передвижения</param>
    /// <param name="agrRadius">Дистанция, с которой враг замечает героя и заагривается</param>
    public virtual void Move(Vector3 target, float movespeed = 3,/* float agrRadius = 10,*/ float agrRadius = 1)
    {
        agent.speed = movespeed;
        //agent.acceleration = 20;
        //agent.stoppingDistance = agrRadius;
        agent.SetDestination(target);
        if (GetAggressive(player.transform.position, _attackRadius) && timer > _attackRate)
        {
            timer = 0;
            Attack(player, _dps);
        }
        timer += Time.deltaTime;
    }

    public override void Die()
    {
        player.Experience += _expCost;
        base.Die();
    }

    protected virtual void FixedUpdate()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);
        //if (GetAggressive(player.transform.position, _agrRadius))
        if (Vector3.Distance(player.transform.position, transform.position) < _agrRadius)
        {
            Move(player.transform.position, _movespeed);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _agrRadius);
        Gizmos.DrawWireSphere(transform.position, _attackRadius);
    }
}
