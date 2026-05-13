using UnityEngine;

/// <summary>
/// 플레이어 HP를 관리한다.
/// EnemyController의 접촉 데미지, 향후 함정/스킬 등에서 TakeDamage()를 호출한다.
/// Phase 3에서 무적 프레임, 방어력 계산, 사망 연출을 추가한다.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _maxHp = 100;

    public int MaxHp     => _maxHp;
    public int CurrentHp { get; private set; }

    /// <summary>HP가 변경될 때 (currentHp, maxHp)를 인자로 발행한다.</summary>
    public event System.Action<int, int> OnHealthChanged;

    /// <summary>HP가 0 이하가 됐을 때 발행한다.</summary>
    public event System.Action OnDeath;

    private void Awake()
    {
        CurrentHp = _maxHp;
    }

    /// <summary>데미지를 적용한다. 이미 사망한 경우 무시한다.</summary>
    public void TakeDamage(int amount)
    {
        if (CurrentHp <= 0) return;

        CurrentHp = Mathf.Max(0, CurrentHp - amount);
        OnHealthChanged?.Invoke(CurrentHp, _maxHp);
        Debug.Log($"[PlayerHealth] HP {CurrentHp} / {_maxHp}");

        if (CurrentHp <= 0)
        {
            Debug.Log("[PlayerHealth] 플레이어 사망");
            OnDeath?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        if (CurrentHp <= 0) return;
        CurrentHp = Mathf.Min(_maxHp, CurrentHp + amount);
        OnHealthChanged?.Invoke(CurrentHp, _maxHp);
    }
}
