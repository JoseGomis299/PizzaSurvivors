using System.Collections.Generic;
using ProjectUtils.Helpers;
using UnityEngine;

public class TripleShotModifier : BulletShotModifier
{
    public TripleShotModifier(BulletSpawner target, int maxStacks, int priority, List<BulletModifierInfo> modifiers) : base(target, maxStacks, priority, modifiers) { }

    public override void Apply() { }
    
    public override List<BulletShotData> GetModifiedShotData(BulletShotData baseData)
    {
        List<BulletShotData> modifications = new List<BulletShotData>();
        List<BulletModifierInfo> modifiers = new List<BulletModifierInfo>(Modifiers);
        if(baseData.Modifiers != null) modifiers.AddRange(baseData.Modifiers);
        
        Vector2 direction = baseData.Direction;
        
        direction = direction.Rotate(-45);
        modifications.Add(new BulletShotData(baseData.StartPosition, baseData.StartPositionDistance, direction, modifiers));
        
        direction = direction.Rotate(45);
        modifications.Add(new BulletShotData(baseData.StartPosition, baseData.StartPositionDistance, direction, modifiers));
        
        direction = direction.Rotate(45);
        modifications.Add(new BulletShotData(baseData.StartPosition, baseData.StartPositionDistance, direction, modifiers));
        
        return modifications;
    }
}