using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ingredient", fileName = "Ingredient")]
public class Ingredient : ScriptableObject
{
    public new string name;
    public Sprite onPizzaSprite;
    public Sprite uiSprite;
    
    [field: SerializeField] public BulletModifierInfo[] BulletModifiers { get; private set; }
    [field: SerializeField] public ShotModifierInfo[] ShotModifiers { get; set; }
    [field: SerializeField] public BuffData[] Buffs { get; private set; }

    public List<Descriptor> GetDescriptors()
    {
        var res = new List<Descriptor>();
        res.AddRange(BulletModifiers);
        res.AddRange(ShotModifiers);
        return res;
    }
}