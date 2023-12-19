using System.Collections.Generic;
using UnityEngine;

public class BoomerangModifier : BulletMovementModifier
{
    private Vector2 _startPosition;
    private Vector2 _returnDirection = Vector2.zero;
    
    private Vector2 _spawnerPosition;
    private bool _hasReturned = false;
    
    private Transform _spawner;

    public BoomerangModifier(Bullet target, int maxStacks, int priority, float amplitude, float frequency) : base(target, maxStacks, priority, amplitude, frequency)
    {
    }

    public override void Apply()
    {
        AddStack();
        _spawner = EffectTarget.PreviousHit != null ? ((MonoBehaviour) EffectTarget.PreviousHit).transform : EffectTarget.Spawner.transform;
        _startPosition = _spawner.position;
    }
    
    public override void ModifyMovement()
    {
        if(_spawner != null) _spawnerPosition = _spawner.position;
        Debug.Log(_spawner);
        
        float distance = Vector2.Distance(EffectTarget.transform.position, _spawnerPosition);
        float multiplier = Mathf.Lerp(-1, 1, time / frequency);

        float t = Mathf.Clamp01(time / (frequency / 2f));
        float lowMultiplier = Mathf.Lerp(-1, 1, t);
        float highMultiplier = Mathf.Lerp(1, -1, time / frequency - t/2f);

        if(multiplier < 0.25f) _returnDirection =  Vector2.Lerp(_returnDirection,_startPosition - (Vector2) EffectTarget.transform.position, Time.fixedDeltaTime * 10f);
        if (CurrentStacks >= 2 && !_hasReturned)
        {
            if (multiplier >= 0 && distance <= 1.25f) _hasReturned = true;
            else _returnDirection =  Vector2.Lerp(_returnDirection,_spawnerPosition - (Vector2) EffectTarget.transform.position, Time.fixedDeltaTime * 15f);
        }

        EffectTarget.Direction += _returnDirection.normalized * (2 * lowMultiplier);
        EffectTarget.Direction += ((Vector2)EffectTarget.transform.up * ((multiplier > 0 ? highMultiplier : lowMultiplier) * amplitude));
        
        time += Time.fixedDeltaTime;
    }
}
