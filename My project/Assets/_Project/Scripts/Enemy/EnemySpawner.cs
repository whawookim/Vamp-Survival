using System.Collections;
using UnityEngine;

/// <summary>
/// 일정 주기로 플레이어 주변 바깥쪽 원형 범위에 Enemy를 스폰한다.
/// Phase 2: WaveSystem의 CurrentPhase에 따라 EnemyType 비율을 적용한다.
///
/// Phase별 스폰 비율:
///   Phase 1: Normal 100%
///   Phase 2: Normal 70%  / Fast 30%
///   Phase 3: Normal 50%  / Fast 30% / Tank 20%
///   Phase 4: Normal 45%  / Fast 30% / Tank 20% / Elite 5%
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval     = 2f;
    [SerializeField] private float _spawnRadius       = 12f;
    [SerializeField] private int   _spawnCountPerWave = 3;
    [SerializeField] private int   _maxEnemyCount     = 50;
    [SerializeField] private string _enemyPoolKey     = "Enemy";

    private Transform _playerTransform;
    private int _enemyLayerMask;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            _playerTransform = player.transform;

        _enemyLayerMask = LayerMask.GetMask("Enemy");
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

        // 현재 활성 Enemy 수가 최대치를 넘으면 스킵
        int activeCount = Physics2D.OverlapCircleNonAlloc(
            _playerTransform.position, _spawnRadius * 3f,
            new Collider2D[_maxEnemyCount], _enemyLayerMask);

        if (activeCount >= _maxEnemyCount) return;

        for (int i = 0; i < _spawnCountPerWave; i++)
        {
            float angle    = Random.Range(0f, Mathf.PI * 2f);
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * _spawnRadius;
            Vector3 spawnPos = _playerTransform.position + (Vector3)offset;

            GameObject obj = ObjectPoolManager.Instance.Get(_enemyPoolKey, spawnPos, Quaternion.identity);
            if (obj == null) continue;

            EnemyType type = GetRandomEnemyType();
            EnemyStats stats = EnemyStatDatabase.Get(type);

            if (obj.TryGetComponent<EnemyController>(out var enemy))
                enemy.Initialize(stats);
        }
    }

    /// <summary>WaveSystem의 현재 Phase에 따라 가중 랜덤으로 EnemyType을 결정한다.</summary>
    private EnemyType GetRandomEnemyType()
    {
        int phase = WaveSystem.Instance != null ? WaveSystem.Instance.CurrentPhase : 1;
        float r = Random.value;

        switch (phase)
        {
            case 1:
                return EnemyType.Normal;

            case 2:
                // Normal 70% / Fast 30%
                return r < 0.70f ? EnemyType.Normal : EnemyType.Fast;

            case 3:
                // Normal 50% / Fast 30% / Tank 20%
                if (r < 0.50f) return EnemyType.Normal;
                if (r < 0.80f) return EnemyType.Fast;
                return EnemyType.Tank;

            default: // Phase 4+
                // Normal 45% / Fast 30% / Tank 20% / Elite 5%
                if (r < 0.45f) return EnemyType.Normal;
                if (r < 0.75f) return EnemyType.Fast;
                if (r < 0.95f) return EnemyType.Tank;
                return EnemyType.Elite;
        }
    }
}
