using UnityEngine;

[CreateAssetMenu(menuName = "StoreItem", fileName = "StoreItem")]
public class StoreItem : ScriptableObject
{
    public new string name;
    public Sprite icon;
    public int price;
    public Rarity rarity;
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}