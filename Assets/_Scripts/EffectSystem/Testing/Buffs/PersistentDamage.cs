using System.Collections;
using ProjectUtils.ObjectPooling;
using UnityEngine;

public class PersistentDamage : BaseEffect
{
    private readonly float _damage;
    private readonly DamageType _damageType;
    private readonly float _interval;
    private readonly Element _element;
    
    private Coroutine _damageCoroutine;
    private readonly HealthComponent _healthComponent;
    
    private readonly GameObject _visualEffect;
    private GameObject _effectInstance;
    private ParticleSystem _particleSystem;
    
    public PersistentDamage(StatsManager target, float duration, float damage, Element element, DamageType damageType, float interval, GameObject visualEffect) : base(target, duration, 1, TimerType.Default)
    {
        _healthComponent = target.GetComponent<HealthComponent>();
        _damage = damage;
        _element = element;
        _damageType = damageType;
        _interval = interval;
        _visualEffect = visualEffect;
    }

    public override void Apply()
    {
        Timer.ResetTimer();
        if (_particleSystem != null)
        {
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            var main = _particleSystem.main;
            main.duration = Duration - 1;
            _particleSystem.Play();
        }

        if(_damageCoroutine != null) return;
        
        DeApplyOnTimerEnd();
        _damageCoroutine = EffectTarget.StartCoroutine(DealDamage());
        _effectInstance = ObjectPool.Instance.InstantiateFromPool(_visualEffect, EffectTarget.transform.position, Quaternion.identity);
        _particleSystem = _effectInstance.GetComponent<ParticleSystem>();
        _effectInstance.transform.parent = EffectTarget.transform;
    }

    public override void DeApply()
    {
        if(_damageCoroutine == null) return;

        ((MonoBehaviour) EffectTarget).StopCoroutine(_damageCoroutine);
        _effectInstance.SetActive(false);

        _damageCoroutine = null;
        _particleSystem = null;
    }

    private IEnumerator DealDamage()
    {
        while (true)
        {
            float damage = _damageType switch
            {
                DamageType.Additive => _damage,
                DamageType.MaxHealthPercentage => _healthComponent.MaxHealth * _damage,
                DamageType.CurrentHealthPercentage => _healthComponent.Health * _damage,
                _ => _damage
            };
            _healthComponent.TakeDamage(damage, _element);

            yield return new WaitForSeconds(_interval);
        }
    }

    public override void ReApply() { }
}

public enum DamageType
{
    Additive,
    MaxHealthPercentage,
    CurrentHealthPercentage
}