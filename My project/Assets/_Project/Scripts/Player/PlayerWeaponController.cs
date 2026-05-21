using UnityEngine;

/// <summary>
/// Player에 부착된 WeaponBase 컴포넌트들을 순회하며 쿨다운을 관리하고 발동한다.
/// Enemy 탐색은 재사용 Collider2D 버퍼를 통해 수행한다.
/// </summary>
public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private float _detectionRadius = 15f;
    [SerializeField] private int _targetBufferSize = 128;

    private WeaponBase[] _weapons;
    private float[] _cooldowns;
    private int _enemyLayerMask;
    private Collider2D[] _targetBuffer;
    private ContactFilter2D _enemyContactFilter;

    private void Awake()
    {
        _enemyLayerMask = LayerMask.GetMask("Enemy");
        _targetBuffer = new Collider2D[_targetBufferSize];
        _enemyContactFilter = new ContactFilter2D
        {
            useLayerMask = true,
            layerMask = _enemyLayerMask,
            useTriggers = true
        };

        RefreshWeapons();
    }

    private void Update()
    {
        if (_weapons == null || _weapons.Length == 0) return;

        AdvanceCooldowns();

        EnemyController target = FindNearestEnemy();
        if (target == null) return;

        for (int i = 0; i < _weapons.Length; i++)
        {
            if (_cooldowns[i] > 0f) continue;

            _weapons[i].Fire(transform, target);
            _cooldowns[i] = _weapons[i].AttackInterval;
        }
    }

    /// <summary>모든 무기의 쿨다운 시간을 진행시킨다.</summary>
    private void AdvanceCooldowns()
    {
        for (int i = 0; i < _cooldowns.Length; i++)
        {
            if (_cooldowns[i] <= 0f) continue;
            _cooldowns[i] -= Time.deltaTime;
        }
    }

    /// <summary>GetComponentsInChildren으로 무기 목록을 갱신한다.</summary>
    private void RefreshWeapons()
    {
        _weapons = GetComponentsInChildren<WeaponBase>();
        _cooldowns = new float[_weapons.Length];
    }

    private EnemyController FindNearestEnemy()
    {
        int hitCount = Physics2D.OverlapCircle(transform.position, _detectionRadius,
            _enemyContactFilter, _targetBuffer);

        EnemyController nearest = null;
        float minSqrDist = float.MaxValue;

        for (int i = 0; i < hitCount; i++)
        {
            Collider2D hit = _targetBuffer[i];
            if (hit == null) continue;
            if (!hit.TryGetComponent<EnemyController>(out var enemy)) continue;

            float sqrDist = Vector2.SqrMagnitude(
                (Vector2)transform.position - (Vector2)enemy.transform.position);

            if (sqrDist < minSqrDist)
            {
                minSqrDist = sqrDist;
                nearest = enemy;
            }
        }

        return nearest;
    }
}
