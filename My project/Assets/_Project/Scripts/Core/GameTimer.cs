using UnityEngine;

/// <summary>
/// 게임 시작 후 경과 시간을 관리하는 싱글톤.
/// GameManager.IsPlaying이 false면 타이머를 멈춘다.
/// WaveSystem과 SurvivalTimeUI가 이 값을 참조한다.
/// </summary>
public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    /// <summary>게임 시작 후 경과 시간 (초).</summary>
    public float ElapsedTime { get; private set; }

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
        if (GameManager.Instance != null && !GameManager.Instance.IsPlaying) return;
        ElapsedTime += Time.deltaTime;
    }
}
