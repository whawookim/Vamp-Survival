using UnityEngine;

/// <summary>
/// 적의 이동, HP 관리, 피격 판정, 접촉 데미지, 사망 처리를 담당한다.
/// 비주얼은 IEnemyView를 통해 처리하므로 렌더링 구현에 의존하지 않는다.
/// Phase 2: EnemyStats 기반 Initialize() 추가, 접촉 데미지 지원.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private string _enemyPoolKey  = "Enemy";
    [SerializeField] private string _expGemPoolKey = "ExpGem";

    /// <summary>접촉 데미지 쿨다운 (초). 동일 프레임 중복 피해 방지.</summary>
    [SerializeField] private float _contactDamageInterval = 0.5f;

    private Rigidbody2D _rigidbody;
    private IEnemyView  _view;
    private Transform   _playerTransform;

    // ── 런타임 스탯 (Initialize로 덮어 씌워진다) ──
    private int   _maxHp        = 30;
    private int   _currentHp;
    private float _moveSpeed    = 2.5f;
    private int   _expReward    = 10;
    private int   _contactDamage = 5;

    private bool  _isDead;
    private float _lastContactDamageTime = -999f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale  = 0f;
        _rigidbody.freezeRotation = true;

        _view = GetComponent<IEnemyView>();

        // Player는 씬 로드 후 한 번만 캐싱한다
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            _playerTransform = player.transform;
    }

    private void OnEnable()
    {
        _currentHp = _maxHp;
        _isDead    = false;
        _rigidbody.linearVelocity = Vector2.zero;

        // Player 참조가 없을 경우를 대비해 재캐싱 (씬 전환 대비)
        if (_playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null) _playerTransform = player.transform;
        }

        _view?.OnSpawn();
    }

    /// <summary>
    /// EnemySpawner에서 풀 꺼낸 직후 호출한다.
    /// EnemyStats 값으로 스탯을 초기화하고 비주얼을 갱신한다.
    /// </summary>
    public void Initialize(EnemyStats stats)
    {
        _maxHp         = stats.MaxHp;
        _currentHp     = stats.MaxHp;
        _moveSpeed     = stats.MoveSpeed;
        _expReward     = stats.ExpReward;
        _contactDamage = stats.ContactDamage;

        _view?.SetAppearance(stats.Color, stats.Scale);
    }

    private void FixedUpdate()
    {
        if (_isDead || _playerTransform == null) return;

        Vector2 direction = ((Vector2)_playerTransform.position - _rigidbody.position).normalized;
        _rigidbody.linearVelocity = direction * _moveSpeed;
        _view?.SetMoveDirection(direction);
    }

    /// <summary>데미지를 적용한다. 외부(ProjectileController 등)에서 호출한다.</summary>
    public void TakeDamage(int amount)
    {
        if (_isDead) return;

        _currentHp -= amount;
        _view?.OnTakeDamage((float)_currentHp / _maxHp);

        if (_currentHp <= 0)
            Die();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_isDead) return;
        if (!other.gameObject.CompareTag("Player")) return;
        if (Time.time - _lastContactDamageTime < _contactDamageInterval) return;

        _lastContactDamageTime = Time.time;
        if (other.gameObject.TryGetComponent<PlayerHealth>(out var health))
            health.TakeDamage(_contactDamage);
    }

    private void Die()
    {
        _isDead = true;
        _rigidbody.linearVelocity = Vector2.zero;
        _view?.OnDeath();

        DropExpGem();
        ObjectPoolManager.Instance.Return(_enemyPoolKey, gameObject);
    }

    private void DropExpGem()
    {
        GameObject gem = ObjectPoolManager.Instance.Get(_expGemPoolKey, transform.position, Quaternion.identity);
        if (gem != null && gem.TryGetComponent<ExpGemController>(out var expGem))
            expGem.SetExpValue(_expReward);
    }
}
