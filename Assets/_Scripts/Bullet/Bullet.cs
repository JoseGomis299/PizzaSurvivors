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
    private Element _element;

    //Movement
    public float Speed { get; set; }
    private float _initialSpeed;
    public Vector2 Direction { get; set; }
    private Vector2 _initialDirection;

    private Rigidbody2D _rb;
    private Dictionary<Type, BulletMovementModifier> _movementModifiers;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnDisable()
    {
        if (_movementModifiers == null) return;
        foreach (var modifier in _movementModifiers)
            modifier.Value.ReApply();
    }

    public void Initialize(Vector2 direction, List<BulletModifierInfo> modifiers, BulletSpawner spawner, Stats characterStats, Element element = Element.None)
    {
        Spawner = spawner;
        _element = element;
        
        _modifiers = new List<BulletModifier>();
        foreach (var modifier in modifiers)
            _modifiers.Add(modifier.GetModifier(this));

        // If there are any bullet stats modifiers, create a new BulletStats object and apply the modifiers to it
        if (_modifiers.Any(modifier => modifier is BulletStatsModifier))
        {
            _stats = ScriptableObject.CreateInstance<BulletStats>();
            if(characterStats != null) _stats.SetValues(characterStats, stats);
            else _stats.SetValues(stats);
            
            foreach (var modifier in _modifiers.OfType<BulletStatsModifier>())
                ApplyEffect(modifier);
        }
        else _stats = stats;

        // If there are any bullet movement modifiers, add them to the list, and apply them
        _movementModifiers = new Dictionary<Type, BulletMovementModifier>();
        foreach (var modifier in _modifiers.OfType<BulletMovementModifier>())
        {
            if(!_movementModifiers.ContainsKey(modifier.GetType()))
                _movementModifiers.Add(modifier.GetType(), modifier);
            
            _movementModifiers[modifier.GetType()].Apply();
        }
        _initialSpeed = Speed = _stats.BaseSpeed;
        _initialDirection = Direction = direction;
    }

    public void Move()
    {
        Speed = _initialSpeed;
        Direction = _initialDirection;
        foreach (var modifier in _movementModifiers)
            modifier.Value.ModifyMovement();
        
        _rb.velocity = Direction.normalized * Speed;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out IDamageable damageable) && col.gameObject != Spawner.gameObject)
        {
            damageable.TakeDamage(_stats.GetAttack(_element, stats.BaseDamage));
            ApplyHitModifiers();
            gameObject.SetActive(false);
        }
    }

    public void ApplyHitModifiers()
    {
        foreach (var modifier in _modifiers.Where(modifier => modifier.GetType() == typeof(BulletHitModifier)))
            modifier.Apply();
    }

    public void ApplyEffect(BaseEffect effect)
    {
        effect.Apply();
        ((BulletModifier)effect).RemainsAfterHit--;
    }
    public void ReApplyEffects() { }
}

