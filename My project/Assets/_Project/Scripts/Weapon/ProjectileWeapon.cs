using UnityEngine;

/// <summary>
/// 투사체를 발사하는 WeaponBase 구현체.
/// ObjectPool에서 Projectile을 꺼내 target 방향으로 초기화한다.
/// PlayerAutoAttack을 대체하는 Phase 2 표준 무기.
/// </summary>
public class ProjectileWeapon : WeaponBase
{
    [SerializeField] private string _projectilePoolKey = "Projectile";

    /// <summary>target 방향으로 Projectile을 발사한다.</summary>
    public override void Fire(Transform origin, EnemyController target)
    {
        if (target == null) return;

        Vector2 direction = ((Vector2)target.transform.position - (Vector2)origin.position).normalized;

        GameObject obj = ObjectPoolManager.Instance.Get(
            _projectilePoolKey, origin.position, Quaternion.identity);

        if (obj == null) return;

        if (obj.TryGetComponent<ProjectileController>(out var projectile))
            projectile.Initialize(direction, _damage);
    }
}
