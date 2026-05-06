using System.Collections;
using UnityEngine;

/// <summary>
/// 발사체의 이동, Enemy 충돌 시 데미지 적용, 수명 만료 시 자동 반환을 처리한다.
/// Rigidbody2D + CircleCollider2D(IsTrigger=true)로 구성한다.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifetime = 3f;
    [SerializeField] private string _projectilePoolKey = "Projectile";

    private Rigidbody2D _rigidbody;
    private int _damage;
    private Coroutine _lifetimeCoroutine;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0f;
        _rigidbody.freezeRotation = true;
    }

    private void OnEnable()
    {
        // 풀에서 재활성화될 때 velocity를 초기화해 잔류 운동 방지
        _rigidbody.linearVelocity = Vector2.zero;
    }

    /// <summary>방향과 데미지를 설정하고 발사를 시작한다. PlayerAutoAttack에서 호출한다.</summary>
    public void Initialize(Vector2 direction, int damage)
    {
        _damage = damage;
        _rigidbody.linearVelocity = direction.normalized * _speed;

        // 발사 방향으로 스프라이트를 회전시킨다 (선택적)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (_lifetimeCoroutine != null)
            StopCoroutine(_lifetimeCoroutine);

        _lifetimeCoroutine = StartCoroutine(LifetimeRoutine());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<EnemyController>(out var enemy)) return;

        enemy.TakeDamage(_damage);
        ReturnToPool();
    }

    private IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(_lifetime);
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (_lifetimeCoroutine != null)
        {
            StopCoroutine(_lifetimeCoroutine);
            _lifetimeCoroutine = null;
        }

        ObjectPoolManager.Instance.Return(_projectilePoolKey, gameObject);
    }
}
