using UnityEngine;

/// <summary>
/// WASD / 방향키 입력을 받아 Rigidbody2D로 플레이어를 이동시킨다.
/// 입력 처리(Update)와 물리 이동(FixedUpdate)을 분리하여 물리 프레임 독립성을 보장한다.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;

    private Rigidbody2D _rigidbody;
    private Vector2 _inputDirection;

    /// <summary>현재 프레임의 입력 방향 (정규화됨). 다른 컴포넌트에서 읽기 전용으로 참조한다.</summary>
    public Vector2 InputDirection => _inputDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = 0f;
        _rigidbody.freezeRotation = true;
    }

    private void Update()
    {
        _inputDirection = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;
    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = _inputDirection * _moveSpeed;
    }
}
