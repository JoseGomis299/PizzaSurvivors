using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ingredient", fileName = "Ingredient")]
public class Ingredient : StoreItem
{
    public Sprite onPizzaSprite;
    [HideInInspector] public Ingredient OriginalIngredient;
    
    [field: SerializeField] public BulletModifierInfo[] BulletModifiers { get; private set; }
    [field: SerializeField] public BuffData[] Buffs { get; set; }

    public int GetModifierCount(BulletModifierInfo modifier)
    {
        int count = 0;
        foreach (var modifierInfo in BulletModifiers)
        {
            if (modifierInfo.name == modifier.name) count++;
        }
        return count;
    }
    
    public void Initialize(Ingredient ingredient)
    {
        name = ingredient.name;
        icon = ingredient.icon;
        onPizzaSprite = ingredient.onPizzaSprite;
        OriginalIngredient = ingredient;
        
        BulletModifiers = new BulletModifierInfo[ingredient.BulletModifiers.Length];
        for (int i = 0; i < ingredient.BulletModifiers.Length; i++)
        {
            BulletModifiers[i] = ingredient.BulletModifiers[i];
        }
        Buffs = new BuffData[ingredient.Buffs.Length];
        for (int i = 0; i < ingredient.Buffs.Length; i++)
        {
            Buffs[i] = ingredient.Buffs[i];
        }
    }
}

