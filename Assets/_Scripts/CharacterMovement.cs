using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private Vector2 _direction;
    private Rigidbody2D _rb;
    private StatsManager _statsManager;
    
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
}