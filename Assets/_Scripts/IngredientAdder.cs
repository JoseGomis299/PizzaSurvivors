#if UNITY_EDITOR

using System;
using UnityEngine;

public class IngredientAdder : MonoBehaviour
{
    [Serializable]
    class IngredientPair
    {
        public Ingredient ingredient;
        public int amount;
    }
    [SerializeField] private IngredientPair[] ingredients;
   
    void Start()
    {
        foreach (var ingredient in ingredients)
        {
            IngredientInventory.AddIngredient(ingredient.ingredient, ingredient.amount);
        }
    }
}

#endif
