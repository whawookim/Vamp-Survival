using UnityEngine;

/// <summary>
/// 모든 무기의 공통 기반 클래스 (MonoBehaviour + IWeapon).
/// Inspector에서 기본 수치를 조정하고, Fire()만 자식 클래스에서 구현한다.
///
/// 확장 예시:
///   ProjectileWeapon  : WeaponBase  → 현재 구현
///   OrbitWeapon       : WeaponBase  → Phase 3 예정
///   AreaPulseWeapon   : WeaponBase  → Phase 3 예정
/// </summary>
public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    [SerializeField] protected int _damage = 10;
    [SerializeField] protected float _attackInterval = 1f;

    public float AttackInterval => _attackInterval;

    /// <summary>무기를 발동한다. 구체적인 로직은 자식 클래스에서 구현한다.</summary>
    public abstract void Fire(Transform origin, EnemyController target);

    public void SetDamage(int damage) => _damage = damage;
    public void SetAttackInterval(float value) => _attackInterval = value;
}
