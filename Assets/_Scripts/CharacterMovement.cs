using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Vector2 _direction;
    private Rigidbody2D _rb;
    private StatsManager _statsManager;

    [SerializeField] private bool receiveKnockBack;
    private Vector2 _knockBackImpulse;
    
    //VARIABLECITAS PARA MOVIMIENTO TO GUAPO QUE SE SIENTA BIEN
    private float _startMoveTime = 0f;
    private float _speed = 0f;
    private Vector2 _prevDirection = new Vector2(0, 0);

    private bool _isRolling = false;
    private float _startRollTime = 0f;
    private Vector2 _rollDirection = new Vector2(0, 0);
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _statsManager = GetComponent<StatsManager>();
    }
    
    public void UpdateMovement(Vector3 direction, float deltaTime)
    {
        if (_isRolling)
        {
            UpdateMovementRoll();
            return;
        }
        
        _direction = direction;

        if (_prevDirection.sqrMagnitude == 0 && _direction.sqrMagnitude > 0)
        {
            SetStartMoveTime();
        }
        
        HandleAccel();
        
        _knockBackImpulse = Vector2.Lerp(_knockBackImpulse, Vector2.zero, deltaTime*10f);
        _rb.velocity = _direction * _speed + _knockBackImpulse;

        _prevDirection = direction;
    }

    private void UpdateMovementRoll()
    {
        if (Time.time - _startRollTime >= _statsManager.Stats.RollTime)
        {
            _isRolling = false;
            return;
        }

        float val = -((2 * _statsManager.Stats.RollDistance) *
            (-1 + (Time.time - _startRollTime) / _statsManager.Stats.RollTime) / _statsManager.Stats.RollTime);

        _rb.velocity = _rollDirection * val;
        Debug.Log(val);
        Debug.Log(_rb.velocity);
    }

    public void ApplyKnockBack(Vector3 damageKnockBack)
    {
        if (receiveKnockBack)
            _knockBackImpulse = damageKnockBack;
    }

    public void SetStartMoveTime()
    {
        _startMoveTime = Time.time;
    }

    private void HandleAccel()
    {
        _speed = Mathf.Lerp(0, _statsManager.Stats.Speed, (Time.time - _startMoveTime) / _statsManager.Stats.AccelTime);
    }

    public bool StartRoll(Vector3 direction)
    {
        if (_isRolling) return false;
        
        _rollDirection = direction;
        _isRolling = true;
        _startRollTime = Time.time;

        return true;
    }
}