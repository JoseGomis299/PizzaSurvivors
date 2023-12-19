using System.Collections.Generic;
using ProjectUtils.Helpers;

public class MultiDirectionalShotModifier : BulletShotModifier
{
    public MultiDirectionalShotModifier(BulletSpawner target, int maxStacks, int priority, List<BulletModifierInfo> modifiers) : base(target, maxStacks, priority, modifiers)
    {
    }
    
    public override List<BulletShotData> GetModifiedShotData(BulletShotData baseData)
    {
        List<BulletShotData> modifications = new List<BulletShotData>();
        List<BulletModifierInfo> modifiers = new List<BulletModifierInfo>(Modifiers);
        if(baseData.Modifiers != null) modifiers.AddRange(baseData.Modifiers);

        BulletShotData data = new BulletShotData(baseData.StartPosition, baseData.StartPositionDistance, baseData.Direction, modifiers);
        modifications.Add(data);
        data.RotateStartPositionTo(baseData.Direction.Rotate(180));
        modifications.Add(data);
        
        if (CurrentStacks >= 2)
        {
            data.RotateStartPositionTo(baseData.Direction.Rotate(90));
            modifications.Add(data);
            data.RotateStartPositionTo(baseData.Direction.Rotate(-90));
            modifications.Add(data);
        }

        return modifications;
    }

    public override void Apply()
    {
        AddStack();
    }
}