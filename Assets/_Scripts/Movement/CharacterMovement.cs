using System;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Vector2 _direction;
    private Rigidbody2D _rb;
    private StatsManager _statsManager;

    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] private bool receiveKnockBack;
    private Vector2 _knockBackImpulse;
    
    [Space(10)]
    [SerializeField] private float accelTime = 0.5f;
    [SerializeField] private float rollDistance = 3f;
    [SerializeField] private float rollTime = 0.2f;
    
    public float Speed => _direction.magnitude*_speed;
    
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
        
        _rb.excludeLayers = ignoreLayer;
        _isRolling = false;
    }

    private void OnDisable()
    {
        _knockBackImpulse = Vector2.zero;
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
        
        _rb.velocity = _direction * _speed + _knockBackImpulse;
        _knockBackImpulse = Vector2.Lerp(_knockBackImpulse, Vector2.zero, deltaTime*10f);

        _prevDirection = direction;
    }

    private void UpdateMovementRoll()
    {
        if (Time.time - _startRollTime >= rollTime)
        {
            _isRolling = false;
            return;
        }
        
        //SI SE QUIERE QUE SE DASH, SUMAR LA VARIABLE _statsManager.Stats.Speed A LA EXPRESIÓN DE ABAJO

        float val = -((2 * rollDistance) *
            (-1 + (Time.time - _startRollTime) / rollTime) / rollTime);

        _rb.velocity = _rollDirection * val;
        // Debug.Log(val);
        // Debug.Log(_rb.velocity);
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
        _speed = Mathf.Lerp(0, _statsManager.Stats.Speed, (Time.time - _startMoveTime) / accelTime);
    }

    public bool StartRoll(Vector3 direction)
    {
        if (_isRolling) return false;
        
        _rollDirection = direction;
        _isRolling = true;
        _startRollTime = Time.time;

        return true;
    }

    public void SetRollData(float rollDistance, float rollTime)
    {
        this.rollDistance = rollDistance;
        this.rollTime = rollTime;
    }
}