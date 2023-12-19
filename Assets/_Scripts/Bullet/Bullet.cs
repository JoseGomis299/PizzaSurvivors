using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletStats stats;
    private BulletStats _stats;
    private Stats _characterStats;
    public BulletStats Stats => _stats;

    private List<BulletModifierInfo> _modifiersInfo;

    public BulletSpawner Spawner { get; private set; }
    public int ID { get; private set; }
    public IEffectTarget PreviousHit { get; private set; }

    private Vector3 _initialPosition;
    
    //Movement
    public float Speed { get; set; }
    private float _initialSpeed;
    public Vector2 Direction { get; set; }
    private Vector2 _initialDirection;

    private Rigidbody2D _rb;
    private GameObject _gfx;
    
    private Dictionary<Type, BulletMovementModifier> _movementModifiers;
    private Dictionary<Type, BulletHitModifier> _hitModifiers;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _gfx = transform.GetChild(0).gameObject;
    }

    public void Initialize(Bullet other, Vector2 direction, IEffectTarget previousHit)
    {
        Spawner = other.Spawner;
        _initialPosition = transform.position;
        ID = other.ID + 1;
        
        PreviousHit = previousHit;

        _stats = ScriptableObject.CreateInstance<BulletStats>();
        _stats.SetValues(other._characterStats, stats);
        _characterStats = other._characterStats;
        
        _modifiersInfo = new List<BulletModifierInfo>();
        
        _movementModifiers = new Dictionary<Type, BulletMovementModifier>();
        _hitModifiers = new Dictionary<Type, BulletHitModifier>();
        
        foreach (var mod in other._modifiersInfo)
        {
            var modifier = mod.GetBulletModification(this);
            if(other._hitModifiers.ContainsKey(modifier.GetType()) && other._hitModifiers[modifier.GetType()].RemainsAfterHit - ID < 0) continue;

            _modifiersInfo.Add(mod);

            switch (modifier)
            {
                case BulletStatsModifier statsModifier:
                    statsModifier.Apply();
                    break;
                case BulletMovementModifier movementModifier:
                {
                    if(!_movementModifiers.ContainsKey(movementModifier.GetType()))
                        _movementModifiers.Add(movementModifier.GetType(), movementModifier);
            
                    _movementModifiers[movementModifier.GetType()].Apply();
                    break;
                }
                case BulletHitModifier hitModifier:
                {
                    if(!_hitModifiers.ContainsKey(hitModifier.GetType()))
                        _hitModifiers.Add(hitModifier.GetType(), hitModifier);
            
                    _hitModifiers[hitModifier.GetType()].Apply();
                    break;
                }
            }
        }
        
        transform.localScale = Vector3.one * _stats.Size;

        _initialSpeed = Speed = _stats.Speed;
        _initialDirection = Direction = direction.normalized;
        
        transform.right = Direction;
    }
    
    public void Initialize(Vector2 direction, List<BulletModifierInfo> modifiers, BulletSpawner spawner, Stats characterStats)
    {
        Spawner = spawner;
        _initialPosition = transform.position;
        ID = 0;
        
        _stats = ScriptableObject.CreateInstance<BulletStats>();
        _stats.SetValues(characterStats, stats);
        _characterStats = characterStats;
        
        _modifiersInfo = new List<BulletModifierInfo>(modifiers);
        
        _movementModifiers = new Dictionary<Type, BulletMovementModifier>();
        _hitModifiers = new Dictionary<Type, BulletHitModifier>();
        
        foreach (var mod in modifiers)
        {
            var modifier = mod.GetBulletModification(this);

            switch (modifier)
            {
                case BulletStatsModifier statsModifier:
                    statsModifier.Apply();
                    break;
                case BulletMovementModifier movementModifier:
                {
                    if(!_movementModifiers.ContainsKey(movementModifier.GetType()))
                        _movementModifiers.Add(movementModifier.GetType(), movementModifier);
            
                    _movementModifiers[movementModifier.GetType()].Apply();
                    break;
                }
                case BulletHitModifier hitModifier:
                {
                    if(!_hitModifiers.ContainsKey(hitModifier.GetType()))
                        _hitModifiers.Add(hitModifier.GetType(), hitModifier);
            
                    _hitModifiers[hitModifier.GetType()].Apply();
                    break;
                }
            }
        }
        
        transform.localScale = Vector3.one * _stats.Size;

        _initialSpeed = Speed = _stats.Speed;
        _initialDirection = Direction = direction.normalized;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        //Disable bullet if it's out of range
        if(Vector2.SqrMagnitude(transform.position - _initialPosition) > _stats.MaxRange * _stats.MaxRange) gameObject.SetActive(false);
        
        Speed = _initialSpeed;
        Direction = _initialDirection;
        foreach (var modifier in _movementModifiers)
            modifier.Value.ModifyMovement();
        
        _gfx.transform.up = Direction;
        _rb.velocity = Direction.normalized * Speed;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if((Spawner != null && col.gameObject == Spawner.gameObject) || col.CompareTag("Bullet")) return;
        var effectTarget = col.GetComponent<IEffectTarget>();
        if(effectTarget != null && effectTarget == PreviousHit) return;
        
        float attack = _stats.GetAttack(_stats.Element, _stats.Damage);

        if (col.TryGetComponent(out IDamageable damageable))
            damageable.TakeDamage(attack, _stats.Element);

        foreach (var modifier in _hitModifiers)
        {
            modifier.Value.OnHit(col.GetComponent<IEffectTarget>(), attack, _hitModifiers.Values.Where(m => m != modifier.Value).ToList() , _stats.Element);
        }

        //Bounce if colliding with not an enemy
        if (damageable == null && _stats.Bounce-- > 0)
        {
            Vector2 normal = ((Vector2) (transform.position - (Vector3)Direction) - col.ClosestPoint(transform.position - (Vector3)Direction)).normalized;
            _initialDirection = Vector2.Reflect(Direction, normal);
            return;
        }
        //Pierce if colliding with an enemy
        if(_stats.Pierce-- <= 0 || damageable == null) gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if ((Spawner != null && col.gameObject == Spawner.gameObject) || col.CompareTag("Bullet")) return;

        if (!col.TryGetComponent(out IDamageable _) && _stats.Bounce-- > 0)
        {
            Vector2 normal = ((Vector2)(transform.position - (Vector3)Direction) - col.ClosestPoint(transform.position - (Vector3)Direction)).normalized;
            _initialDirection = Vector2.Reflect(Direction, normal);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PreviousHit = null;
    }
}

