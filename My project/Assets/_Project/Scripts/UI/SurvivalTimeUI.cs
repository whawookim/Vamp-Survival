using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GameTimer의 경과 시간을 MM:SS 포맷으로 Text에 표시한다.
/// Text 슬롯이 비어 있어도 NullReferenceException이 발생하지 않는다.
/// </summary>
public class SurvivalTimeUI : MonoBehaviour
{
    [SerializeField] private Text _timeText;

    private void Update()
    {
        if (_timeText == null || GameTimer.Instance == null) return;

        float elapsed  = GameTimer.Instance.ElapsedTime;
        int   minutes  = (int)(elapsed / 60f);
        int   seconds  = (int)(elapsed % 60f);
        _timeText.text = $"{minutes:00}:{seconds:00}";
    }
}
