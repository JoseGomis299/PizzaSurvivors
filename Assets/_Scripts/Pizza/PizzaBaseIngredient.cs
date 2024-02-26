using UnityEngine;

public class PizzaBaseIngredient : MonoBehaviour
{
    private Paintable _paintable; 
    private IIngrdientModifier _ingredientModifier;
    [SerializeField] private BaseIngredientType ingredientType;
    
    private void Awake()
    {
        _paintable = GetComponent<Paintable>();
        
        _ingredientModifier = ingredientType switch
        {
            BaseIngredientType.MozzarellaCheese => new MozzarellaCheeseModifier(),
            _ => null
        };
    }
    
    public void Draw(Vector3 pos)
    {
        _paintable.Draw(pos);
    }
    
    public bool IsPainted(Vector3 mousePos)
    {
        return _paintable.IsPainted(mousePos);
    }
    
    public Sprite GetSprite()
    {
        return _paintable.GetSprite();
    }
    
    
    public void ModifyIngredient(PizzaIngredient pizzaIngredient)
    {
        _ingredientModifier?.ModifyIngredient(pizzaIngredient);
    }
}