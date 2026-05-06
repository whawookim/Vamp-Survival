using UnityEngine;

/// <summary>
/// 경험치 수집, 레벨 계산, 레벨업 이벤트를 관리하는 싱글톤.
/// UI(ExperienceUI)와 디커플링되어 이벤트로 통신한다.
/// </summary>
public class ExperienceSystem : MonoBehaviour
{
    public static ExperienceSystem Instance { get; private set; }

    [SerializeField] private int _expPerLevel = 100;

    public int CurrentLevel { get; private set; } = 1;
    public int CurrentExp { get; private set; }
    public int RequiredExp => CurrentLevel * _expPerLevel;

    /// <summary>레벨업 시 새 레벨을 인자로 전달한다.</summary>
    public event System.Action<int> OnLevelUp;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>경험치를 추가하고 레벨업 조건을 반복 체크한다.</summary>
    public void AddExperience(int amount)
    {
        CurrentExp += amount;

        while (CurrentExp >= RequiredExp)
        {
            CurrentExp -= RequiredExp;
            CurrentLevel++;
            Debug.Log($"[ExperienceSystem] Level Up! → Lv.{CurrentLevel}");
            OnLevelUp?.Invoke(CurrentLevel);
        }
    }
}
