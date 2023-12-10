using System.Collections.Generic;
using UnityEngine;

public class BoomerangModifier : BulletMovementModifier
{
    private Vector2 _startPosition;
    private Vector2 _returnDirection = Vector2.zero;

    public BoomerangModifier(IEffectTarget target, int maxStacks, int remainsAfterHit, int priority, float amplitude, float frequency) : base(target, maxStacks, remainsAfterHit, priority, amplitude, frequency)
    {
    }

    public override void Apply()
    {
        AddStack();
        _startPosition = EffectTarget.Spawner.transform.position;
    }

    protected override void DeApply() { }

    public override void ReApply()
    {
        time = 0;
        _returnDirection = Vector2.zero;
    }
    
    public override void ModifyMovement()
    {
        float distance = Vector2.Distance(EffectTarget.transform.position, EffectTarget.Spawner.transform.position);
        float multiplier = Mathf.Lerp(-1, 1, time / frequency);

        float t = Mathf.Clamp01(time / (frequency / 2f));
        float lowMultiplier = Mathf.Lerp(-1, 1, t);
        float highMultiplier = Mathf.Lerp(1, -1, time / frequency - t/2f);

        if(multiplier < 0.5f) _returnDirection =  (_startPosition - (Vector2) EffectTarget.transform.position);
        if (CurrentStacks >= 2)
        {
            _returnDirection =  (EffectTarget.Spawner.transform.position - EffectTarget.transform.position);
            if(distance <= 0.3f) EffectTarget.gameObject.SetActive(false);
        }

        EffectTarget.Direction += _returnDirection.normalized * (2 * lowMultiplier);
        EffectTarget.Direction += ((Vector2)EffectTarget.transform.up * ((multiplier > 0 ? highMultiplier : lowMultiplier) * amplitude));
        
        time += Time.fixedDeltaTime;
    }
}
