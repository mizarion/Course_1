using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    #region Object Pooling 
#pragma warning disable 649

    [SerializeField] GameObject enemySample;
    [SerializeField] Transform[] _spawnPositions;
    [SerializeField] int poolStartLength = 20;

    public List<GameObject> ragdollPool = new List<GameObject>();
    [SerializeField, DataMember] List<GameObject> _pool = new List<GameObject>();
#pragma warning restore 649

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
            _pool.Add(Instantiate(enemySample, transform));
            return _pool[_pool.Count - 1];
        }
    }

    public List<GameObject> GetPool
    {
        get => _pool;
        set => (_pool) = value;
    }



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
    }

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

    public void StartNewWawe(int count = 5)
    {
        StartCoroutine(SpawnNewEnemies(count));
    }

    IEnumerator SpawnNewEnemies(int count)
    {
        //for (int j = 0; j < count; j++)
        //{
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(Random.Range(.5f, 2));
            var AbsEnemy = GetEnemy.GetComponent<AbstractEnemy>();
            AbsEnemy.agent.SetDestination(Player.instance.transform.position);
            //AbsEnemy.Level = j;
            Vector3 position;
            do
            {
                position = _spawnPositions[Random.Range(0, _spawnPositions.Length)].position;
            } while (Vector3.Distance(Player.instance.transform.position, position) < 20);
            AbsEnemy.transform.position = position;
            //AbsEnemy.transform.localScale = Vector3.one * Mathf.Log(AbsEnemy.Level + 1);
        }
        //yield return new WaitForSeconds(2);
        //}

    }
}
