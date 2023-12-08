using UnityEngine;

public class SineMovementModifier : BulletMovementModifier
{
    public SineMovementModifier(IEffectTarget target, int maxStacks, int remainsAfterHit, int priority, float amplitude, float frequency) : base(target, maxStacks, remainsAfterHit, priority, amplitude, frequency)
    { }

    public override void Apply()
    {
        EffectTarget.Mover.Direction +=  (Vector2) EffectTarget.transform.up * (Mathf.Cos(Time * Frequency) * Amplitude);
        Time += UnityEngine.Time.deltaTime;
    }

    protected override void DeApply() { }
    public override void ReApply() { }
}