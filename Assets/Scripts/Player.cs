using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Класс, реализующий игрока
/// </summary>
[System.Serializable]
public class Player : AbstractCharacter
{
    [HideInInspector] public static Player instance;
    bool isAttacking;
    GameObject attackTarget;

    private void Awake()
    {
        InitializeProperties(DataManager.Stats.Player.Health, DataManager.Stats.Player.Manapool, DataManager.Stats.Player.Experience, DataManager.Stats.Player.Damage, "Hero");

        instance = this;

        StartCoroutine(Recovery(5, 5));
    }


    private void Update()
    {
        animator.SetFloat("Speed", agent.velocity.magnitude);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //int damage = 10;
            //GetDamage(damage);
            Debug.Log($"health:{Health}");
            Debug.Log($"Exp: {Experience += 10}");
        }
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

        agent.isStopped = true;

        // Todo: Донастроить

        transform.LookAt(attackTarget.transform);
        //animator.SetTrigger("Attack");
        Attack(attackTarget.GetComponent<ICharacter>());
        StopCoroutine(AttackTarget());
    }

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
    /// <param name="healRecovery">Скорость восстановления здоровья</param>
    /// <param name="manaRecovery">Скорость восстановления маны</param>
    /// <param name="delay">Задержка между лечениями</param>
    /// <returns></returns>
    public override IEnumerator Recovery(float healRecovery = 5, float manaRecovery = 5, float delay = 1)
    {
        while (true)
        {
            Health += healRecovery;
            Manapool += manaRecovery;
            CanvasManager.Instance.UpdateHUD();
            yield return new WaitForSeconds(delay);
        }
    }


    public override void Die(/*float delay = 0*/)
    {
        // ToDo: обработать смерть героя

        CanvasManager.Instance.DeathHandler();

        Debug.Log($"[Player] Hero: {Name} die");
        //base.Die();
    }

    #region Legacy

    //void FixedUpdate()
    //{
    //    Move();
    //}

    //private void LateUpdate()
    //{
    //    RotateCamera();
    //}

    ///// <summary>
    ///// Отвечает за движение персонажа
    ///// </summary>
    //void Move()
    //{
    //    Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
    //    transform.Translate(direction * Time.fixedDeltaTime * _movespeed);

    //    //if (Input.GetMouseButton(1))
    //    {
    //        transform.localEulerAngles += Vector3.up * Input.GetAxis("Mouse X") * _cameraMoveSens;
    //    }

    //}

    ///// <summary>
    ///// Отвечает за поворот камеры
    ///// </summary>
    //void RotateCamera()
    //{
    //    //Vector3 direction = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
    //    Vector3 direction = new Vector3(-Input.GetAxis("Mouse Y"), 0, 0);
    //    _camera.transform.localEulerAngles += direction;// * Time.deltaTime;
    //}

    ///// <summary>
    ///// Обрабатывает нажатие левой клавишы мыши на цели
    ///// </summary>
    //void TakeTarget()
    //{
    //    Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
    //    Debug.DrawRay(transform.position, ray.direction * 20);
    //    RaycastHit hit;
    //    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
    //    {
    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            Debug.Log(hit.collider.name);
    //        }
    //    }
    //}

    #endregion
}
