using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour, IEffectTarget
{
    [SerializeField] private BulletStats stats;
    private BulletStats _stats;

    public BulletStats BaseStats => stats;
    public BulletStats Stats => _stats;

    private List<BulletModifier> _modifiers;

    public BulletSpawner Spawner { get; private set; }

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
    
    public void Initialize(Vector2 direction, List<BulletModifierInfo> modifiers, BulletSpawner spawner, Stats characterStats)
    {
        Spawner = spawner;
        
        _modifiers = new List<BulletModifier>();
        foreach (var modifier in modifiers)
            _modifiers.Add(modifier.GetModifier(this));

        _stats = ScriptableObject.CreateInstance<BulletStats>();
        _stats.SetValues(characterStats, stats);

        //Apply all bullet stats modifiers
        foreach (var modifier in _modifiers.OfType<BulletStatsModifier>())
            modifier.Apply();
        
        transform.localScale = Vector3.one * _stats.Size;

        _movementModifiers = new Dictionary<Type, BulletMovementModifier>();
        _hitModifiers = new Dictionary<Type, BulletHitModifier>();
        //If there are any bullet modifiers that need to be applied on shoot, apply them and add them to the dictionaries
        foreach (var modifier in _modifiers)
        {
            if (modifier is BulletMovementModifier movementModifier)
            {
                if(!_movementModifiers.ContainsKey(movementModifier.GetType()))
                    _movementModifiers.Add(movementModifier.GetType(), movementModifier);
            
                _movementModifiers[movementModifier.GetType()].Apply();
            }
            else if (modifier is BulletHitModifier hitModifier)
            {
                if(!_hitModifiers.ContainsKey(hitModifier.GetType()))
                    _hitModifiers.Add(hitModifier.GetType(), hitModifier);
            
                _hitModifiers[hitModifier.GetType()].Apply();
            }
        }
        
        _initialSpeed = Speed = _stats.Speed;
        _initialDirection = Direction = direction;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Speed = _initialSpeed;
        Direction = _initialDirection;
        foreach (var modifier in _movementModifiers)
            modifier.Value.ModifyMovement();
        
        _gfx.transform.up = Direction;
        _rb.velocity = Direction.normalized * Speed;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        HandleCollision(col);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        HandleCollision(col);
    }

    private void HandleCollision(Collider2D col)
    {
        if((Spawner != null && col.gameObject == Spawner.gameObject) || col.CompareTag("Bullet")) return;
        
        float attack = _stats.GetAttack(_stats.Element, _stats.Damage);

        if (col.TryGetComponent(out IDamageable damageable))
            damageable.TakeDamage(attack, _stats.Element);

        foreach (var modifier in _hitModifiers)
        {
            modifier.Value.OnHit(col.GetComponent<IEffectTarget>(), attack, modifier.Value is ExplosiveModifier ? _hitModifiers.Values.Where(m => m.RemainsAfterHit > 0 && m != modifier.Value).ToList() : null, _stats.Element);
        }

        //Bounce if colliding with not an enemy
        if (damageable == null && _stats.Bounce-- > 0)
        {
            Vector2 normal = ((Vector2) (transform.position - (Vector3)Direction) - col.ClosestPoint(transform.position - (Vector3)Direction)).normalized;
            _initialDirection = Vector2.Reflect(Direction, normal);
            return;
        }
        //Piece if colliding with an enemy
        if(_stats.Pierce-- <= 0 || damageable == null) gameObject.SetActive(false);
    }

    public void ApplyEffect(IEffect effect) { }
    public void ReApplyEffects() { }
}

