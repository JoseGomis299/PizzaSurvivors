public interface IIngrdientModifier
{
    public void ModifyIngredient(PizzaIngredient pizzaIngredient);
}

public enum BaseIngredientType
{
    MozzarellaCheese,
    TomatoSauce
}