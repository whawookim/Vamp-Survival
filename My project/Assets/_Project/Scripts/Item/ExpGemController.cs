using UnityEngine;

/// <summary>
/// 경험치 젬. Player와 충돌 시 ExperienceSystem에 경험치를 전달하고 풀에 반환한다.
/// CircleCollider2D(IsTrigger=true) + Rigidbody2D(Kinematic)으로 구성한다.
/// </summary>
public class ExpGemController : MonoBehaviour
{
    [SerializeField] private string _expGemPoolKey = "ExpGem";

    private int _expValue;

    /// <summary>Enemy 사망 시 드랍할 경험치 값을 설정한다.</summary>
    public void SetExpValue(int value) => _expValue = value;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        ExperienceSystem.Instance?.AddExperience(_expValue);
        ObjectPoolManager.Instance.Return(_expGemPoolKey, gameObject);
    }
}
