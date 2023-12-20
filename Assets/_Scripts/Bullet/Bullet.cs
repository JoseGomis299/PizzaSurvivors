using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    private float _initialSpeed;
    public Vector2 Direction { get; set; }
    private Vector2 _initialDirection;

    private Rigidbody2D _rb;
    private GameObject _gfx;
    
    private EnumSet<BulletMovementModifier> _movementModifiers;
    private EnumSet<BulletHitModifier> _hitModifiers;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _gfx = transform.GetChild(0).gameObject;
    }

    public void Initialize(Bullet other, Vector2 direction, StatsManager previousHit)
    {
        Spawner = other.Spawner;
        _initialPosition = transform.position;
        ID = other.ID + 1;
        
        PreviousHit = previousHit;

        _stats = ScriptableObject.CreateInstance<BulletStats>();
        _stats.SetValues(other._characterStats, stats);
        _characterStats = other._characterStats;
        
        _modifiersInfo = new List<BulletModifierInfo>();
        
        _movementModifiers = new EnumSet<BulletMovementModifier>(typeof(BulletMovementModifierInfo.MovementModifierType));
        _hitModifiers = new EnumSet<BulletHitModifier>(typeof(BulletHitModifierInfo.HitModifierType));
        
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

        _movementModifiers = new EnumSet<BulletMovementModifier>(typeof(BulletMovementModifierInfo.MovementModifierType));
        _hitModifiers = new EnumSet<BulletHitModifier>(typeof(BulletHitModifierInfo.HitModifierType));
        
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
            modifier.ModifyMovement();
        
        _gfx.transform.up = Direction;
        _rb.velocity = Direction.normalized * Speed;
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if((Spawner != null && col.gameObject == Spawner.gameObject) || col.CompareTag("Bullet")) return;
        var effectTarget = col.GetComponent<StatsManager>();
        if(effectTarget != null && effectTarget == PreviousHit) return;
        
        float attack = _stats.GetAttack(_stats.Element, _stats.Damage);
        Damage damage = new Damage(attack, _stats.Element, _stats.KnockBack, Direction);
        
        if (col.TryGetComponent(out IDamageable damageable))
            damageable.TakeDamage(damage);

        foreach (var modifier in _hitModifiers)
        {
            modifier.OnHit(col.GetComponent<StatsManager>(), damage, _hitModifiers.Where(m => m != modifier).ToList() , _stats.Element);
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

