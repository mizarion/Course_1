using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    #region Object Pooling 
    [SerializeField] GameObject enemySample;
    [SerializeField] int poolStartLength = 20;

    [SerializeField, DataMember] List<GameObject> _pool = new List<GameObject>();
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

}
