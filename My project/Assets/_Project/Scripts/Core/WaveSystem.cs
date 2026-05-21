using UnityEngine;

/// <summary>
/// 경과 시간에 따라 현재 Phase를 관리하는 싱글톤.
/// Phase 전환 시 OnPhaseChanged 이벤트를 발행해 EnemySpawner와 PhaseUI에 알린다.
///
/// 기본 Phase 구성 (Inspector에서 수정 가능):
///   Phase 1:   0 ~  30초   Normal Enemy
///   Phase 2:  30 ~  60초   Normal + Fast
///   Phase 3:  60 ~ 120초   Normal + Fast + Tank
///   Phase 4: 120초 이후    Normal + Fast + Tank + Elite
/// </summary>
public class WaveSystem : MonoBehaviour
{
    public static WaveSystem Instance { get; private set; }

    /// <summary>
    /// 각 Phase의 시작 시간(초). 인덱스 0 = Phase 1 시작(0초), 인덱스 1 = Phase 2 시작 ...
    /// 길이가 Phase 수와 같아야 한다.
    /// </summary>
    [SerializeField] private float[] _phaseStartTimes = { 0f, 30f, 60f, 120f };

    /// <summary>현재 활성 Phase (1-based).</summary>
    public int CurrentPhase { get; private set; } = 1;

    /// <summary>Phase가 바뀔 때 새 Phase 번호를 인자로 발행한다.</summary>
    public event System.Action<int> OnPhaseChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (GameTimer.Instance == null) return;

        int newPhase = CalculatePhase(GameTimer.Instance.ElapsedTime);
        if (newPhase == CurrentPhase) return;

        CurrentPhase = newPhase;
        Debug.Log($"[WaveSystem] Phase {CurrentPhase} 시작 (경과 {GameTimer.Instance.ElapsedTime:F0}초)");
        OnPhaseChanged?.Invoke(CurrentPhase);
    }

    /// <summary>경과 시간으로 현재 Phase 번호를 계산한다 (1-based).</summary>
    private int CalculatePhase(float elapsed)
    {
        for (int i = _phaseStartTimes.Length - 1; i >= 0; i--)
        {
            if (elapsed >= _phaseStartTimes[i])
                return i + 1;
        }

        return 1;
    }
}
