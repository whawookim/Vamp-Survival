using UnityEngine;

/// <summary>
/// 적의 비주얼 표현을 추상화하는 인터페이스.
/// EnemyController는 이 인터페이스를 통해서만 뷰에 접근하므로
/// SpriteRenderer → Spine 전환 시 구현체 교체만으로 대응 가능하다.
/// </summary>
public interface IEnemyView
{
    /// <summary>풀에서 꺼내 활성화될 때 호출된다.</summary>
    void OnSpawn();

    /// <summary>피격 시 호출된다. normalizedHp는 0~1 범위의 현재 체력 비율이다.</summary>
    void OnTakeDamage(float normalizedHp);

    /// <summary>사망 시 호출된다.</summary>
    void OnDeath();

    /// <summary>이동 방향을 전달해 스프라이트 방향 전환 등에 사용한다.</summary>
    void SetMoveDirection(Vector2 direction);
}
