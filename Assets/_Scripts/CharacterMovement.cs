using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Vector2 _direction;
    private Rigidbody2D _rb;
    private StatsManager _statsManager;

    [SerializeField] private bool receiveKnockBack;
    private Vector2 _knockBackImpulse;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _statsManager = GetComponent<StatsManager>();
    }
    
    public void UpdateMovement(Vector3 direction, float deltaTime)
    {
        _direction = direction;
        
        _knockBackImpulse = Vector2.Lerp(_knockBackImpulse, Vector2.zero, deltaTime*10f);
        _rb.velocity = _direction * _statsManager.Stats.Speed + _knockBackImpulse;
    }

    public void ApplyKnockBack(Vector3 damageKnockBack)
    {
        if (receiveKnockBack)
            _knockBackImpulse = damageKnockBack;
    }
}