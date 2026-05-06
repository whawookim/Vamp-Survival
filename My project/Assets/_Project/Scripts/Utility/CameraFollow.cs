using UnityEngine;

/// <summary>
/// Main Camera가 지정된 Target Transform을 부드럽게 따라가도록 처리한다.
/// LateUpdate를 사용해 Physics 이동이 완료된 후 카메라를 갱신한다.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _smoothSpeed = 8f;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 0f, -10f);

    private void LateUpdate()
    {
        if (_target == null) return;

        Vector3 desired = _target.position + _offset;
        transform.position = Vector3.Lerp(transform.position, desired, _smoothSpeed * Time.deltaTime);
    }
}
