using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [field: SerializeField] public IngredientInfo Info { get; private set; }
}