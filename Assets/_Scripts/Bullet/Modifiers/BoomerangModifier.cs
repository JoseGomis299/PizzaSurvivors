using System.Collections.Generic;
using UnityEngine;

public class BoomerangModifier : BulletMovementModifier
{
    private Vector2 _finalDirection;
    private bool _hasReturned = false;
    
    private Transform _spawner;

    public BoomerangModifier(Bullet target, int maxStacks, int priority, float amplitude, float frequency) : base(target, maxStacks, priority, amplitude, frequency)
    {
        priority = -1;
    }

    public override void Apply()
    {
        AddStack();
        _spawner = EffectTarget.PreviousHit != null ? EffectTarget.PreviousHit.transform : EffectTarget.Spawner.transform;
        EffectTarget.Spawner.GetFirePointDistance();
        EffectTarget.IgnoreMaxRange = true;
        _time = 0;
    }
    
    // public override void ModifyMovement()
    // {
    //     if(_spawner != null) _spawnerPosition = _spawner.position;
    //     
    //     float distance = Vector2.Distance(EffectTarget.transform.position, _spawnerPosition);
    //     float multiplier = Mathf.Lerp(-1, 1, time / frequency);
    //
    //     float t = Mathf.Clamp01(time / (frequency / 2f));
    //     float lowMultiplier = Mathf.Lerp(-1, 1, t);
    //     float highMultiplier = Mathf.Lerp(1, -1, time / frequency - t/2f);
    //
    //     if(multiplier < 0.25f) _returnDirection =  Vector2.Lerp(_returnDirection,_startPosition - (Vector2) EffectTarget.transform.position, Time.fixedDeltaTime * 10f);
    //     if (CurrentStacks >= 2 && !_hasReturned)
    //     {
    //         if (multiplier >= 0 && distance <= 1.25f) _hasReturned = true;
    //         else _returnDirection =  Vector2.Lerp(_returnDirection,_spawnerPosition - (Vector2) EffectTarget.transform.position, Time.fixedDeltaTime * 15f);
    //     }
    //
    //     EffectTarget.Direction += _returnDirection.normalized * (2 * lowMultiplier);
    //     EffectTarget.Direction += ((Vector2)EffectTarget.transform.up * ((multiplier > 0 ? highMultiplier : lowMultiplier) * amplitude));
    //     
    //     time += Time.fixedDeltaTime;
    // }
      
    public override void ModifyMovement()
    {
        float time = _time * ((EffectTarget.Stats.Speed*2f)/EffectTarget.Stats.MaxRange);
        if (time*Mathf.Rad2Deg < 350 || CurrentStacks > 1)
        {
            EffectTarget.Direction = (Vector2)EffectTarget.transform.right * (Mathf.Sin(time));
            EffectTarget.Direction -= (Vector2) EffectTarget.transform.up * (Mathf.Cos(time));
            _finalDirection = EffectTarget.Direction;
        }
        else
        {
            EffectTarget.Direction = _finalDirection;
            EffectTarget.IgnoreMaxRange = false;
        }
        
        _time += Time.fixedDeltaTime;
    }
}
