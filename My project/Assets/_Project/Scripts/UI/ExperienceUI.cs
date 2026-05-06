using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ExperienceSystem의 레벨/경험치를 UI에 표시한다.
/// 이벤트 구독으로 ExperienceSystem과 디커플링한다.
/// Phase 2에서 TextMeshPro 및 레벨업 연출로 교체 예정.
/// </summary>
public class ExperienceUI : MonoBehaviour
{
    [SerializeField] private Text _levelText;
    [SerializeField] private Slider _expSlider;

    private ExperienceSystem _expSystem;

    private void Start()
    {
        _expSystem = ExperienceSystem.Instance;
        if (_expSystem == null)
        {
            Debug.LogWarning("[ExperienceUI] ExperienceSystem을 찾을 수 없습니다.");
            return;
        }

        _expSystem.OnLevelUp += HandleLevelUp;
        Refresh();
    }

    private void OnDestroy()
    {
        if (_expSystem != null)
            _expSystem.OnLevelUp -= HandleLevelUp;
    }

    private void Update()
    {
        if (_expSystem == null) return;
        // 경험치 슬라이더는 매 프레임 갱신한다 (젬 획득 시 즉시 반영)
        RefreshSlider();
    }

    private void HandleLevelUp(int newLevel)
    {
        RefreshLevelText();
    }

    private void Refresh()
    {
        RefreshLevelText();
        RefreshSlider();
    }

    private void RefreshLevelText()
    {
        if (_levelText != null)
            _levelText.text = $"Lv. {_expSystem.CurrentLevel}";
    }

    private void RefreshSlider()
    {
        if (_expSlider != null && _expSystem.RequiredExp > 0)
            _expSlider.value = (float)_expSystem.CurrentExp / _expSystem.RequiredExp;
    }
}
