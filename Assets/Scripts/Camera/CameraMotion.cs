using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _speedCursor = 10f;
    [SerializeField] private float _smoothing = 5f;
    [SerializeField] private Vector2 _range = new (100, 100);
    [SerializeField] private float borderLimit = 10f;

    private Vector3 _targetPosition;
    private Vector3 _input;

    private void Awake()
    {
        _targetPosition = transform.position;
    }

    private void HandleInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 right = transform.right * x;
        Vector3 forward = transform.forward * z;

        _input = (forward + right).normalized;
    }

    private void Move()
    {
        Vector3 nextTargetPosition = _targetPosition + _input * _speed;

        if (IsInBounds(nextTargetPosition)) 
            _targetPosition = nextTargetPosition;

        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _smoothing);
    }

    private void MoveCursor()
    {
        if (Input.mousePosition.y > Screen.height - borderLimit && _targetPosition.z < _range.y)
        {
            _targetPosition.z += _speedCursor * Time.deltaTime;
        }

        if (Input.mousePosition.y < borderLimit && _targetPosition.z > -_range.y)
        {
            _targetPosition.z -= _speedCursor * Time.deltaTime;
        }

        if (Input.mousePosition.x > Screen.width - borderLimit && _targetPosition.x < _range.x)
        {
            _targetPosition.x += _speedCursor * Time.deltaTime;
        }

        if (Input.mousePosition.x < borderLimit && _targetPosition.x > -_range.x)
        {
            _targetPosition.x -= _speedCursor * Time.deltaTime;
        }

        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _smoothing);
    }

    private bool IsInBounds(Vector3 position)
    {
        return position.x > -_range.x &&
            position.x < _range.x &&
            position.z > -_range.y &&
            position.z < _range.y;
    }

    private void Update()
    {
        HandleInput();

        Move();
        MoveCursor();
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 5f);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(_range.x * 2f, 5f, _range.y * 2f));
    }*/
}