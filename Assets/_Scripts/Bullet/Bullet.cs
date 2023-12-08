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

    public BulletMover Mover { get; private set; }

    public void Initialize(Vector2 direction, List<BulletModifierInfo> modifiers, BulletSpawner spawner, Stats characterStats, Element element = Element.None)
    {
        _stats = ScriptableObject.CreateInstance<BulletStats>();
        if(characterStats != null) _stats.SetValues(characterStats, stats);
        else _stats.SetValues(stats);
        
        _modifiers = new List<BulletModifier>();
        foreach (var modifier in modifiers)
            _modifiers.Add(modifier.GetModifier(this));

        Spawner = spawner;
        _element = element;
        
        Mover = new BulletMover(GetComponent<Rigidbody2D>(), 
            _modifiers.FindAll(modifier => modifier is BulletMovementModifier).ConvertAll(modifier => modifier as BulletMovementModifier),
            stats.BaseSpeed, direction);

        foreach (var modifier in _modifiers.Where(modifier => modifier.GetType() == typeof(BulletStatsModifier)))
            ApplyEffect(modifier);
    }

    public void Move()
    {
        Mover.Move();
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