using UnityEngine;

public class SineMovementModifier : BulletMovementModifier
{
    public SineMovementModifier(IEffectTarget target, int maxStacks, int remainsAfterHit, int priority, float amplitude, float frequency) : base(target, maxStacks, remainsAfterHit, priority, amplitude, frequency)
    { }
    
    public override void Apply() { }
    
    public override void ModifyMovement()
    {
        EffectTarget.Direction +=  (Vector2) EffectTarget.transform.up * (Mathf.Cos(time * frequency) * amplitude);
        time += Time.fixedDeltaTime;    
    }
}