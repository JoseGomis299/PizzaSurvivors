using System.Collections.Generic;
using ProjectUtils.Helpers;
using UnityEngine;

public class TripleShotModifier : BulletShootModifier
{
    public TripleShotModifier(IEffectTarget target, int maxStacks, int remainsAfterHit, int priority) : base(target, maxStacks, remainsAfterHit, priority) { }

    public override List<BulletMovementData> GetModifications(BulletMovementData baseData)
    {
        List<BulletMovementData> modifications = new List<BulletMovementData>();
        Vector2 direction = baseData.Direction;
        
        direction = direction.Rotate(-45);
        modifications.Add(new BulletMovementData(baseData.StartPosition, baseData.StartPositionDistance, direction, baseData.Modifiers));
        
        direction = direction.Rotate(45);
        modifications.Add(new BulletMovementData(baseData.StartPosition, baseData.StartPositionDistance, direction, baseData.Modifiers));
        
        direction = direction.Rotate(45);
        modifications.Add(new BulletMovementData(baseData.StartPosition, baseData.StartPositionDistance, direction, baseData.Modifiers));
        
        return modifications;
    }

    public override void Apply() { }
    protected override void DeApply() { }
    public override void ReApply() { }
}