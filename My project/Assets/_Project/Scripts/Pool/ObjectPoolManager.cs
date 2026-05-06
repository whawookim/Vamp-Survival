using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 프리팹별 ObjectPool을 등록하고 관리하는 싱글톤.
/// Inspector에서 PoolEntry 목록을 등록하면 Awake 시점에 풀을 생성한다.
/// </summary>
public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    [System.Serializable]
    public class PoolEntry
    {
        public string key;
        public GameObject prefab;
        public int initialSize = 10;
    }

    [SerializeField] private PoolEntry[] _poolEntries;

    private readonly Dictionary<string, ObjectPool> _pools = new Dictionary<string, ObjectPool>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        foreach (var entry in _poolEntries)
        {
            if (entry.prefab == null)
            {
                Debug.LogError($"[ObjectPoolManager] Pool '{entry.key}'의 prefab이 비어 있습니다.");
                continue;
            }

            var poolParent = new GameObject($"Pool_{entry.key}").transform;
            poolParent.SetParent(transform);
            _pools[entry.key] = new ObjectPool(entry.prefab, entry.initialSize, poolParent);
        }
    }

    /// <summary>키에 해당하는 풀에서 오브젝트를 꺼낸다.</summary>
    public GameObject Get(string key, Vector3 position, Quaternion rotation)
    {
        if (!_pools.TryGetValue(key, out var pool))
        {
            Debug.LogError($"[ObjectPoolManager] '{key}' 풀을 찾을 수 없습니다.");
            return null;
        }

        return pool.Get(position, rotation);
    }

    /// <summary>키에 해당하는 풀로 오브젝트를 반환한다.</summary>
    public void Return(string key, GameObject obj)
    {
        if (!_pools.TryGetValue(key, out var pool))
        {
            Debug.LogError($"[ObjectPoolManager] '{key}' 풀을 찾을 수 없습니다.");
            return;
        }

        pool.Return(obj);
    }
}
