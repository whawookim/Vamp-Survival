using UnityEngine;

/// <summary>
/// Player에 부착된 WeaponBase 컴포넌트들을 순회하며 쿨다운을 관리하고 발동한다.
/// Enemy 탐색은 Physics2D.OverlapCircleAll + "Enemy" 레이어 마스크로 수행한다.
///
/// 사용법:
///   1. Player GameObject에 PlayerWeaponController 추가
///   2. Player(또는 그 자식)에 ProjectileWeapon 추가
///   3. (선택) PlayerAutoAttack 컴포넌트 제거
///
/// Phase 3 확장: 무기 추가/제거를 런타임에 처리하려면
///   AddWeapon(WeaponBase) / RemoveWeapon(WeaponBase) API 구현.
/// </summary>
public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private float _detectionRadius = 15f;

    private WeaponBase[] _weapons;
    private float[]      _cooldowns;
    private int          _enemyLayerMask;

    private void Awake()
    {
        _enemyLayerMask = LayerMask.GetMask("Enemy");
        RefreshWeapons();
    }

    private void Update()
    {
        if (_weapons == null || _weapons.Length == 0) return;

        EnemyController target = FindNearestEnemy();
        if (target == null) return;

        for (int i = 0; i < _weapons.Length; i++)
        {
            _cooldowns[i] -= Time.deltaTime;
            if (_cooldowns[i] > 0f) continue;

            _weapons[i].Fire(transform, target);
            _cooldowns[i] = _weapons[i].AttackInterval;
        }
    }

    /// <summary>GetComponentsInChildren으로 무기 목록을 갱신한다.</summary>
    private void RefreshWeapons()
    {
        _weapons   = GetComponentsInChildren<WeaponBase>();
        _cooldowns = new float[_weapons.Length];
        // 쿨다운을 0으로 초기화해 첫 프레임에 즉시 발사 가능하게 한다
    }

    private EnemyController FindNearestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position, _detectionRadius, _enemyLayerMask);

        EnemyController nearest = null;
        float minSqrDist = float.MaxValue;

        foreach (var hit in hits)
        {
            if (!hit.TryGetComponent<EnemyController>(out var enemy)) continue;

            float sqrDist = Vector2.SqrMagnitude(
                (Vector2)transform.position - (Vector2)enemy.transform.position);

            if (sqrDist < minSqrDist)
            {
                minSqrDist = sqrDist;
                nearest    = enemy;
            }
        }

        return nearest;
    }
}
