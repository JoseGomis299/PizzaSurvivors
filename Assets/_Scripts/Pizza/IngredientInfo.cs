using UnityEngine;

[CreateAssetMenu(menuName = "Ingredients/Ingredient", fileName = "Ingredient")]
public class IngredientInfo : ScriptableObject
{
    public new string name;
    public Sprite sprite;
    
    [field: SerializeField] public BulletModifierInfo[] BulletModifiers { get; private set; }
    [field: SerializeField] public BuffData[] Buffs { get; private set; }
}