using UnityEngine;

/// <summary>
/// 무기의 공통 인터페이스.
/// WeaponBase(MonoBehaviour 기반)와 비-MonoBehaviour 무기 모두 지원하기 위해 분리한다.
/// Phase 3에서 OrbitWeapon, AreaPulseWeapon 추가 시 이 인터페이스를 구현한다.
/// </summary>
public interface IWeapon
{
    /// <summary>공격 쿨다운 (초). PlayerWeaponController가 타이밍 관리에 사용한다.</summary>
    float AttackInterval { get; }

    /// <summary>무기를 발동한다. origin은 발사 위치, target은 공격 대상이다.</summary>
    void Fire(Transform origin, EnemyController target);

    /// <summary>데미지 값을 외부에서 덮어쓴다 (버프, 업그레이드 등).</summary>
    void SetDamage(int damage);
}
