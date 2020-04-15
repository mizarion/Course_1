using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Абстрактный класс отвечающий за базовый функционал существа.
/// </summary>
[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(Animator))/*, RequireComponent(typeof(Rigidbody))*/]
public abstract class AbstractCharacter : MonoBehaviour, ICharacter
{
    protected NavMeshAgent agent;
    protected Animator animator;
    protected Rigidbody rbody;
    [SerializeField] ScrollingText _scrollingText;
    [SerializeField] Color _scrollingTextColor;
    Transform _scrollingTextContainer;

    ///// <summary>
    ///// Конструктор для создания абстрактного персонажа
    ///// </summary>
    ///// <param name="health">Список значений количества здоровья, соответствующий уровню героя</param>
    ///// <param name="mana">Список значений количества маны, соответствующий уровню героя</param>
    ///// <param name="experience">Список значений количества опыта, соответствующий уровню героя</param>
    ///// <param name="name">Имя персонажа</param>
    ///// <param name="lvl">Уровень персонажа</param>
    //public AbstractCharacter(List<int> health, List<int> mana, List<int> experience, string name, int lvl = 1)
    //{
    //    _listHp = health;
    //    _listMp = mana;
    //    _listExp = experience;
    //    Level = lvl;
    //    Health = health[Level];
    //    Manapool = mana[Level];
    //    Name = name;
    //}

    /// <summary>
    /// Метод (Конструктор) для создания абстрактного персонажа.
    /// </summary>
    /// <param name="health">Список значений количества здоровья, соответствующий уровню героя</param>
    /// <param name="mana">Список значений количества маны, соответствующий уровню героя</param>
    /// <param name="experience">Список значений количества опыта, соответствующий уровню героя</param>
    /// <param name="name">Имя персонажа</param>
    /// <param name="lvl">Уровень персонажа</param>
    protected void InitializeProperties(List<int> health, List<int> mana, List<int> experience, string name, int lvl = 1)
    {
        _listHp = health;
        _listMp = mana;
        _listExp = experience;
        Level = lvl;
        Health = health[Level];
        Manapool = mana[Level];
        Name = name;

        animator = GetComponent<Animator>();
        //rbody = GetComponent<Rigidbody>();

        _scrollingTextContainer = GameObject.Find("TextContainer").transform;

        // инициализация NavMeshAgent 
        agent = GetComponent<NavMeshAgent>();
        agent.acceleration = 20;
        agent.angularSpeed = 800;
        agent.autoBraking = false;
        agent.stoppingDistance = 1f;
    }

    protected List<int> _listHp;
    protected List<int> _listMp;
    protected List<int> _listExp;
    public string Name { get; set; }

    /// <summary>
    /// Свойство отвечающее за здоровье.
    /// </summary>


    [Header("Values"), Space]
    [SerializeField] float _health;
    [SerializeField] float _mana;
    [SerializeField] int _lvl;
    [SerializeField] float _exp;
    [SerializeField] private float damage = 10;

    /// <summary>
    /// Свойство отвечающее за здоровье
    /// </summary>
    public float Health
    {
        get => _health;
        set
        {
            _health = Mathf.Clamp(value, -1, _listHp[Level]);
            if (_health <= 0) Die();
        }
    }

    /// <summary>
    /// Свойство отвечающее за ману.
    /// </summary>
    public float Manapool
    {
        get => _mana;
        set => _mana = Mathf.Clamp(value, 0, _listMp[Level]);
    }

    /// <summary>
    /// Свойство отвечающее за уровень.
    /// </summary>
    public int Level
    {
        get => _lvl;
        set => _lvl = Mathf.Clamp(value, 1, _listExp.Count - 1);
    }

    /// <summary>
    /// Свойство отвечающее за опыт.
    /// </summary>
    public float Experience
    {
        get => _exp;
        set
        {
            while (value >= _listExp[Level])
            {
                value -= _listExp[Level];
                Level++;
            }
            _exp = value;
        }
    }

    /// <summary>
    /// Свойство отвечающее за урон
    /// </summary>
    public float Damage
    {
        get => damage;
        protected set => (damage) = value;
    }

    /// <summary>
    /// Свойство отвечающее за скорость перемещения
    /// </summary>
    public float Movespeed { get; protected set; }

    /// <summary>
    /// Атакует выбранную цель, нанося damage урона.
    /// </summary>
    /// <param name="character">Цель атаки</param>
    public virtual void Attack(ICharacter character)
    {
        character.GetDamage(Random.Range(Damage / 2, Damage));

        animator.SetTrigger("Attack");
    }

    public virtual void Hit()
    {

    }

    /// <summary>
    /// Обрабатывает смерть персонажа
    /// </summary>
    public virtual void Die()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Получиет урон в размере damage.
    /// </summary>
    /// <param name="damage">Получаемый урон</param>
    public virtual void GetDamage(float damage)
    {
        Health -= damage;
        var text = Instantiate(_scrollingText, transform.position, Quaternion.identity, _scrollingTextContainer);
        text.SetTextAndColor(((int)damage).ToString(), _scrollingTextColor);
    }

    /// <summary>
    /// Восполняет здоровье и ману персонажа.
    /// </summary>
    /// <param name="healRecovery">Скорость восстановления здоровья</param>
    /// <param name="manaRecovery">Скорость восстановления маны</param>
    /// <param name="delay">Задержка между лечениями</param>
    /// <returns></returns>
    public virtual IEnumerator Recovery(float healRecovery = 5, float manaRecovery = 5, float delay = 1)
    {
        while (true)
        {
            Health += healRecovery;
            Manapool += manaRecovery;
            yield return new WaitForSeconds(delay);
        }
    }
}
