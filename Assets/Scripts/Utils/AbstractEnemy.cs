using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Абстрактный класс, дополняющий AbstractCharacter, реализуя базовый функционал врага
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public abstract class AbstractEnemy : AbstractCharacter
{
    protected Player player;           // Ссылка на главного героя
    protected float _expMultiplier;    // Множитель получаемого опыта за убийство этого противника

    [SerializeField] protected float _agrRadius = 10;       // Радиус, с которого враг замечает героя
    [SerializeField] protected float _attackRadius; //= 2;  // Радиус, с которого враг начинает атаковать героя
    [SerializeField] protected float _attackRate = 1;       // Время необходимое для совершения одной атаки
    //[SerializeField] float _multiplier = 3;

    [Header("GameObject")]
    [SerializeField] protected GameObject DieRagdoll;       // Ragdoll’ для анимации смерти.
    [SerializeField] protected GameObject healthBar;        // Полоска здоровья

    float timer;    // Отсчитывает время прошедшее с предыдущей атаки

    public override float Health
    {
        get => base.Health;
        set
        {
            base.Health = value;
            healthBar.transform.localScale = new Vector3(Health / _listHp[Level], transform.localScale.y, transform.localScale.z) / 4;
        }
    }

    /// <summary>
    /// Метод (Конструктор) для создания абстрактного врага.
    /// </summary>
    /// <param name="health">Список значений количества здоровья, соответствующий уровню героя</param>
    /// <param name="mana">Список значений количества маны, соответствующий уровню героя</param>
    /// <param name="experience">Список значений количества опыта, соответствующий уровню героя</param>
    /// <param name="damages">>Список значений возможного урона, соответствующий уровню героя </param>
    /// <param name="name">Имя персонажа</param>
    /// <param name="lvl">Уровень персонажа</param>
    /// <param name="expMultiplier">Множитель получаемого количества опыта за убийство</param>
    protected void InitializeProperties(List<int> health, List<int> mana, List<int> experience, List<int> damages, string name, int lvl = 1, float expMultiplier = 1)
    {
        base.InitializeProperties(health, mana, experience, damages, name, lvl);
        _expMultiplier = expMultiplier;


        agent.stoppingDistance += .5f;
        _attackRadius = agent.stoppingDistance + .5f;

        healthBar = GetComponentInChildren<SpriteRenderer>().gameObject;
    }

    protected virtual void Start()
    {
        player = Player.instance;
    }

    protected virtual void FixedUpdate()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);

        healthBar.transform.LookAt(Camera.main.transform);
        Move();
    }

    /// <summary>
    /// Обработчик события 
    /// </summary>
    public virtual void Hit()
    {
        transform.LookAt(Player.instance.transform);
        healthBar.transform.LookAt(Camera.main.transform);
    }


    /// <summary>
    /// Реализует перемещение врага к игроку.
    /// </summary>
    public virtual void Move()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < _agrRadius)
        {
            agent.SetDestination(player.transform.position);
            if (Vector3.Distance(player.transform.position, transform.position) < _attackRadius && timer > _attackRate)
            {
                timer = 0;
                Attack(player);
            }
            timer += Time.deltaTime;
        }
    }

    /// <summary>
    /// Обработчик смерти врага
    /// </summary>
    public override void Die()
    {
        player.Experience += _expMultiplier * Level;
        CanvasManager.Instance.UpdateHUD();
        GameManager.Instance.DeathCounter++;
        // Todo: Настроить 
        var inst = Instantiate(DieRagdoll, transform.position, transform.rotation);
        EnemyManager.Instance.ragdollPool.Add(inst);
        var ragdoll = inst.GetComponent<RagdollScript>();
        var force = transform.position - player.transform.position;
        force.Normalize();
        force.y += .5f;
        var forceMultiplier = Mathf.Clamp(Mathf.Log(player.Level * 1f / Level), .2f, 2);
        ragdoll.StartDeath(force * forceMultiplier, 3);

        base.Die();
    }

    /// <summary>
    /// Отображает в editor'е радиус атаки и агра противников 
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _agrRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRadius);
    }

    /// <summary>
    /// Обрабатывает получение урона.
    /// </summary>
    /// <param name="damage">количество урона</param>
    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
    }
}
