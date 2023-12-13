using UnityEngine;

public class IngredientPlacer : MonoBehaviour
{
    private Pizza _pizza;

    [Header("Pizza")]
    [SerializeField] private RectTransform pizzaBase;
    [SerializeField] private float borderLength = 100;

    [Header("Paintables")]
    [SerializeField] private Paintable sauce;
    [SerializeField] private Paintable cheese;
    private Paintable _currentPaintable;
    
    [SerializeField] private Ingredient currentIngredient;

    private void Start()
    {
        _pizza = GetComponent<Pizza>();
        
        sauce.enabled = false;
        cheese.enabled = false;
        
        IngredientInventory.Clear();
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {        
            _pizza.ExitPizzaView();
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            _currentPaintable = cheese;
            cheese.enabled = true;
            sauce.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            _currentPaintable = sauce;
            sauce.enabled = true;
            cheese.enabled = false;
        }

        if (currentIngredient != null)
        {
            if (Input.GetMouseButtonDown(0)) PlaceIngredient();
        }
        else if (_currentPaintable != null && Input.GetMouseButton(0)) _currentPaintable.Paint(Input.mousePosition);
        
        if(Input.GetMouseButtonDown(1)) currentIngredient = null;
    }
    
    public void SetIngredient(Ingredient ingredient)
    {
        currentIngredient = ingredient;
    }
    
    private void PlaceIngredient()
    {
        Vector2 mousePos = Input.mousePosition;
        
        float limit = pizzaBase.rect.width * pizzaBase.localScale.x / 2f - borderLength;
        float distance = Vector2.Distance(mousePos, pizzaBase.position);
        if (distance > limit) return;
        
        if (IngredientInventory.GetIngredientQuantity(currentIngredient.Info) <= 0)
        {
            currentIngredient = null;
            return;
        }
        IngredientInventory.RemoveIngredient(currentIngredient.Info);
        
        _pizza.PlaceIngredient(currentIngredient.Info);
        Instantiate(currentIngredient, mousePos,  Quaternion.Euler(0, 0, Random.Range(0, 360)), pizzaBase.parent);
    }
}