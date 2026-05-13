using System.Collections;
using UnityEngine;

/// <summary>
/// SpriteRenderer 기반 IEnemyView 구현체.
/// Phase 2: SetAppearance()로 EnemyType별 색상과 스케일을 지원한다.
/// 피격 플래시는 타입 색상으로 복귀하며, 원본 프리팹 색상을 덮어쓰지 않는다.
///
/// TODO: Spine 도입 시 SpineEnemyView를 동일 인터페이스로 구현하고
///       이 컴포넌트와 교체한다. EnemyController 수정 없이 전환 가능하다.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteEnemyView : MonoBehaviour, IEnemyView
{
    [SerializeField] private Color _damageFlashColor = Color.white;
    [SerializeField] private float _flashDuration    = 0.1f;

    private SpriteRenderer _spriteRenderer;

    /// <summary>현재 적 타입의 기준 색상. 피격 플래시 후 이 색으로 복귀한다.</summary>
    private Color _typeColor = Color.white;

    private Coroutine _flashCoroutine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _typeColor = _spriteRenderer.color;
    }

    public void OnSpawn()
    {
        // SetAppearance가 뒤에 호출되므로 여기서는 색상/스케일만 초기화한다
        _spriteRenderer.color = _typeColor;
    }

    public void OnTakeDamage(float normalizedHp)
    {
        if (_flashCoroutine != null)
            StopCoroutine(_flashCoroutine);

        _flashCoroutine = StartCoroutine(FlashRoutine());
    }

    public void OnDeath()
    {
        // 사망 비주얼은 Pool 반환(SetActive false)으로 대체한다.
        // Phase 3에서 사망 파티클 또는 Spine 사망 애니메이션 추가 예정.
    }

    public void SetMoveDirection(Vector2 direction)
    {
        if (direction.x == 0f) return;
        _spriteRenderer.flipX = direction.x < 0f;
    }

    /// <summary>EnemyType에 따른 색상과 스케일을 적용한다.</summary>
    public void SetAppearance(Color color, float scale)
    {
        _typeColor = color;
        _spriteRenderer.color = color;
        transform.localScale = Vector3.one * scale;
    }

    private IEnumerator FlashRoutine()
    {
        _spriteRenderer.color = _damageFlashColor;
        yield return new WaitForSeconds(_flashDuration);
        _spriteRenderer.color = _typeColor;
        _flashCoroutine = null;
    }
}
