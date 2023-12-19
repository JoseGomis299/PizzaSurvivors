using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Vector2 _direction;
    private Rigidbody2D _rb;
    private StatsManager _statsManager;

    [SerializeField] private bool receiveKnockBack;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _statsManager = GetComponent<StatsManager>();
    }
    
    public void UpdateMovement(Vector3 direction)
    {
        _direction = direction;
        
        _rb.velocity = _direction * _statsManager.Stats.Speed;
    }

    public void ApplyKnockBack(Vector3 damageKnockBack)
    {
        if(receiveKnockBack)
            _rb.AddForce(damageKnockBack, ForceMode2D.Impulse);
    }
}