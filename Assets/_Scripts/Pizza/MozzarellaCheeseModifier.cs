using UnityEngine;

public class MozzarellaCheeseModifier : IIngrdientModifier
{
    public void ModifyIngredient(PizzaIngredient pizzaIngredient)
    {
        Ingredient ingredient = ScriptableObject.CreateInstance<Ingredient>();
        ingredient.Initialize(pizzaIngredient.Ingredient);
        for (int i = 0; i < ingredient.Buffs.Length; i++)
        {
            ingredient.Buffs[i] = new BuffData
            {
                multiplier = pizzaIngredient.Ingredient.Buffs[i].multiplier * 1.1f,
                incrementType = pizzaIngredient.Ingredient.Buffs[i].incrementType,
                buffType = pizzaIngredient.Ingredient.Buffs[i].buffType
            };
        }
        pizzaIngredient.Ingredient = ingredient;
    }
}