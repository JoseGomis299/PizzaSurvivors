using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PizzaIngredient : MonoBehaviour
{
    public Ingredient Ingredient;
    private Pizza _pizza;
    
    public void Initialize(Ingredient ingredient, Pizza pizza)
    {
        Ingredient = ingredient;
        _pizza = pizza;
        GetComponent<Image>().sprite = ingredient.onPizzaSprite;
    }
    
    public void RemoveFromPizza()
    {
        _pizza.RemoveIngredient(Ingredient);
        Destroy(gameObject);
    }
}