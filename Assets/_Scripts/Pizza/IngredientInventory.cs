using System.Collections.Generic;

public class IngredientInventory 
{
    private static readonly Dictionary<IngredientInfo, int> Ingredients = new();
    
    public static void Clear()
    {
        Ingredients.Clear();
    }
    
    public static void AddIngredient(IngredientInfo ingredient, int quantity = 1)
    {
        if (Ingredients.ContainsKey(ingredient)) Ingredients[ingredient] += quantity;
        else Ingredients.Add(ingredient, quantity);
        
        if(Ingredients[ingredient] < 0) 
            Ingredients[ingredient] = 0;
    }
    
    public static void RemoveIngredient(IngredientInfo ingredient, int quantity = 1)
    {
        if (Ingredients.ContainsKey(ingredient)) Ingredients[ingredient] -= quantity;
        else Ingredients.Add(ingredient, 0);
        
        if(Ingredients[ingredient] < 0) 
            Ingredients[ingredient] = 0;
    }
    
    public static int GetIngredientQuantity(IngredientInfo ingredient)
    {
        return Ingredients.ContainsKey(ingredient) ? Ingredients[ingredient] : 0;
    }
}