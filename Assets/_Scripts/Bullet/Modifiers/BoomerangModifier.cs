using System.Collections.Generic;
using UnityEngine;

public class BoomerangModifier : BulletMovementModifier
{
    public BoomerangModifier(IEffectTarget target, int maxStacks, int remainsAfterHit, int priority, float amplitude, float frequency) : base(target, maxStacks, remainsAfterHit, priority, amplitude, frequency)
    {
    }

    public override void Apply()
    {
        float distance = Vector2.Distance(EffectTarget.transform.position, EffectTarget.Spawner.transform.position);
        float multiplier = Mathf.Lerp(-1, 1, time / frequency);

        float t = Mathf.Clamp01(time / (frequency / 2f));
        float lowMultiplier = Mathf.Lerp(-1, 1, t);
        float highMultiplier = Mathf.Lerp(1, -1, time / frequency - t/2f); //min value is 0
        
        EffectTarget.Direction += (Vector2)(EffectTarget.Spawner.transform.position - EffectTarget.transform.position).normalized * (2 * lowMultiplier);

        EffectTarget.Direction += ((Vector2)EffectTarget.transform.up * ((multiplier > 0 ? highMultiplier : lowMultiplier) * amplitude));

        if(distance <= 0.3f) DeApply();
        
        time += Time.fixedDeltaTime;
    }

    protected override void DeApply()
    {
        time = 0;
        EffectTarget.gameObject.SetActive(false);
    }
    public override void ReApply() { }
}
