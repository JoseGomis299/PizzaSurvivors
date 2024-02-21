using System;
using System.Collections.Generic;
using UnityEngine;

public class IngredientInventory 
{
    public static event Action OnInventoryChanged;

    private static Dictionary<Ingredient, int> _ingredients = new Dictionary<Ingredient, int>();
    public static IEnumerable<KeyValuePair<Ingredient, int>> Ingredients => _ingredients;

    [RuntimeInitializeOnLoadMethod]
    public static void Initialize()
    {
        _ingredients = new Dictionary<Ingredient, int>();
    }
    
    public static void Clear()
    {
        _ingredients.Clear();
    }
    
    public static void AddIngredient(Ingredient ingredient, int quantity = 1)
    {
        if (_ingredients.ContainsKey(ingredient)) _ingredients[ingredient] += quantity;
        else _ingredients.Add(ingredient, quantity);

        if (_ingredients[ingredient] <= 0)
            _ingredients.Remove(ingredient);
            
        OnInventoryChanged?.Invoke();
    }
    
    public static void RemoveIngredient(Ingredient ingredient, int quantity = 1)
    {
        if (_ingredients.ContainsKey(ingredient)) _ingredients[ingredient] -= quantity;
        else _ingredients.Add(ingredient, 0);
        
        if(_ingredients[ingredient] <= 0) 
            _ingredients.Remove(ingredient);
        
        OnInventoryChanged?.Invoke();
    }
    
    public static int GetIngredientQuantity(Ingredient ingredient)
    {
        return _ingredients.ContainsKey(ingredient) ? _ingredients[ingredient] : 0;
    }
}