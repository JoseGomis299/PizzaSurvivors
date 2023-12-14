using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class IngredientPlacer : MonoBehaviour
{
    private Pizza _pizza;
    
    public Action OnIngredientChanged;

    [Header("Pizza")]
    [SerializeField] private RectTransform pizzaBase;
    [SerializeField] private float borderLength = 100;

    [Header("Paintables")]
    [SerializeField] private Paintable sauce;
    [SerializeField] private Paintable cheese;
    
    private Paintable _currentPaintable;
    private Ingredient _currentIngredient;
    [SerializeField] private GameObject ingredientPrefab;

    private void Start()
    {
        _pizza = GetComponent<Pizza>();
        
        sauce.enabled = false;
        cheese.enabled = false;
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {        
            ExitPizzaView();
            return;
        }
        
        if (_currentIngredient != null)
        {
            if (Input.GetMouseButtonDown(0)) PlaceIngredient();
        }
        else if (_currentPaintable != null && Input.GetMouseButton(0)) 
            _currentPaintable.Paint(Input.mousePosition);

        if (Input.GetMouseButtonDown(1))
            ClearSelection();
    }
    
    public void SetIngredient(Ingredient ingredient)
    {
        _currentPaintable = null;
        _currentIngredient = ingredient;
        
        OnIngredientChanged?.Invoke();
    }
    
    public void StartPlacingCheese()
    {
        _currentIngredient = null;
        
        _currentPaintable = cheese;
        cheese.enabled = true;
        sauce.enabled = false;
        
        OnIngredientChanged?.Invoke();
    }
    
    public void StartPlacingSauce()
    {
        _currentIngredient = null;
        
        _currentPaintable = sauce;
        sauce.enabled = true;
        cheese.enabled = false;
        
        OnIngredientChanged?.Invoke();
    }
    
    public Sprite GetCurrentSprite()
    {
        if(_currentIngredient != null) return _currentIngredient.uiSprite;
        return _currentPaintable != null ? _currentPaintable.GetSprite() : null;
    }
    
    public void ExitPizzaView()
    {
        ClearSelection();
        
        _pizza.ExitPizzaView();
    }
    
    private void ClearSelection()
    {
        _currentPaintable = null;
        _currentIngredient = null;
        
        OnIngredientChanged?.Invoke();
    }
    
    private void PlaceIngredient()
    {
        if (IngredientInventory.GetIngredientQuantity(_currentIngredient) <= 0) return;

        Vector2 mousePos = Input.mousePosition;
        
        float limit = pizzaBase.rect.width * pizzaBase.localScale.x / 2f - borderLength;
        float distance = Vector2.Distance(mousePos, pizzaBase.position);
        if (distance > limit) return;
        
        IngredientInventory.RemoveIngredient(_currentIngredient);

        _pizza.PlaceIngredient(_currentIngredient);
        GameObject ingredient = Instantiate(ingredientPrefab, mousePos,  Quaternion.Euler(0, 0, Random.Range(0, 360)), pizzaBase.parent);
        ingredient.GetComponent<Image>().sprite = _currentIngredient.onPizzaSprite;
        
        if (IngredientInventory.GetIngredientQuantity(_currentIngredient) <= 0) ClearSelection();
    }
}