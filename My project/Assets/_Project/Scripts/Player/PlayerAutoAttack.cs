using System.Collections;
using UnityEngine;

/// <summary>
/// 일정 주기마다 탐지 범위 내에서 가장 가까운 Enemy를 찾아 Projectile을 발사한다.
/// 탐지는 Physics2D.OverlapCircleAll + "Enemy" 레이어 마스크로 수행한다.
/// </summary>
public class PlayerAutoAttack : MonoBehaviour
{
    [SerializeField] private float _attackInterval = 1f;
    [SerializeField] private float _detectionRadius = 15f;
    [SerializeField] private int _damage = 10;
    [SerializeField] private string _projectilePoolKey = "Projectile";

    private int _enemyLayerMask;

    private void Awake()
    {
        _enemyLayerMask = LayerMask.GetMask("Enemy");
    }

    private void Start()
    {
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_attackInterval);
            TryAttack();
        }
    }

    private void TryAttack()
    {
        EnemyController nearest = FindNearestEnemy();
        if (nearest == null) return;

        Vector2 direction = ((Vector2)nearest.transform.position - (Vector2)transform.position).normalized;

        GameObject projectileObj = ObjectPoolManager.Instance.Get(
            _projectilePoolKey, transform.position, Quaternion.identity);

        if (projectileObj == null) return;

        if (projectileObj.TryGetComponent<ProjectileController>(out var projectile))
            projectile.Initialize(direction, _damage);
    }

    private EnemyController FindNearestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _detectionRadius, _enemyLayerMask);

        EnemyController nearest = null;
        float minDist = float.MaxValue;

        foreach (var hit in hits)
        {
            if (!hit.TryGetComponent<EnemyController>(out var enemy)) continue;
            float dist = Vector2.SqrMagnitude((Vector2)transform.position - (Vector2)enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy;
            }
        }

        return nearest;
    }
}
