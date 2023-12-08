using System.Collections.Generic;
using ProjectUtils.Helpers;

public class MultiDirectionalShotModifier : BulletShootModifier
{
    public MultiDirectionalShotModifier(IEffectTarget target, int maxStacks, int remainsAfterHit, int priority) : base(target, maxStacks, remainsAfterHit, priority)
    {
    }
    
    public override List<BulletMovementData> GetModifications(BulletMovementData baseData)
    {
        List<BulletMovementData> modifications = new List<BulletMovementData>();
        
        modifications.Add(baseData);
        BulletMovementData data = new BulletMovementData(baseData.StartPosition, baseData.StartPositionDistance, baseData.Direction, baseData.Modifiers);
        data.RotateStartPositionTo(baseData.Direction.Rotate(180));
        modifications.Add(data);
        
        if (CurrentStacks >= 2)
        {
            data = new BulletMovementData(baseData.StartPosition, baseData.StartPositionDistance, baseData.Direction, baseData.Modifiers);
            data.RotateStartPositionTo(baseData.Direction.Rotate(90));
            modifications.Add(data);
            data = new BulletMovementData(baseData.StartPosition, baseData.StartPositionDistance, baseData.Direction, baseData.Modifiers);
            data.RotateStartPositionTo(baseData.Direction.Rotate(-90));
            modifications.Add(data);
        }

        
        return modifications;
    }

    public override void Apply()
    {
        AddStack();
    }
    protected override void DeApply() { }
    public override void ReApply() { }
}