using UnityEngine;

/// <summary>
/// 게임 전체 상태(플레이 중 / 종료 등)를 관리하는 싱글톤.
/// Phase 2에서 ScriptableObject 기반 게임 설정으로 확장 예정.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsPlaying { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        IsPlaying = true;
    }
}
