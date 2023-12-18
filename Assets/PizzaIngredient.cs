using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PizzaIngredient : MonoBehaviour
{
    private Ingredient _ingredientType;
    private Pizza _pizza;
    
    public void Initialize(Ingredient ingredient, Pizza pizza)
    {
        _ingredientType = ingredient;
        _pizza = pizza;
        GetComponent<Image>().sprite = ingredient.onPizzaSprite;
    }
    
    public void RemoveFromPizza()
    {
        _pizza.RemoveIngredient(_ingredientType);
        Destroy(gameObject);
    }
}