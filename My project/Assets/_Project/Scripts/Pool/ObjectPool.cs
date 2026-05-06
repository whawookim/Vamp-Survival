using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 단일 프리팹에 대한 오브젝트 풀. Instantiate/Destroy 반복을 방지한다.
/// </summary>
public class ObjectPool
{
    private readonly GameObject _prefab;
    private readonly Transform _parent;
    private readonly Queue<GameObject> _pool = new Queue<GameObject>();

    public ObjectPool(GameObject prefab, int initialSize, Transform parent)
    {
        _prefab = prefab;
        _parent = parent;
        for (int i = 0; i < initialSize; i++)
            Enqueue(CreateNew());
    }

    /// <summary>풀에서 오브젝트를 꺼내 활성화한다.</summary>
    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject obj = _pool.Count > 0 ? _pool.Dequeue() : CreateNew();
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);
        return obj;
    }

    /// <summary>오브젝트를 비활성화하고 풀에 반환한다.</summary>
    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        Enqueue(obj);
    }

    private GameObject CreateNew()
    {
        GameObject obj = Object.Instantiate(_prefab, _parent);
        obj.SetActive(false);
        return obj;
    }

    private void Enqueue(GameObject obj) => _pool.Enqueue(obj);
}
