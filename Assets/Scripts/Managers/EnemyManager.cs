using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

/// <summary>
/// Класс, реализующий работу с врагами.
/// </summary>
public class EnemyManager : Singleton<EnemyManager>
{
    #region Object Pooling 
#pragma warning disable 649

    [SerializeField] GameObject enemySample;        // Образец создоваемых врагов
    [SerializeField] Transform[] _spawnPositions;   // Позиции для создания противников
    [SerializeField] int poolStartLength = 20;      // Минимальное количество противников в массиве. Для Object Pooling 
    [SerializeField] float _spawnRadius = 50;       // Минимальное расстояние между героем и созданным противником.
    List<AbstractEnemy> _abstractEnemiesPool = new List<AbstractEnemy>();       // Список противников приведенных к AbstractEnemy

    public List<GameObject> ragdollPool = new List<GameObject>();               // Список ragdoll’ов, которые нужно удалить при загрузке сохранения.

    [SerializeField, DataMember] List<GameObject> _pool = new List<GameObject>(); // Список GameObject противников.

#pragma warning restore 649

    /// <summary>
    /// Свойство, находящее не используемого противника из пула, активирует и возвращает его.
    /// </summary>
    public GameObject GetEnemy      
    {
        get
        {
            foreach (var item in _pool)
            {
                if (!item.activeSelf)
                {
                    item.SetActive(true);
                    return item;
                }
            }
            // Если элемента не нашлось
            var newEnemy = Instantiate(enemySample, transform);
            _pool.Add(newEnemy);
            _abstractEnemiesPool.Add(newEnemy.GetComponent<AbstractEnemy>());

            return _pool[_pool.Count - 1];
        }
    }

    /// <summary>
    /// Свойство, возвращающее ссылку на пул GameObject’ов.
    /// </summary>
    public List<GameObject> GetPool => _pool;

    /// <summary>
    /// Свойство, находящее используемых противников из пула, добавляет их в список и возвращает его.
    /// </summary>
    public List<AbstractEnemy> GetActiveAbstractEnemyPool
    {
        get
        {
            List<AbstractEnemy> list = new List<AbstractEnemy>();
            foreach (var item in GetAbstractEnemyPool)
            {
                if (item.gameObject.activeSelf)
                {
                    list.Add(item);
                }
            }
            return list;
        }
    }

    /// <summary>
    /// Свойство, возвращающее список противников приведенных к AbstractEnemy.
    /// </summary>
    public List<AbstractEnemy> GetAbstractEnemyPool => _abstractEnemiesPool;


    #endregion

    protected override void Awake()
    {
        base.Awake();
        for (int i = _pool.Count - 1; i < poolStartLength; i++)
        {
            var inst = Instantiate(enemySample, transform);
            inst.SetActive(false);
            _pool.Add(inst);
        }
        foreach (var enemy in GetPool)
        {
            _abstractEnemiesPool.Add(enemy.GetComponent<AbstractEnemy>());
        }
    }

    /// <summary>
    /// Удаляет все существующие ragdoll'ы со сцены
    /// </summary>
    public void DestroyAllRagdolls()
    {
        foreach (var item in ragdollPool)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }
    }

    /// <summary>
    /// Начинает новую "волну" атаки противников на героя
    /// </summary>
    /// <param name="count">Количество врагов</param>
    /// <param name="enemyLevel">Уровень врагов</param>
    public void StartNewWawe(int count, int enemyLevel = 4)
    {
        StartCoroutine(SpawnNewEnemies(count, enemyLevel));
    }

    /// <summary>
    /// Корутина для создания новой "волны" атаки противников на героя
    /// </summary>
    /// <param name="count">Количество врагов</param>
    /// <param name="enemyLevel">Уровень врагов</param>
    /// <returns></returns>
    IEnumerator SpawnNewEnemies(int count, int enemyLevel = 4)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(Random.Range(1.5f, 4));
            var AbsEnemy = GetEnemy.GetComponent<AbstractEnemy>();
            AbsEnemy.agent.SetDestination(Player.instance.transform.position);
            AbsEnemy.Level = enemyLevel;
            Vector3 position;
            do
            {
                position = _spawnPositions[Random.Range(0, _spawnPositions.Length)].position;
            } while (Vector3.Distance(Player.instance.transform.position, position) < _spawnRadius);
            AbsEnemy.transform.position = position;
        }
    }
}
