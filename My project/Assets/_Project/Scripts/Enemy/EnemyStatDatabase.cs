using UnityEngine;

/// <summary>
/// EnemyType별 기본 스탯을 코드 기반으로 제공하는 정적 데이터베이스.
/// Phase 3에서 ScriptableObject 또는 JSON으로 전환 예정.
/// Profiler Before 기준점: 스탯 조회는 new 객체 생성으로 GC Alloc 발생.
/// 전환 시 struct 또는 캐싱으로 개선 가능.
/// </summary>
public static class EnemyStatDatabase
{
    /// <summary>EnemyType에 해당하는 EnemyStats를 반환한다.</summary>
    public static EnemyStats Get(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Normal:
                return new EnemyStats
                {
                    Type         = EnemyType.Normal,
                    MaxHp        = 30,
                    MoveSpeed    = 2.5f,
                    ContactDamage = 5,
                    ExpReward    = 10,
                    Scale        = 1.0f,
                    Color        = new Color(0.85f, 0.40f, 0.40f)   // 붉은 회색
                };

            case EnemyType.Fast:
                return new EnemyStats
                {
                    Type         = EnemyType.Fast,
                    MaxHp        = 15,
                    MoveSpeed    = 4.5f,
                    ContactDamage = 3,
                    ExpReward    = 15,
                    Scale        = 0.75f,
                    Color        = new Color(0.40f, 0.85f, 0.85f)   // 청록
                };

            case EnemyType.Tank:
                return new EnemyStats
                {
                    Type         = EnemyType.Tank,
                    MaxHp        = 100,
                    MoveSpeed    = 1.2f,
                    ContactDamage = 15,
                    ExpReward    = 30,
                    Scale        = 1.5f,
                    Color        = new Color(0.40f, 0.40f, 0.85f)   // 남색
                };

            case EnemyType.Elite:
                return new EnemyStats
                {
                    Type         = EnemyType.Elite,
                    MaxHp        = 200,
                    MoveSpeed    = 2.0f,
                    ContactDamage = 20,
                    ExpReward    = 60,
                    Scale        = 2.0f,
                    Color        = new Color(0.80f, 0.20f, 0.85f)   // 보라
                };

            default:
                return Get(EnemyType.Normal);
        }
    }
}
