using UnityEngine;

public class SineMovementModifier : BulletMovementModifier
{
    public SineMovementModifier(Bullet target, int maxStacks, int priority, float amplitude, float frequency) : base(target, maxStacks, priority, amplitude, frequency)
    { }
    
    public override void Apply() { }
    
    public override void ModifyMovement()
    {
        EffectTarget.Direction +=  (Vector2) EffectTarget.transform.up * (Mathf.Cos(time * frequency) * amplitude);
        time += Time.fixedDeltaTime;    
    }
}