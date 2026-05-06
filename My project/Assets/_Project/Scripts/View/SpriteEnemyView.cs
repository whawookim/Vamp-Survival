using System.Collections;
using UnityEngine;

/// <summary>
/// SpriteRenderer 기반 IEnemyView 구현체.
/// 피격 시 색상 플래시, 이동 방향에 따른 스프라이트 반전을 처리한다.
///
/// TODO: Spine 도입 시 SpineEnemyView를 동일 인터페이스로 구현하고
///       이 컴포넌트와 교체한다. EnemyController 수정 없이 전환 가능하다.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteEnemyView : MonoBehaviour, IEnemyView
{
    [SerializeField] private Color _damageFlashColor = Color.red;
    [SerializeField] private float _flashDuration = 0.1f;

    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private Coroutine _flashCoroutine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalColor = _spriteRenderer.color;
    }

    public void OnSpawn()
    {
        _spriteRenderer.color = _originalColor;
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
        // Phase 2에서 사망 파티클 또는 Spine 사망 애니메이션을 여기에 추가한다.
    }

    public void SetMoveDirection(Vector2 direction)
    {
        if (direction.x == 0f) return;
        _spriteRenderer.flipX = direction.x < 0f;
    }

    private IEnumerator FlashRoutine()
    {
        _spriteRenderer.color = _damageFlashColor;
        yield return new WaitForSeconds(_flashDuration);
        _spriteRenderer.color = _originalColor;
        _flashCoroutine = null;
    }
}
