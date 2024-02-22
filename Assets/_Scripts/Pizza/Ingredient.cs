using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ingredient", fileName = "Ingredient")]
public class Ingredient : StoreItem
{
    public Sprite onPizzaSprite;
    
    [field: SerializeField] public BulletModifierInfo[] BulletModifiers { get; private set; }
    [field: SerializeField] public BuffData[] Buffs { get; private set; }

    public int GetModifierCount(BulletModifierInfo modifier)
    {
        int count = 0;
        foreach (var modifierInfo in BulletModifiers)
        {
            if (modifierInfo.name == modifier.name) count++;
        }
        return count;
    }
}

