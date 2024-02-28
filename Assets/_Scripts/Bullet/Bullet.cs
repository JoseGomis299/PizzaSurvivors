using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ProjectUtils.Helpers;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
    public StatsManager PreviousHit { get; private set; }

    private Vector3 _initialPosition;
    
    //Movement
    public float Speed { get; set; }
    public bool IgnoreMaxRange { get; set; }
    
    private float _initialSpeed;
    public Vector2 Direction { get; set; }
    public Vector2 InitialDirection { get; private set; }

    private Rigidbody2D _rb;
    private SpriteRenderer _gfx;
    private Sprite _sprite;
    private float _exitPreviousHitTime;
    private PhysicsMaterial2D _bouncyMaterial;
    private PhysicsMaterial2D _normalMaterial;

    private EnumSet<BulletMovementModifier, BulletMovementModifierInfo.MovementModifierType> _movementModifiers;
    private EnumSet<BulletHitModifier, BulletHitModifierInfo.HitModifierType> _hitModifiers;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _gfx = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        _sprite = _gfx.sprite;
        
        _bouncyMaterial = new PhysicsMaterial2D
        {
            bounciness = 1f,
            friction = 0f
        };
        
        _normalMaterial = new PhysicsMaterial2D
        {
            bounciness = 0f,
            friction = 0f
        };
    }

    private void OnDisable()
    {
        Direction = Vector2.zero;
        Speed = 0;
        _gfx.sprite = _sprite;
        if(Spawner != null) _rb.includeLayers &= ~Helpers.GetLayerMask(Spawner.transform);
    }

    public void Initialize(Bullet other, Vector2 direction, StatsManager previousHit, Sprite newSprite)
    {
        Spawner = other.Spawner;
        _initialPosition = transform.position;
        _exitPreviousHitTime = float.MaxValue;
        ID = other.ID + 1;
        _rb.excludeLayers |= Helpers.GetLayerMask(Spawner.transform);
        if(newSprite != null) _gfx.sprite = newSprite;
        
        PreviousHit = previousHit;

        _stats = ScriptableObject.CreateInstance<BulletStats>();
        _stats.SetValues(other._characterStats, stats);
        _characterStats = other._characterStats;
        
        _modifiersInfo = new List<BulletModifierInfo>();
        
        _movementModifiers = new EnumSet<BulletMovementModifier, BulletMovementModifierInfo.MovementModifierType>();
        _hitModifiers = new EnumSet<BulletHitModifier, BulletHitModifierInfo.HitModifierType>();
        
        foreach (var mod in other._modifiersInfo)
        {
            switch (mod)
            {
                case BulletStatsModifierInfo statsModifier:
                    var modifierStats = statsModifier.GetBulletModification(this);
                    modifierStats.Apply();
                    break;
                case BulletMovementModifierInfo movementModifier:
                {
                    var modifierMovement = movementModifier.GetBulletModification(this);
                    int index = (int)movementModifier.type;

                    _movementModifiers.SetValue(index, modifierMovement);
                    _movementModifiers[index].Apply();
                    break;
                }
                case BulletHitModifierInfo hitModifier:
                {
                    int index = (int)hitModifier.type;
                    
                    if(other._hitModifiers[index].RemainsAfterHit - ID < 0) break;
                    var modifierHit = hitModifier.GetBulletModification(this);

                    _hitModifiers.SetValue(index, modifierHit);
                    _hitModifiers[index].Apply();
                    break;
                }
            }
            
            _modifiersInfo.Add(mod);
        }


        _rb.sharedMaterial = _stats.Bounce > 0 ? _bouncyMaterial : _normalMaterial;

        transform.localScale = Vector3.one * _stats.Size;

        _initialSpeed = Speed = _stats.Speed;
        InitialDirection = Direction = direction.normalized;
        
        transform.right = Direction;
    }
    
    public void Initialize(Vector2 direction, List<BulletModifierInfo> modifiers, BulletSpawner spawner, Stats characterStats)
    {
        Spawner = spawner;
        _initialPosition = transform.position;
        ID = 0;
        _exitPreviousHitTime = float.MinValue;
        _rb.excludeLayers |= Helpers.GetLayerMask(Spawner.transform);
        
        _stats = ScriptableObject.CreateInstance<BulletStats>();
        _stats.SetValues(characterStats, stats);
        _characterStats = characterStats;
        
        _modifiersInfo = new List<BulletModifierInfo>(modifiers);

        _movementModifiers = new EnumSet<BulletMovementModifier, BulletMovementModifierInfo.MovementModifierType>();
        _hitModifiers = new EnumSet<BulletHitModifier, BulletHitModifierInfo.HitModifierType>();
        foreach (var mod in modifiers)
        {
            switch (mod)
            {
                case BulletStatsModifierInfo statsModifier:
                    var modifierStats = statsModifier.GetBulletModification(this);
                    modifierStats.Apply();
                    break;
                case BulletMovementModifierInfo movementModifier:
                {
                    var modifierMovement = movementModifier.GetBulletModification(this);
                    int index = (int)movementModifier.type;

                    _movementModifiers.SetValue(index, modifierMovement);
                    _movementModifiers[index].Apply();
                    break;
                }
                case BulletHitModifierInfo hitModifier:
                {
                    var modifierHit = hitModifier.GetBulletModification(this);
                    int index = (int)hitModifier.type;

                    _hitModifiers.SetValue(index, modifierHit);
                    _hitModifiers[index].Apply();
                    break;
                }
            }
            
            _modifiersInfo.Add(mod);
        }
        
        _rb.sharedMaterial = _stats.Bounce > 0 ? _bouncyMaterial : _normalMaterial;

        transform.localScale = Vector3.one * _stats.Size;

        _initialSpeed = Speed = _stats.Speed;
        InitialDirection = Direction = direction.normalized;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        //Disable bullet if it's out of range
        if (!IgnoreMaxRange && Vector2.SqrMagnitude(transform.position - _initialPosition) > _stats.MaxRange * _stats.MaxRange)
        {
            foreach (var modifier in _hitModifiers)
            {
                modifier.OnHit(null, new Damage(_stats.GetAttack(_stats.Element, _stats.Damage), _stats.Element, _stats.KnockBack, transform.position), _hitModifiers.Where(m => m != modifier).ToList() , _stats.Element);
            }
            gameObject.SetActive(false);
        }
        
        Speed = _initialSpeed;
        Direction = InitialDirection;
        foreach (var modifier in _movementModifiers)
            modifier.ModifyMovement();
        
        _gfx.transform.up = Direction;
        _rb.velocity = Direction.normalized * Speed;
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if((Spawner != null && (Helpers.IsHimOrHisChild(col.transform, Spawner.transform) || col.collider.CompareTag(Spawner.tag))) || col.collider.CompareTag("Bullet") || col.collider.TryGetComponent<ICollectable>(out _)) return;
        var target = col.collider.GetComponent<StatsManager>();
        if(0.05f > Time.time - _exitPreviousHitTime) return;
        float attack = _stats.GetAttack(_stats.Element, _stats.Damage);
        Damage damage = new Damage(attack, _stats.Element, _stats.KnockBack, transform.position, Direction);
        
        var damageable = col.collider.GetComponent<IDamageable>();

        foreach (var modifier in _hitModifiers)
        {
            modifier.OnHit(target, damage, _hitModifiers.Where(m => m != modifier).ToList() , _stats.Element);
        }

        //Bounce if colliding with something
        if (--_stats.Bounce <= 0)
        {
            _rb.sharedMaterial = _normalMaterial;
        }
        else
        {
            InitialDirection = Vector2.Reflect(Direction, col.contacts[0].normal).normalized;
            return;
        }

        //Pierce if colliding with an enemy
        if(_stats.Pierce-- <= 0) gameObject.SetActive(false);
        if (damageable != null) damageable.TakeDamage(damage);
        else gameObject.SetActive(false);
    }


    private void OnCollisionExit2D(Collision2D col)
    {
       if (PreviousHit != null && col.gameObject == PreviousHit.gameObject)
       {
           PreviousHit = null;
           _exitPreviousHitTime = Time.time;
       }
    }
}

