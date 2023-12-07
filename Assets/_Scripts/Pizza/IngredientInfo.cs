using UnityEngine;

[CreateAssetMenu(menuName = "Ingredients/Ingredient", fileName = "Ingredient")]
public class IngredientInfo : ScriptableObject
{
    public new string name;
    public Sprite sprite;
    
    [SerializeField] private BulletModifierInfo modifier;
    public string GetModifierInfo(int level) => modifier.GetDescription(level);
}