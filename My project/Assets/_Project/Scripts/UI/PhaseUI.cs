using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// WaveSystem의 현재 Phase 번호를 Text에 표시한다.
/// OnPhaseChanged 이벤트로 WaveSystem과 디커플링되어 통신한다.
/// Text 슬롯이 비어 있어도 NullReferenceException이 발생하지 않는다.
/// </summary>
public class PhaseUI : MonoBehaviour
{
    [SerializeField] private Text _phaseText;

    private void Start()
    {
        if (WaveSystem.Instance != null)
            WaveSystem.Instance.OnPhaseChanged += UpdatePhaseText;

        // 초기 Phase 표시
        int initial = WaveSystem.Instance != null ? WaveSystem.Instance.CurrentPhase : 1;
        UpdatePhaseText(initial);
    }

    private void OnDestroy()
    {
        if (WaveSystem.Instance != null)
            WaveSystem.Instance.OnPhaseChanged -= UpdatePhaseText;
    }

    private void UpdatePhaseText(int phase)
    {
        if (_phaseText == null) return;
        _phaseText.text = $"Phase {phase}";
    }
}
