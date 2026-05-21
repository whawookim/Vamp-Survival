using UnityEngine;

/// <summary>
/// 적 한 종류의 스탯 데이터를 담는 데이터 클래스.
/// Phase 3에서 ScriptableObject로 전환할 것을 고려해 필드 구조를 미리 갖춰둔다.
/// </summary>
[System.Serializable]
public class EnemyStats
{
    /// <summary>적 종류 식별자.</summary>
    public EnemyType Type;

    /// <summary>최대 체력.</summary>
    public int MaxHp;

    /// <summary>이동 속도 (units/s).</summary>
    public float MoveSpeed;

    /// <summary>플레이어와 접촉 시 초당 가하는 데미지.</summary>
    public int ContactDamage;

    /// <summary>사망 시 드랍되는 경험치 보상.</summary>
    public int ExpReward;

    /// <summary>스프라이트 스케일 배율. Normal = 1f 기준.</summary>
    public float Scale;

    /// <summary>스프라이트 렌더링 색상. 타입별로 구분한다.</summary>
    public Color Color;
}
