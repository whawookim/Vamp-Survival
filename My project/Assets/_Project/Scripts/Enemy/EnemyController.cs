using UnityEngine;

/// <summary>
/// 적의 이동, HP 관리, 피격 판정, 사망 처리를 담당한다.
/// 비주얼은 IEnemyView를 통해 처리하므로 렌더링 구현에 의존하지 않는다.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2.5f;
    [SerializeField] private int _maxHp = 30;
    [SerializeField] private int _expReward = 10;
    [SerializeField] private string _enemyPoolKey = "Enemy";
    [SerializeField] private string _expGemPoolKey = "ExpGem";

    private Rigidbody2D _rigidbody;
    private IEnemyView _view;
    private Transform _playerTransform;
    private int _currentHp;
    private bool _isDead;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0f;
        _rigidbody.freezeRotation = true;

        // IEnemyView는 같은 GameObject에 있는 컴포넌트에서 가져온다
        _view = GetComponent<IEnemyView>();
    }

    private void OnEnable()
    {
        _currentHp = _maxHp;
        _isDead = false;
        _view?.OnSpawn();

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            _playerTransform = player.transform;
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
