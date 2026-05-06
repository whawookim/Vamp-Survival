using System.Collections;
using UnityEngine;

/// <summary>
/// 일정 주기로 플레이어 주변 바깥쪽 원형 범위에 Enemy를 스폰한다.
/// Enemy는 ObjectPool에서 꺼내므로 Instantiate를 반복하지 않는다.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private float _spawnRadius = 12f;
    [SerializeField] private int _spawnCountPerWave = 3;
    [SerializeField] private string _enemyPoolKey = "Enemy";

    private Transform _playerTransform;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            _playerTransform = player.transform;

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnInterval);
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        if (_playerTransform == null) return;

        for (int i = 0; i < _spawnCountPerWave; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * _spawnRadius;
            Vector3 spawnPos = _playerTransform.position + (Vector3)offset;
            ObjectPoolManager.Instance.Get(_enemyPoolKey, spawnPos, Quaternion.identity);
        }
    }
}
